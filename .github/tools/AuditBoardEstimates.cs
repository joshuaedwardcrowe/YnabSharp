// Audits delivery boards for estimates on work that was never actually built.
//
// The invariant: a board item at Status=Done carrying an Estimate has at least
// one closing pull request. Points on a Done item are what velocity is
// calculated from, so an item closed without delivering code — a spike
// superseded by its own sub-issues, an abandoned ticket, a duplicate —
// silently inflates it.
//
// This flags candidates for a human to review. It never edits a board: a
// ticket genuinely delivered but closed without a linked PR is a false
// positive, and silently deleting estimates on a heuristic would be worse than
// the problem it solves.
//
// It is also deliberately fail-closed. Anything that stops it reading a board
// in full — a missing token, an HTTP error, a GraphQL error, an empty board —
// exits 2 rather than reporting zero violations. A gate that can be skipped
// without anyone noticing is not a gate.
//
// Exit codes:  0 clean   1 violations found   2 could not audit
//
// Run:  GH_TOKEN=<pat> dotnet run .github/tools/AuditBoardEstimates.cs
//
// On violations it writes a markdown table to VIOLATIONS_PATH. Set that
// explicitly: `dotnet run` on a file-based app sets the working directory to
// the .cs file's own directory, not the directory you invoked it from, so a
// relative default would land somewhere the caller isn't looking.

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

const string Owner = "joshuaedwardcrowe";

// The delivery boards tracking *this repo's* work. Other repos audit their own
// boards with their own copy of this tool — a repo shouldn't be filing issues
// about another repo's board hygiene on a schedule it owns.
//
// The Ideas boards (#10, and the KitCli/Diagnosea org ones) are deliberately
// absent rather than overlooked: they size in WAG/SWAG months rather than
// Fibonacci points, so this invariant doesn't apply to them at all.
int[] projects = [11, 12];

// Work older than this predates the PR-linking convention and would bury real
// signal under items that were delivered perfectly well before anyone was
// linking PRs to issues. This is a tripwire, not an archaeological dig.
const int LookbackDays = 90;

const string Query = """
query($owner: String!, $number: Int!, $cursor: String) {
  user(login: $owner) {
    projectV2(number: $number) {
      title
      items(first: 100, after: $cursor) {
        pageInfo { hasNextPage endCursor }
        nodes {
          fieldValues(first: 20) {
            nodes {
              __typename
              ... on ProjectV2ItemFieldSingleSelectValue {
                name
                field { ... on ProjectV2FieldCommon { name } }
              }
              ... on ProjectV2ItemFieldNumberValue {
                number
                field { ... on ProjectV2FieldCommon { name } }
              }
            }
          }
          content {
            __typename
            ... on Issue {
              number
              title
              closedAt
              repository { nameWithOwner }
              closedByPullRequestsReferences(first: 5) { nodes { number } }
            }
          }
        }
      }
    }
  }
}
""";

var token = Environment.GetEnvironmentVariable("GH_TOKEN");
if (string.IsNullOrWhiteSpace(token))
{
    Console.Error.WriteLine(
        "GH_TOKEN is not set. Projects v2 is unreachable with the default " +
        "GITHUB_TOKEN — this needs a PAT carrying `project` (read) and `repo` scope.");
    return 2;
}

using var http = new HttpClient();
http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
http.DefaultRequestHeaders.UserAgent.ParseAdd("ynabsharp-board-audit");

var cutoff = DateTimeOffset.UtcNow.AddDays(-LookbackDays);
var violations = new List<Violation>();

foreach (var project in projects)
{
    Console.WriteLine($"auditing project #{project} ...");

    int itemsSeen = 0, found = 0;
    string? cursor = null;

    do
    {
        JsonElement page;
        try
        {
            page = await FetchPage(http, project, cursor);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"  ! could not read project #{project}: {ex.Message}");
            return 2;
        }

        var items = page.GetProperty("items");
        foreach (var item in items.GetProperty("nodes").EnumerateArray())
        {
            itemsSeen++;
            if (TryReadViolation(item, cutoff) is { } violation)
            {
                violation = violation with { Project = project };
                violations.Add(violation);
                found++;
            }
        }

        var pageInfo = items.GetProperty("pageInfo");
        cursor = pageInfo.GetProperty("hasNextPage").GetBoolean()
            ? pageInfo.GetProperty("endCursor").GetString()
            : null;
    }
    while (cursor is not null);

    // A configured delivery board is never genuinely empty. Reading zero items
    // means something went wrong that didn't surface as an error — treat it as
    // an audit failure rather than a pass.
    if (itemsSeen == 0)
    {
        Console.Error.WriteLine(
            $"  ! project #{project} returned no items at all — refusing to " +
            "report it clean. Check the board number and the token's scopes.");
        return 2;
    }

    Console.WriteLine($"  {itemsSeen} item(s) read, {found} violation(s)");
}

