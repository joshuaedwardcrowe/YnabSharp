#!/usr/bin/env python3
"""Audit delivery boards for estimates on work that was never actually built.

The invariant: a board item at Status=Done carrying an Estimate should have at
least one closing pull request. Points on a Done item are what velocity is
calculated from, so an item closed without delivering code (a spike superseded
by its own sub-issues, an abandoned ticket, a duplicate) silently inflates it.

This flags candidates for a human to review. It never edits the board — a
ticket genuinely delivered but closed without a linked PR is a false positive,
and silently deleting estimates on a heuristic is worse than the problem.

This is a tripwire, not an archaeological dig. It only looks at items closed
within LOOKBACK_DAYS, because older work predates the PR-linking convention and
would bury real signal under items that were delivered perfectly well before
anyone was linking PRs to issues.

Exits 1 if violations are found, 0 otherwise.
"""

import json
import subprocess
import sys
from datetime import datetime, timedelta, timezone

OWNER = "joshuaedwardcrowe"

# Delivery boards carrying Fibonacci Estimate fields. The Ideas board (#10) and
# the org-level Ideas boards (KitCli, Diagnosea) size in WAG/SWAG months rather
# than points, so they have no equivalent invariant.
PROJECTS = [1, 8, 9, 11, 12]

LOOKBACK_DAYS = 90


def gh_json(args):
    result = subprocess.run(
        ["gh", *args], capture_output=True, text=True, check=False
    )
    if result.returncode != 0:
        print(f"  ! gh {' '.join(args)} failed: {result.stderr.strip()}", file=sys.stderr)
        return None
    try:
        return json.loads(result.stdout)
    except json.JSONDecodeError:
        return None


def issue_facts(repo, number):
    """Returns (closed_at, closing_pr_numbers), or None if unreadable."""
    data = gh_json([
        "issue", "view", str(number),
        "--repo", repo,
        "--json", "closedAt,closedByPullRequestsReferences",
    ])
    if data is None:
        return None
    prs = [pr["number"] for pr in data.get("closedByPullRequestsReferences", [])]
    return data.get("closedAt"), prs


def audit_project(number):
    data = gh_json([
        "project", "item-list", str(number),
        "--owner", OWNER,
        "--format", "json",
        "-L", "500",
    ])
    if data is None:
        return []

    cutoff = datetime.now(timezone.utc) - timedelta(days=LOOKBACK_DAYS)
    violations = []
    for item in data.get("items", []):
        if item.get("status") != "Done" or item.get("estimate") is None:
            continue

        content = item.get("content") or {}
        # Pull requests get put on boards directly and have no "closing PR" of
        # their own, so they'd trip this check by construction. Draft items have
        # no issue to check at all.
        if content.get("type") != "Issue":
            continue

        repo, issue = content.get("repository"), content.get("number")
        if not repo or not issue:
            continue

        facts = issue_facts(repo, issue)
        if facts is None:
            continue
        closed_at, prs = facts
        if prs:
            continue

        if closed_at:
            closed = datetime.fromisoformat(closed_at.replace("Z", "+00:00"))
            if closed < cutoff:
                continue

        violations.append({
            "project": number,
            "repo": repo,
            "issue": issue,
            "estimate": item["estimate"],
            "title": content.get("title", ""),
        })

    return violations


def main():
    all_violations = []
    for number in PROJECTS:
        print(f"auditing project #{number} ...")
        found = audit_project(number)
        print(f"  {len(found)} violation(s)")
        all_violations.extend(found)

    if not all_violations:
        print("\nNo violations. Every Done item with an estimate has a closing PR.")
        return 0

    total = sum(v["estimate"] for v in all_violations)
    print(f"\n{len(all_violations)} item(s) carrying {total} unearned point(s):\n")

    lines = [
        "| Project | Issue | Estimate | Title |",
        "|---|---|---|---|",
    ]
    for v in sorted(all_violations, key=lambda x: (x["project"], x["issue"])):
        url = f"https://github.com/{v['repo']}/issues/{v['issue']}"
        lines.append(
            f"| #{v['project']} | [{v['repo']}#{v['issue']}]({url}) "
            f"| {v['estimate']} | {v['title']} |"
        )
        print(f"  project #{v['project']}  {v['repo']}#{v['issue']}  "
              f"est={v['estimate']}  {v['title']}")

    with open("violations.md", "w") as handle:
        handle.write("\n".join(lines) + "\n")

    return 1


if __name__ == "__main__":
    sys.exit(main())
