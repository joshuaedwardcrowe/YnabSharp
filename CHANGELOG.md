# Changelog

All notable changes to YnabSharp are documented here. Format is
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [Unreleased]

- **Breaking:** renamed the public API from `Budget`/`BudgetsClient`/
  `ConnectedBudget` to `Plan`/`PlansClient`/`ConnectedPlan` to match the
  YNAB API's canonical `/plans/` resource (#51). Also fixes a
  duplicated URL segment in the renamed `GetPlan(Guid)` (#4). Requires
  a major version bump on next publish.
