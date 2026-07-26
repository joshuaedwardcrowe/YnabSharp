# Contributing to YnabSharp

YnabSharp wraps a real financial API — the process below optimizes for
one thing above all: **every non-obvious decision has a paper trail, and
nothing that touches money or a real budget ships without being able to
explain itself.** If someone asks "why does X work this way" in a year,
the answer should be a link, not archaeology.

## Before you write code

- **Bugs and small fixes** — just open a PR. No issue required for
  anything you can describe in the PR description.
- **Features, breaking changes, or anything touching money conversion,
  authentication, or the Seeder's write path** — open an issue first
  using the [feature request template](.github/ISSUE_TEMPLATE/feature_request.yml).
  Get a shape agreed before investing in the implementation — this
  library writes to real accounts, mistakes are expensive to unwind.
- **Architectural decisions** — see [ADRs](#adrs) below.

## Branching & PRs

- Branch off `main`. No long-running branches — trunk-based, short-lived.
- One logical change per PR.
- **PR titles use [Conventional Commits](https://www.conventionalcommits.org/):**
  `<type>(scope): <description>` — `type` is one of `feat` `fix` `docs`
  `chore` `refactor` `test` `ci`; `scope` is optional and, where it
  applies, matches an [issue area](#issues) (`client`, `domain`,
  `seeder`, `tooling`). `description` is lowercase, imperative, no
  trailing period. For a breaking change, add `!` right before the colon
  — `fix(domain)!: make NewAccount.ClearedBalance hold pounds, not
  milliunits` — in addition to checking **Breaking change** in the PR
  body. This becomes the squash-merge commit title, so it's also the
  CHANGELOG line — get it right here and there's nothing to rewrite
  later.
- Fill in the [PR template](.github/PULL_REQUEST_TEMPLATE.md) — in
  particular, link the issue if one exists, and say how you tested it.
- CI (`dotnet build` + `dotnet test`) must be green before merge — this
  is enforced by branch protection, not discipline.
- At least one approving review is required. See [CODEOWNERS](CODEOWNERS).
- We squash-merge, so the PR title ends up as the commit title on `main`
  and the changelog line.

## ADRs

An [ADR](docs/adr/) (Architecture Decision Record) captures a decision,
its alternatives, and its consequences — not how something works today
(that's [`docs/concepts/`](docs/concepts/)).

**Write one when you're:**
- Introducing a new cross-cutting pattern
- Changing how money is represented or converted anywhere in the library
- Making a breaking change to public API shape
- Reversing a previous ADR

**Skip it for:** bug fixes, internal refactors, anything a code comment
already explains. If you're not sure, err toward not writing one.

Copy [`docs/adr/0000-template.md`](docs/adr/0000-template.md), number it
sequentially, and open it in the same PR as the change it justifies (or
on its own if the decision precedes the implementation).

## Concepts

A [concept doc](docs/concepts/) explains how a subsystem works today —
narrative, examples, a Q&A — the opposite of an ADR's terse
decision-record shape. YNAB's API has real, easy-to-miss domain quirks
(milliunit currency representation, split transactions, empty-string-
instead-of-null enum fields) — write a concept doc for anything a new
consumer of this library would need onboarding to.

Copy [`docs/concepts/0000-template.md`](docs/concepts/0000-template.md).
Verify every claim, class/method name, and signature against the actual
source before writing it down — for a library that moves real money, a
concept doc describing behavior that doesn't match `main` is actively
dangerous, not just stale.

**Keep them current.** If your change makes an existing concept doc
inaccurate, update it in the same PR — don't leave the drift for someone
else to notice later.

## Issues

Every issue gets three independent labels once triaged:

| Axis | Values |
|---|---|
| **Type** | `bug` · `feature` · `tech-debt` · `docs` · `process` |
| **Area** | `area:client` · `area:domain` · `area:seeder` · `area:tooling` |
| **Severity** | `sev:high` · `sev:medium` · `sev:low` |

Use the matching [issue template](.github/ISSUE_TEMPLATE/). There's no
fixed triage meeting; an issue should have an area label within about a
week or it's fair game to close as stale.

## Versioning & releases

`YnabSharp` (the library) is the only published package — standard
semver, bumped in `YnabSharp/YnabSharp.csproj`. `YnabSharp.Seeder` is not
published; its version doesn't need to track the library's.

Every squash-merged PR that changes behavior gets a line in
[`CHANGELOG.md`](CHANGELOG.md) under `[Unreleased]`, in [Keep a
Changelog](https://keepachangelog.com/) format.

## A note on the Seeder

`YnabSharp.Seeder` writes real data to a real YNAB budget via the live
API — there is no sandbox mode. Any change to its write path (account
creation, transaction creation, the confirmation step) should be treated
with the same care as a database migration: understand exactly what it
does before it runs, and never run it against a budget you're not
prepared to clean up by hand.

## Questions

Open a `process`-labeled issue if something in this document is unclear
or actively getting in the way.
