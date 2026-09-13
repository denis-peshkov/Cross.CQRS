#!/usr/bin/env node
/**
 * GitVersion strategy matrix runner for Cross.CQRS fixtures.
 *
 * Collects SemVer measurements from GitVersion.yml fixtures ONLY, then fills
 * templates/MATRIX-REPORT.md. No /overrideconfig, no CI emulation, no retries
 * that change SemVer — FAIL cells must stay FAIL if YAML fails.
 *
 * Usage:
 *   node .cursor/skills/gitversion-strategy/scripts/run-matrix.mjs
 *   node .cursor/skills/gitversion-strategy/scripts/run-matrix.mjs --config GitVersion.yml --base-tag v10.1.3
 *   node .cursor/skills/gitversion-strategy/scripts/run-matrix.mjs --json
 */
import { spawnSync } from 'node:child_process';
import { existsSync, mkdirSync, mkdtempSync, readFileSync, rmSync, writeFileSync } from 'node:fs';
import { homedir } from 'node:os';
import { dirname, join, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const SCRIPT_DIR = dirname(fileURLToPath(import.meta.url));
const SKILL_DIR = dirname(SCRIPT_DIR);
const TEMPLATE_PATH = join(SKILL_DIR, 'templates', 'MATRIX-REPORT.md');

const DEFAULT_BRANCHES = [
  'release/11.0.0-new',
  'release/test-12-new',
  'release/test',
  'hotfix/test',
  'dev',
];

/** Manual develop train in the matrix only (not applied to release/hotfix rows). */
const DEV_NEXT_VERSION = '11.3.3';

function usage() {
  return `Usage:
  node .cursor/skills/gitversion-strategy/scripts/run-matrix.mjs [options]

Options:
  --config PATH     GitVersion.yml (default: ./GitVersion.yml)
  --base-tag TAG    Baseline git tag (default: v10.1.3)
  --gv PATH         Path to dotnet-gitversion
  --branch NAME     Branch to test (repeatable; default built-in set)
  --json            Emit JSON instead of markdown
  --workdir PATH    Fixture root (default: temp under .tmp-gvfind/matrix)
  -h, --help        Show help`;
}

function parseArgs(argv) {
  const args = {
    config: 'GitVersion.yml',
    baseTag: 'v10.1.3',
    gv: null,
    branches: [],
    json: false,
    workdir: null,
  };
  for (let i = 0; i < argv.length; i++) {
    const a = argv[i];
    if (a === '-h' || a === '--help') {
      console.log(usage());
      process.exit(0);
    }
    if (a === '--json') {
      args.json = true;
      continue;
    }
    if (a === '--config' || a === '--base-tag' || a === '--gv' || a === '--branch' || a === '--workdir') {
      const v = argv[++i];
      if (v == null) {
        throw new Error(`missing value for ${a}`);
      }
      if (a === '--config') args.config = v;
      else if (a === '--base-tag') args.baseTag = v;
      else if (a === '--gv') args.gv = v;
      else if (a === '--branch') args.branches.push(v);
      else if (a === '--workdir') args.workdir = v;
      continue;
    }
    throw new Error(`unknown argument: ${a}\n${usage()}`);
  }
  return args;
}

function cfgForBranch(cfgText, branch) {
  if (branch === 'dev' || branch === 'develop' || branch.startsWith('dev/')) {
    return cfgText.replace(/^next-version:\s*.*$/m, `next-version: ${DEV_NEXT_VERSION}`);
  }
  return cfgText;
}

function findGv(explicit) {
  if (explicit) return explicit;
  const which = spawnSync('which', ['dotnet-gitversion'], { encoding: 'utf8' });
  if (which.status === 0 && which.stdout.trim()) {
    return which.stdout.trim();
  }
  const home = join(homedir(), '.dotnet', 'tools', 'dotnet-gitversion');
  if (existsSync(home)) return home;
  throw new Error('dotnet-gitversion not found (install GitVersion.Tool)');
}

function parseJsonBlob(stdout) {
  const start = stdout.indexOf('{');
  if (start < 0) return null;
  let depth = 0;
  for (let i = start; i < stdout.length; i++) {
    const ch = stdout[i];
    if (ch === '{') depth++;
    else if (ch === '}') {
      depth--;
      if (depth === 0) {
        try {
          return JSON.parse(stdout.slice(start, i + 1));
        } catch {
          return null;
        }
      }
    }
  }
  return null;
}

function runGv(gv, cwd) {
  const p = spawnSync(gv, ['/targetpath', cwd, '/output', 'json'], {
    encoding: 'utf8',
    timeout: 90_000,
    env: process.env,
  });
  const stdout = p.stdout ?? '';
  const stderr = p.stderr ?? '';
  const data = parseJsonBlob(stdout);
  if (!data) {
    const err = stderr + stdout;
    if (err.includes('No base versions')) return 'FAIL:no-base-versions';
    if (err.toLowerCase().includes('orphaned')) return 'FAIL:orphaned-branch';
    return 'FAIL';
  }
  return String(data.SemVer ?? '?');
}

function git(cwd, args, { check = true, capture = false } = {}) {
  const p = spawnSync('git', args, {
    cwd,
    encoding: 'utf8',
    env: { ...process.env, GIT_TEMPLATE_DIR: '' },
  });
  if (check && p.status !== 0) {
    const msg = (p.stderr || p.stdout || '').trim() || `git ${args.join(' ')} failed (${p.status})`;
    throw new Error(msg);
  }
  return p;
}

/**
 * Minimal clone: tagged master + optional `dev` tip at the same commit.
 *
 * Fixture contract (stable — do not tweak per-run to force SemVer):
 * - always seed `dev` when withDev=true (real repos have it after fetch);
 * - never seed leftover `release/*` / `hotfix/*` placeholders.
 */
function initRepo(path, cfgText, baseTag, { withDev = true } = {}) {
  rmSync(path, { recursive: true, force: true });
  mkdirSync(path, { recursive: true });
  git(path, ['init', '-b', 'master', '-q']);
  for (const [k, v] of [
    ['user.email', 'gitversion-matrix@local'],
    ['user.name', 'gitversion-matrix'],
    ['core.hooksPath', '/dev/null'],
    ['core.autocrlf', 'false'],
  ]) {
    git(path, ['config', k, v]);
  }
  writeFileSync(join(path, 'GitVersion.yml'), cfgText, 'utf8');
  writeFileSync(join(path, 'f.txt'), 'base\n', 'utf8');
  git(path, ['add', '-A']);
  git(path, ['commit', '-qm', 'init']);
  git(path, ['tag', baseTag]);
  if (withDev) {
    git(path, ['branch', 'dev']);
  }
}

function commitWork(path, branch) {
  const existing = git(path, ['show-ref', '--verify', '--quiet', `refs/heads/${branch}`], {
    check: false,
  });
  if (existing.status === 0) {
    git(path, ['checkout', '-q', branch]);
  } else {
    git(path, ['checkout', '-qb', branch]);
  }
  writeFileSync(join(path, 'f.txt'), `${branch}\n`, 'utf8');
  git(path, ['add', '-A']);
  git(path, ['commit', '-qm', `work on ${branch}`]);
}

function masterMergeMessage(branch) {
  return `Merge branch '${branch}'`;
}

function runMatrix({ gv, cfgText, baseTag, branches, workRoot }) {
  const rows = [];
  for (const branch of branches) {
    const branchCfg = cfgForBranch(cfgText, branch);
    const safe = branch.replaceAll('/', '_');

    const dSrc = join(workRoot, `src_${safe}`);
    initRepo(dSrc, branchCfg, baseTag);
    commitWork(dSrc, branch);
    const source = runGv(gv, dSrc);
    writeFileSync(join(dSrc, 'f.txt'), `${branch} push+1\n`, 'utf8');
    git(dSrc, ['add', '-A']);
    git(dSrc, ['commit', '-qm', `push +1 on ${branch}`]);
    const afterPush = runGv(gv, dSrc);

    const dSq = join(workRoot, `sq_${safe}`);
    initRepo(dSq, branchCfg, baseTag);
    commitWork(dSq, branch);
    const mergeMsg = masterMergeMessage(branch);
    git(dSq, ['checkout', '-q', 'master']);
    git(dSq, ['merge', '--squash', branch], { capture: true });
    git(dSq, ['commit', '-qm', mergeMsg]);
    const afterSquash = runGv(gv, dSq);

    const dMg = join(workRoot, `mg_${safe}`);
    initRepo(dMg, branchCfg, baseTag);
    commitWork(dMg, branch);
    git(dMg, ['checkout', '-q', 'master']);
    const mr = git(dMg, ['merge', '--no-ff', '-m', mergeMsg, branch], {
      check: false,
      capture: true,
    });
    const afterMerge = mr.status !== 0 ? 'FAIL' : runGv(gv, dMg);

    rows.push({
      branch,
      source,
      squash: afterSquash,
      merge: afterMerge,
      push: afterPush,
    });
  }

  const d = join(workRoot, 'direct');
  initRepo(d, cfgText, baseTag, { withDev: true });
  const atTag = runGv(gv, d);

  writeFileSync(join(d, 'f.txt'), 'push1-c1\n', 'utf8');
  git(d, ['add', '-A']);
  git(d, ['commit', '-qm', 'push1 one commit']);
  git(d, ['branch', '-f', 'dev', 'HEAD']);
  const push1One = runGv(gv, d);

  const d10 = join(workRoot, 'direct_10');
  initRepo(d10, cfgText, baseTag, { withDev: true });
  for (let i = 1; i <= 10; i++) {
    writeFileSync(join(d10, 'f.txt'), `push1-c${i}\n`, 'utf8');
    git(d10, ['add', '-A']);
    git(d10, ['commit', '-qm', `push1 commit ${i}/10`]);
  }
  git(d10, ['branch', '-f', 'dev', 'HEAD']);
  const push1Ten = runGv(gv, d10);

  let push2;
  if (push1One.startsWith('FAIL') || push1One.includes('-')) {
    push2 = push1One;
  } else {
    git(d, ['tag', `v${push1One}`]);
    writeFileSync(join(d, 'f.txt'), 'push2-c1\n', 'utf8');
    git(d, ['add', '-A']);
    git(d, ['commit', '-qm', 'push2 one commit']);
    git(d, ['branch', '-f', 'dev', 'HEAD']);
    push2 = runGv(gv, d);
  }

  return {
    base_tag: baseTag,
    rows,
    direct: {
      at_tag: atTag,
      push1_one: push1One,
      push1_ten: push1Ten,
      push2,
    },
  };
}

function loadReportTemplate() {
  return readFileSync(TEMPLATE_PATH, 'utf8');
}

/** Fill templates/MATRIX-REPORT.md with measured matrix cells. */
function renderMarkdown(result) {
  const tag = result.base_tag;
  const branchRows = result.rows
    .map(
      (row) =>
        `| \`${row.branch}\` | \`${row.source}\` | \`${row.push}\` | \`${row.squash}\` | \`${row.merge}\` |`,
    )
    .join('\n');
  const d = result.direct;
  let text = loadReportTemplate();
  const replacements = {
    '{{BASE_TAG}}': tag,
    '{{BRANCH_ROWS}}': branchRows,
    '{{DIRECT_AT_TAG}}': d.at_tag,
    '{{DIRECT_PUSH1_ONE}}': d.push1_one,
    '{{DIRECT_PUSH1_TEN}}': d.push1_ten,
    '{{DIRECT_PUSH2}}': d.push2,
  };
  for (const [key, value] of Object.entries(replacements)) {
    text = text.split(key).join(value);
  }
  if (!text.endsWith('\n')) text += '\n';
  return text;
}

function main() {
  const args = parseArgs(process.argv.slice(2));
  const cfgPath = resolve(args.config);
  if (!existsSync(cfgPath)) {
    throw new Error(`config not found: ${cfgPath}`);
  }
  const cfgText = readFileSync(cfgPath, 'utf8').replace(/\r\n/g, '\n');

  const repo = process.cwd();
  let workRoot;
  let cleanup = false;
  if (args.workdir) {
    workRoot = resolve(args.workdir);
    mkdirSync(workRoot, { recursive: true });
  } else {
    const base = join(repo, '.tmp-gvfind', 'matrix');
    mkdirSync(base, { recursive: true });
    workRoot = mkdtempSync(join(base, 'run-'));
    cleanup = true;
  }

  const gv = findGv(args.gv);
  const branches = args.branches.length ? args.branches : DEFAULT_BRANCHES;

  let result;
  try {
    result = runMatrix({
      gv,
      cfgText,
      baseTag: args.baseTag,
      branches,
      workRoot,
    });
  } finally {
    if (cleanup) {
      rmSync(workRoot, { recursive: true, force: true });
    }
  }

  if (args.json) {
    process.stdout.write(`${JSON.stringify(result, null, 2)}\n`);
  } else {
    process.stdout.write(renderMarkdown(result));
  }
}

try {
  main();
} catch (err) {
  console.error(err instanceof Error ? err.message : err);
  process.exit(1);
}
