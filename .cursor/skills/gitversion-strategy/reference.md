# GitVersion 6 — reference (Cross.CQRS)

## Built-in branch keys

GV6 keeps default `main` / `develop`. Custom keys `master` / `dev` **conflict** → `-1` pre-release on master, `alpha` instead of `dev`.

| Git branch | YAML key | regex |
|---|---|---|
| `master` / `main` | `main` | `^master$\|^main$` |
| `dev` / `develop` / `development` | `develop` | `^dev(elop)?(ment)?$` |

`source-branches` lists **keys**, not git names.

## Strategies

Typical set without name-versioning:

```yaml
strategies:
  - Fallback
  - ConfiguredNextVersion
  - MergeMessage
  - TaggedCommit
  - TrackReleaseBranches
```

Omit `VersionInBranchName` so `release/11.0.0-…` ≡ `release/test`.

Also set:

```yaml
version-in-branch-pattern: '(?<version>DO_NOT_MATCH_VERSION_IN_BRANCH_NAME)'
```

and `track-merge-message: false` on `main` / `release` / `hotfix` so squash message `Merge branch 'release/11.0.0-…'` does not inject `11.0.0`.

## `of-version-based-on-name`

Not present in GitVersion.Tool **6.8.2** config model (stripped from `/showconfig`). Do not rely on it.

## ContinuousDeployment on main

With `label: ''` and `increment: Minor|Patch|…`:

- tagged tip → tag SemVer (needs `when-current-commit-tagged: true`)
- first commit after tag → one bump
- further commits → **same** SemVer until next tag (`+1` and `+2` both `10.2.0` is expected)

## Trade-off: main.increment

| `main.increment` | release→master | hotfix→master | direct push |
|---|---|---|---|
| `Inherit` | Minor MMP | **Patch** MMP | needs sibling branches or FAIL if orphan; may follow develop |
| `Minor` | Minor MMP | **Minor** MMP (Patch lost) | one Minor after tag |
| `Patch` | **Patch** MMP (release Minor lost) | Patch MMP | one **Patch** after tag (repo default for master push) |
| `None` | stays near tag | stays near tag | often no bump |

Pure YAML cannot satisfy «release Minor + hotfix Patch on master + sterile direct Minor» simultaneously without CI pin.

## Do not

- Run GV on the full production clone for matrix (hang risk)
- Use GV5 keys (`tag:` → use `label:`)
- Put secrets / commit from this skill
