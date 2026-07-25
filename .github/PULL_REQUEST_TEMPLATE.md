<!--
Title must follow Conventional Commits: <type>(scope): <description>
  types: feat | fix | docs | chore | refactor | test | ci
  scope (optional): client | domain | seeder | tooling
  breaking change: add "!" right before the colon, e.g. fix(domain)!: ...
  example: fix(domain): stop double-converting NewAccount balances to milliunits
  example (breaking): fix(domain)!: make NewAccount.ClearedBalance hold pounds
Description is lowercase, imperative mood, no trailing period, no "fix stuff."
This becomes the squash-merge commit title, i.e. the CHANGELOG line.
-->

## What

## Why

Linked issue: #

## How

## Tested

- [ ] Unit tests
- [ ] Manual (describe how, especially for anything touching the Seeder's write path or currency conversion)

## Kind of change

- [ ] Bug fix
- [ ] Feature
- [ ] Refactor
- [ ] Breaking change
- [ ] Tech debt
- [ ] Docs / process

If **Breaking change** or a new cross-cutting pattern: needs an ADR in
`docs/adr/` in this PR. If this changes behavior: update `CHANGELOG.md`
under `[Unreleased]`. If this changes behavior a concept doc describes:
update that doc in `docs/concepts/`. If this touches currency conversion
or the Seeder's write path: extra scrutiny, this moves real money.
