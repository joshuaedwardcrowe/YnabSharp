namespace YnabSharp.IntegrationTests.SpecConformance;

public sealed record SpecSchema(IReadOnlySet<string> Properties, IReadOnlySet<string> Required);
