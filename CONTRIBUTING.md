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
- **Keep PRs small: max 20 files, 10-15 preferred.** If a change is going
  to blow past that, plan the split into multiple PRs upfront, not after
  the fact.
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
- **If a linked issue exists, mirror its labels and milestone onto the
  PR.** GitHub doesn't do this automatically. Keeping both in sync means
  milestone/label filtering and progress tracking work across the PR
  list too, not just issues.
- CI (`dotnet build` + `dotnet test`) must be green before merge — this
  is enforced by branch protection, not discipline.
- No approving review is enforced while there's a single maintainer —
  they're also always the PR author, so GitHub won't let them approve
  their own PR anyway. [CODEOWNERS](CODEOWNERS) still maps areas to
  owners; turn required-review branch protection back on once a second
  maintainer joins.
- We squash-merge, so the PR title ends up as the commit title on `main`
  and the changelog line.
- Docs-only changes (like this file) can be committed straight to
  `main`.

## Testing

- **Build test doubles reusably from the start, not as a private nested
  class you promote later.** If a sibling class carries a
  `// TODO: Write unit tests.` marker, that's a signal more tests of the
  same shape are coming — put the double in `YnabSharp.Tests/TestHelpers/`
  the first time, not the second.
- **Serialize real DTOs instead of hand-writing JSON string literals**
  for canned API response bodies. Construct the actual response type
  (e.g. `PlanResponse`, `GetPlanResponseData`) and serialize it with
  `JsonSerializer.Serialize` — the fixture can't drift from the real
  wire shape, and a `[JsonPropertyName]` rename breaks the build instead
  of silently invalidating the test.
- **Name test doubles `Test*`** (`TestHttpMessageHandler`,
  `TestHttpClientFactory`), not `Stub*`/`Fake*`/`Mock*`.

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

## Investigations

An investigation ([`docs/investigations/`](docs/investigations/)) is what
a technical spike produces — the finding, not the code, since there's no
pair here to carry it forward otherwise. Number sequentially
(`000x-question.md`) from
[`0000-template.md`](docs/investigations/0000-template.md); skip it if a
reader wouldn't otherwise re-derive the finding from scratch.

Lead with the verdict — **new complexity** or **no new complexity** — not
the evidence; the latter closes the spike and opens a fresh ticket. An
investigation records what was found, not what was decided — that's an
ADR, which it may justify but doesn't replace. Ships through a PR with a
Status like any other work; durable facts about a dependency also belong
in that dependency's own docs, not only here.

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

**Issue titles** follow a two-stage convention:

- **Idea-stage** (unvalidated, pre-WAG) — plain-language problem
  statements, e.g. "No way to X" / "Y doesn't handle Z". This is
  deliberate: an idea is a pitch for an unmet need, not yet a scoped
  unit of work.