if (violations.Count == 0)
{
    Console.WriteLine("\nNo violations. Every Done item with an estimate has a closing PR.");
    return 0;
}

var total = violations.Sum(v => v.Estimate);
Console.WriteLine($"\n{violations.Count} item(s) carrying {total} unearned point(s):\n");

var table = new StringBuilder()
    .AppendLine("| Project | Issue | Estimate | Title |")
    .AppendLine("|---|---|---|---|");

foreach (var v in violations.OrderBy(v => v.Project).ThenBy(v => v.Issue))
{
    Console.WriteLine($"  project #{v.Project}  {v.Repository}#{v.Issue}  est={v.Estimate}  {v.Title}");
    table.AppendLine(
        $"| #{v.Project} | [{v.Repository}#{v.Issue}]" +
        $"(https://github.com/{v.Repository}/issues/{v.Issue}) | {v.Estimate} | {v.Title} |");
}

var outputPath = Environment.GetEnvironmentVariable("VIOLATIONS_PATH");
if (!string.IsNullOrWhiteSpace(outputPath))
{
    await File.WriteAllTextAsync(outputPath, table.ToString());
    Console.WriteLine($"\nwrote {outputPath}");
}

return 1;

static async Task<JsonElement> FetchPage(HttpClient http, int project, string? cursor)
{
    // Built by hand rather than by serializing an anonymous type: file-based
    // apps disable reflection-based serialization.
    var payload = new JsonObject
    {
        ["query"] = Query,
        ["variables"] = new JsonObject
        {
            ["owner"] = Owner,
            ["number"] = project,
            ["cursor"] = cursor is null ? null : JsonValue.Create(cursor)
        }
    }.ToJsonString();

    using var response = await http.PostAsync(
        "https://api.github.com/graphql",
        new StringContent(payload, Encoding.UTF8, "application/json"));

    var body = await response.Content.ReadAsStringAsync();
    if (!response.IsSuccessStatusCode)
    {
        throw new InvalidOperationException($"HTTP {(int)response.StatusCode}: {Truncate(body)}");
    }

    using var document = JsonDocument.Parse(body);
    var root = document.RootElement;

    if (root.TryGetProperty("errors", out var errors))
    {
        throw new InvalidOperationException($"GraphQL error: {Truncate(errors.ToString())}");
    }

    var projectNode = root.GetProperty("data").GetProperty("user").GetProperty("projectV2");
    if (projectNode.ValueKind == JsonValueKind.Null)
    {
        throw new InvalidOperationException("project not found, or the token cannot see it");
    }

    return projectNode.Clone();
}

static Violation? TryReadViolation(JsonElement item, DateTimeOffset cutoff)
{
    // Pull requests get put on boards directly and have no closing PR of their
    // own, so they would trip this check by construction. Draft items have no
    // issue to check at all.
    if (!item.TryGetProperty("content", out var content)
        || content.ValueKind == JsonValueKind.Null
        || content.GetProperty("__typename").GetString() != "Issue")
    {
        return null;
    }

    string? status = null;
    double? estimate = null;

    foreach (var value in item.GetProperty("fieldValues").GetProperty("nodes").EnumerateArray())
    {
        if (!value.TryGetProperty("field", out var field)
            || !field.TryGetProperty("name", out var fieldName))
        {
            continue;
        }

        switch (fieldName.GetString())
        {
            case "Status" when value.TryGetProperty("name", out var name):
                status = name.GetString();
                break;
            case "Estimate" when value.TryGetProperty("number", out var number):
                estimate = number.GetDouble();
                break;
        }
    }

    if (status != "Done" || estimate is null)
    {
        return null;
    }

    if (content.GetProperty("closedByPullRequestsReferences")
               .GetProperty("nodes").GetArrayLength() > 0)
    {
        return null;
    }

    var closedAt = content.GetProperty("closedAt");
    if (closedAt.ValueKind != JsonValueKind.Null
        && closedAt.TryGetDateTimeOffset(out var closed)
        && closed < cutoff)
    {
        return null;
    }

    return new Violation(
        Project: 0,
        Repository: content.GetProperty("repository").GetProperty("nameWithOwner").GetString()!,
        Issue: content.GetProperty("number").GetInt32(),
        Estimate: estimate.Value,
        Title: content.GetProperty("title").GetString() ?? string.Empty);
}

static string Truncate(string value)
    => value.Length <= 400 ? value : value[..400] + "...";

record Violation(int Project, string Repository, int Issue, double Estimate, string Title);
