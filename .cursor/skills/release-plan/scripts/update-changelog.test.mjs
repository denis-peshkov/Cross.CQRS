import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import {
  buildGroupedBullets,
  formatSection,
  upsertChangelog,
} from './update-changelog.mjs';

describe('buildGroupedBullets', () => {
  it('groups paths into changelog categories', () => {
    const groups = buildGroupedBullets({
      paths: [
        '.github/workflows/dotnet.yml',
        'GitVersion.yml',
        'CONTRIBUTING.md',
        '.cursor/skills/gitversion-strategy/scripts/run-matrix.mjs',
        'docs/CHANGELOG.md',
      ],
      subjects: ['init', 'work', 'update GitVersion configuration'],
    });
    assert.ok(groups['CI / release process']?.length);
    assert.ok(groups.Versioning?.length);
    assert.ok(groups.Documentation?.length);
    assert.ok(groups['Repository tooling']?.length);
    assert.equal(groups.Library, undefined);
  });
});

describe('upsertChangelog', () => {
  const section = formatSection({
    version: '11.1.1',
    date: '13 Sep 2026',
    groups: {
      'CI / release process': ['CI workflow updated.'],
    },
  });

  it('inserts after intro --- when missing', () => {
    const md = '\uFEFF# Changelog\n\nIntro.\n\n---\n\n## v11.1.0 — 13 Sep 2026\n\n- old\n\n---\n';
    const result = upsertChangelog(md, section, '11.1.1');
    assert.equal(result.status, 'inserted');
    assert.ok(result.markdown.startsWith('\uFEFF'));
    assert.match(result.markdown, /## v11\.1\.1 — 13 Sep 2026/);
    assert.ok(result.markdown.indexOf('## v11.1.1') < result.markdown.indexOf('## v11.1.0'));
  });

  it('updates an existing section', () => {
    const first = upsertChangelog(
      '\uFEFF# Changelog\n\n---\n\n## v11.1.0 — 1 Jan 2026\n\n- x\n\n---\n',
      section,
      '11.1.1',
    );
    const second = upsertChangelog(
      first.markdown,
      formatSection({
        version: '11.1.1',
        date: '13 Sep 2026',
        groups: { Versioning: ['GitVersion.yml updated.'] },
      }),
      '11.1.1',
    );
    assert.equal(second.status, 'updated');
    assert.match(second.markdown, /GitVersion\.yml updated/);
    assert.equal((second.markdown.match(/## v11\.1\.1/g) || []).length, 1);
  });

  it('reports up-to-date when identical', () => {
    const base = '\uFEFF# Changelog\n\n---\n\n';
    const once = upsertChangelog(base, section, '11.1.1');
    const twice = upsertChangelog(once.markdown, section, '11.1.1');
    assert.equal(twice.status, 'up-to-date');
  });
});