- **Delivery-stage sub-issues** (carved out by a planning spike, see
  [Projects](#projects) below, ready to build) — Conventional Commits
  style, matching PR titles: `type(scope): description`, e.g.
  `fix(domain)!: make NewAccount.ClearedBalance hold pounds, not
  milliunits`. By this point the work is scoped, so the title should
  read like the commit that will close it.

## Projects

Work bigger than a single issue goes through a pipeline biased toward
re-planning over predicting — estimates are inputs to prioritization,
not commitments to defend:

1. **WAG** — a fast, rough gut-feel estimate (in months), logged on
   [YnabSharp's own Ideas board](https://github.com/users/joshuaedwardcrowe/projects/14)'s
   `WAG (months)` field, purely to judge whether an idea is worth
   pursuing at all. Non-binding — expected to be wrong. Ideas that
   don't pertain to this repo specifically (cross-cutting, or not yet
   homed anywhere) go on the [shared personal-account Ideas
   board](https://github.com/users/joshuaedwardcrowe/projects/10)
   instead.
2. **SWAG** — the same estimate, re-checked against everything else
   competing for the slot, logged in the same board's `SWAG (months)`
   field. **Setting `Priority` (`High`/`Medium`/`Low`) is mandatory at
   this point** — Status can't move to `SWAG'd / Prioritized` until
   it's set, forcing an explicit call on how the idea stacks up against
   what's already prioritized. "Prioritizing" then means
   sorting/grouping the board by `Priority` or `SWAG` — there's no
   separate roadmap artifact to keep in sync. Still non-binding: a
   relative sizing input, not a plan.
3. **New GitHub Project** — once an idea is greenlit, it graduates off
   the Ideas board into its own project.
4. **Inception spike** — plans the *next* milestone in real detail;
   everything beyond that is a rough forecast, re-planned properly once
   you actually get there (rolling-wave planning, not a full plan for
   the whole estimate up front). Refresh the Ideas board's `Validated
   Estimate (months)` field as it's learned, not just once.
5. **Backlog refinement, just-in-time** — rather than one big spike
   producing the full chronological order for an entire milestone, only
   the next handful of tickets need to be fully ordered and estimated
   at any moment. The rest of the milestone stays a loosely-ordered
   backlog, refined incrementally as work proceeds. A milestone-scale
   re-planning pass is still useful when picking up a milestone cold —
   treat its output as a starting point, not a fixed contract.

   A **spike** (a specific, scoped investigation — "should we support
   X," "what does Y actually look like") resolves to one of two
   outcomes: **new complexity found**, or **no new complexity**. On no
   new complexity, close the spike and open a fresh, cleanly-titled
   delivery-stage ticket for the actual build — don't retitle or reuse
   the spike issue in place. That new ticket gets sized in a normal
   backlog-refinement pass, not as part of the spike itself.
6. **Fixed-length iterations + end-of-iteration review** — work in
   short, regular iterations rather than open-ended milestone spans.
   At the end of each one: check what actually got done vs. planned,
   re-prioritize the backlog based on what was learned, and feed the
   iteration's actual pace back into WAG/SWAG calibration. This
   inspect-and-adapt step is what keeps the rest of the pipeline
   honest — without it, WAG/SWAG/the inception spike are just a plan
   nobody revisits.
7. **Tickets with Estimates** — the leaf/actionable tickets pulled into
   an iteration get the `Estimate` field (Fibonacci story points, not
   time) on the project board — the parent story tracks the outcome,
   not the effort to reach it. Don't second-guess an estimate just
   because a ticket is taking a while — re-estimate only on genuine
   scope change; see [SoloCAIRN's Sizing
   note](https://github.com/joshuaedwardcrowe/SoloCAIRN/blob/main/docs/03-lifecycle.md)
   for the full reasoning.

   **When a ticket gets split, clear its `Estimate`.** Once the work
   lives on sub-issues, an estimate left on the parent is counted
   twice, and if the parent is then closed as superseded those points
   land in `Done` without any code behind them — silently inflating
   velocity, which is calculated from exactly that number. The
   invariant: *a `Done` item carrying an `Estimate` has a closing PR.*
   This is checked weekly by
   [`board-estimate-audit.yml`](.github/workflows/board-estimate-audit.yml)
   across this repo's delivery boards, because it's a mechanical
   always-rule and those belong in a machine rather than in anyone's
   memory (see SoloCAIRN's philosophy §9). The audit only flags — it
   never edits the board.

### Status gates

Once a ticket has an Estimate (step 7 above), the [YnabSharp API
Coverage project](https://github.com/users/joshuaedwardcrowe/projects/11)'s
`Status` field tracks it through these Agile/Scrum-style gates. Move the
card at each transition — don't let it sit stale while the real work
moves past it:

| Status | Move here when |
|---|---|
| `Backlog` | Triaged (has all three labels) but not yet pulled into an iteration. |
| `To Do` | Pulled into the current iteration and given an `Estimate`. |
| `In Development` | Implementation starts — first commit/branch pushed. |
| `In Review` | PR opened, labels/milestone mirrored from the issue. |
| `In QA` | All PR review threads resolved and CI is green — final verification before merge. |
| `Ready for Release` | Merged, but deliberately held back from `Done` — e.g. waiting on a batch of related PRs, or a NuGet release someone's specifically waiting on. Optional: most tickets skip straight to `Done` on merge, matching this repo's actual release cadence (PRs merge continuously; `CHANGELOG.md`'s `[Unreleased]` section can hold several before a version is cut). |
| `Done` | Merged and the issue is closed. This is the default landing spot from `In QA` — don't hold a ticket at `Ready for Release` without a specific reason to. |

This repo follows [SoloCAIRN](https://github.com/joshuaedwardcrowe/SoloCAIRN)
for a ticket's Build-stage lifecycle, with one extension specific to
this repo, not something SoloCAIRN itself prescribes: **the GitHub
Issue itself is the story artifact** — no separate markdown file or
dedicated location. It's already written down, reviewable via
comments, and tracked through GitHub's own history.

## Milestones

When a milestone is tied to catching up to (or tracking) an external
spec or API version, name it after that version (e.g. `YNAB API
v1.86.0`), not a goal-style description (e.g. `Full YNAB API
Coverage`) — a version-anchored name pins the milestone to a concrete,
checkable target and supports a version history over time. For a
feature-area milestone with no external version to anchor to, a plain
descriptive name is fine.

## Versioning & releases

`YnabSharp` (the library) is the only published package — standard
semver, bumped in `YnabSharp/YnabSharp.csproj`. `YnabSharp.Seeder` is not
published; its version doesn't need to track the library's.

Every squash-merged PR that changes behavior gets a line in
[`CHANGELOG.md`](CHANGELOG.md) under `[Unreleased]`, in [Keep a
Changelog](https://keepachangelog.com/) format.

**To cut a release:** open a PR that bumps `<Version>` in
`YnabSharp/YnabSharp.csproj` — nothing else, so it's easy to review —
and merge it. Then run the [`Publish`](.github/workflows/publish.yml)
workflow manually from the Actions tab (`workflow_dispatch` — it never
triggers on its own). It builds, tests, packs, and pushes that exact
version to NuGet, then moves `CHANGELOG.md`'s `[Unreleased]` section
under a new `[version] - date` heading, tags the commit `vX.Y.Z`, and
creates a GitHub Release from it. Requires a `NUGET_API_KEY` repository
secret to be configured first.

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
