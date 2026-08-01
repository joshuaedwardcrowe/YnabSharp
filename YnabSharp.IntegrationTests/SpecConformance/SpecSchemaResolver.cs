namespace YnabSharp.IntegrationTests.SpecConformance;

/// <summary>
/// Resolves a named schema from the vendored YNAB OpenAPI spec to its
/// effective properties and required fields. Wires a <see cref="SpecSchemaLookup"/>
/// (loading + name lookup) to a <see cref="SchemaCompositionMerger"/>
/// (`$ref`/`allOf` composition walk) behind a single entry point.
/// </summary>
public sealed class SpecSchemaResolver
{
    private readonly SchemaCompositionMerger _merger;

    public static SpecSchemaResolver FromYaml(TextReader specYaml) =>
        new(SpecSchemaLookup.FromYaml(specYaml));

    public static SpecSchemaResolver FromFile(string specYamlPath) =>
        new(SpecSchemaLookup.FromFile(specYamlPath));

    public SpecSchema GetEffectiveSchema(string schemaName) =>
        _merger.Merge(schemaName);

    private SpecSchemaResolver(SpecSchemaLookup lookup)
    {
        _merger = new SchemaCompositionMerger(lookup);
    }
}
