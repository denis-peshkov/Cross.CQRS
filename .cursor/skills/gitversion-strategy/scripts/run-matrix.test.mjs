#!/usr/bin/env node
/**
 * Golden SemVer matrix: stdout/JSON of run-matrix.mjs must match the canonical table.
 * Does not import or modify run-matrix.mjs — only spawns it.
 *
 * Usage (from repo root):
 *   node --test .cursor/skills/gitversion-strategy/scripts/run-matrix.test.mjs
 */
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import { spawnSync } from 'node:child_process';
import { resolve } from 'node:path';

/** Canonical expected matrix (base tag v10.1.3). */
export const EXPECTED = {
  base_tag: 'v10.1.3',
  rows: [
    {
      branch: 'release/11.0.0-new',
      source: '10.2.0-preview.1',
      push: '10.2.0-preview.2',
      squash: '10.2.0',
      merge: '10.2.0',
    },
    {
      branch: 'release/test-12-new',
      source: '10.2.0-preview.1',
      push: '10.2.0-preview.2',
      squash: '10.2.0',
      merge: '10.2.0',
    },
    {
      branch: 'release/test',
      source: '10.2.0-preview.1',
      push: '10.2.0-preview.2',
      squash: '10.2.0',
      merge: '10.2.0',
    },
    {
      branch: 'hotfix/test',
      source: '10.1.4-preview.1',
      push: '10.1.4-preview.2',
      squash: '10.1.4',
      merge: '10.1.4',
    },
    {
      branch: 'dev',
      source: '11.3.3-dev.1',
      push: '11.3.3-dev.2',
      squash: '11.3.3',
      merge: '11.3.3',
    },
  ],
  direct: {
    at_tag: '10.1.3',
    push1_one: '10.1.4',
    push1_ten: '10.1.4',
    push2: '10.1.5',
  },
};

const EXPECTED_MARKDOWN = `База: **\`v10.1.3\`**. Merge = \`--no-ff\`. Колонка **push** = +1 коммит на **source**-ветке.

| Ветка (source) | SemVer на source | после push (+1) | после squash | после merge |
|---|---|---|---|---|
| \`release/11.0.0-new\` | \`10.2.0-preview.1\` | \`10.2.0-preview.2\` | \`10.2.0\` | \`10.2.0\` |
| \`release/test-12-new\` | \`10.2.0-preview.1\` | \`10.2.0-preview.2\` | \`10.2.0\` | \`10.2.0\` |
| \`release/test\` | \`10.2.0-preview.1\` | \`10.2.0-preview.2\` | \`10.2.0\` | \`10.2.0\` |
| \`hotfix/test\` | \`10.1.4-preview.1\` | \`10.1.4-preview.2\` | \`10.1.4\` | \`10.1.4\` |
| \`dev\` | \`11.3.3-dev.1\` | \`11.3.3-dev.2\` | \`11.3.3\` | \`11.3.3\` |

**Direct push в \`master\`:**

| Состояние | SemVer |
|---|---|
| на теге \`v10.1.3\` | \`10.1.3\` |
| push #1 = 1 коммит | \`10.1.4\` |
| push #1 = 10 коммитов | \`10.1.4\` |
| push #2 = 1 коммит (после CI-тега \`v10.1.4\`) | \`10.1.5\` |
`;

const SCRIPT = resolve('.cursor/skills/gitversion-strategy/scripts/run-matrix.mjs');

function runMatrixCli(args) {
  const p = spawnSync(process.execPath, [SCRIPT, ...args], {
    encoding: 'utf8',
    cwd: resolve('.'),
    timeout: 300_000,
  });
  if (p.status !== 0) {
    throw new Error(`run-matrix.mjs failed (${p.status}): ${(p.stderr || p.stdout || '').slice(-500)}`);
  }
  return p.stdout;
}

describe('GitVersion matrix golden', () => {
  it('JSON matches canonical SemVer table for GitVersion.yml', () => {
    const stdout = runMatrixCli(['--config', 'GitVersion.yml', '--base-tag', 'v10.1.3', '--json']);
    const result = JSON.parse(stdout);

    assert.equal(result.base_tag, EXPECTED.base_tag);
    assert.equal(result.rows.length, EXPECTED.rows.length);
    for (let i = 0; i < EXPECTED.rows.length; i++) {
      const exp = EXPECTED.rows[i];
      const got = result.rows[i];
      assert.deepEqual(
        {
          branch: got.branch,
          source: got.source,
          push: got.push,
          squash: got.squash,
          merge: got.merge,
        },
        exp,
        `row ${exp.branch}`,
      );
    }
    assert.deepEqual(result.direct, EXPECTED.direct, 'direct push table');
  });

  it('markdown matches canonical report template fill', () => {
    const md = runMatrixCli(['--config', 'GitVersion.yml', '--base-tag', 'v10.1.3']);
    assert.equal(md, EXPECTED_MARKDOWN);
  });
});
