# YnabSharp

A .NET client library for the YNAB API: Http/Clients/Requests/Responses
layer, domain models, milliunit currency conversion, split transactions.
`YnabSharp` is the only published package; `YnabSharp.Seeder` writes real
data to a real YNAB budget via the live API — there is no sandbox mode.

<!-- The full process for reviews, ADRs, concept docs, and issue triage
lives in the "repo-operating-model" skill — invoke it rather than
duplicating that procedure here. This file holds the standing facts a
session needs every time, not the multi-step playbook. -->

## Build & test

```
dotnet restore YnabSharp.sln
dotnet build YnabSharp.sln
dotnet test YnabSharp.sln
```

CI runs the same three steps on every PR and push to `main`.

## Conventions

- **Commits/PR titles**: Conventional Commits — `<type>(scope): <description>`.
  `type` ∈ `feat|fix|docs|chore|refactor|test|ci`. `scope` (optional) ∈
  `client|domain|seeder|tooling`. Breaking change: `!` right before the
  colon. Description is lowercase, imperative, no trailing period.
  Squash-merge titles become the `CHANGELOG.md` line.
- **ADR vs. concept doc**: an ADR (`docs/adr/`) records a decision and
  its alternatives; a concept doc (`docs/concepts/`) explains how a
  subsystem works today. Full criteria for each are in
  [`CONTRIBUTING.md`](CONTRIBUTING.md) — read that before writing either.
- **Issue labels** (three independent axes, always all three): type
  (`bug|feature|tech-debt|docs|process`) × area
  (`area:client|area:domain|area:seeder|area:tooling`) × severity
  (`sev:high|sev:medium|sev:low`).
- **PRs mirror their linked issue's labels and milestone.** GitHub
  doesn't copy these automatically — set them explicitly when opening
  the PR, not just on the issue.
- **Move the issue's `Status` field on project #11 at each gate** (see
  [`CONTRIBUTING.md`](CONTRIBUTING.md)'s "Status gates" section) as work
  actually happens — starting implementation, opening the PR, resolving
  review comments, merging. It's easy to do the work and forget the
  board; a ticket sitting at `Backlog` while merged is a stale signal,
  not a harmless oversight.
- **PR template triggers** (`.github/PULL_REQUEST_TEMPLATE.md`):
  breaking change or new cross-cutting pattern → ADR required in the
  same PR. Behavior change → `CHANGELOG.md` entry. Behavior a concept
  doc describes → update that doc in the same PR. Anything touching
  currency conversion or the Seeder's write path → extra scrutiny, this
  moves real money.

## Standing rules for Claude

- **Always ask before merging** any PR or branch in this repo, every
  time, no exceptions — even if a merge was approved earlier in the
  session. This applies regardless of how confident the change is.
- **Replying to PR review comments, or commenting on issues**: `gh` runs
  as the human's own GitHub account, so an unmarked comment reads as
  them talking to themselves. Prefix every reply/comment with
  `🤖 **Claude:**` so it's clearly not the human's own voice. For PR
  review comments specifically, reply to each comment on its own thread
  (`gh api repos/{owner}/{repo}/pulls/{pr}/comments/{comment_id}/replies`),
  not with one combined comment.
- **When auditing all comments on a PR**, use `gh api --paginate` —
  the default page size (30) silently truncates results on PRs with a
  lot of back-and-forth, which can make an already-answered thread look
  unaddressed (or vice versa).
- **When a review comment raises a good idea that's out of scope for
  the current PR**, don't just acknowledge it — offer to file it as its
  own issue (using the label taxonomy above), and link back to the
  issue on that comment's thread once created.
- **Money and the Seeder are not the place to move fast.** Any change to
  milliunit conversion or the Seeder's write path gets read more
  carefully than everything else in this repo, and should never be
  merged on assumption — verify against `MilliunitConverter` and the
  actual request/response DTOs, not memory of how it "should" work.
