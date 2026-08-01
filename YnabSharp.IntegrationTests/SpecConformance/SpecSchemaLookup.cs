using YamlDotNet.RepresentationModel;

namespace YnabSharp.IntegrationTests.SpecConformance;

/// <summary>
/// Loads the vendored YNAB OpenAPI spec and finds a named schema node under
/// components.schemas. Owns loading and lookup only — it has no opinion on
/// how a schema's `$ref`/`allOf` composition gets walked.
/// </summary>
public sealed class SpecSchemaLookup
{
    private readonly YamlMappingNode _schemas;

    public static SpecSchemaLookup FromYaml(TextReader specYaml)
    {
        var yamlStream = new YamlStream();
        yamlStream.Load(specYaml);
        return new SpecSchemaLookup(ResolveSchemasSection(yamlStream));
    }

    public static SpecSchemaLookup FromFile(string specYamlPath)
    {
        using var reader = new StreamReader(specYamlPath);
        return FromYaml(reader);
    }

    public YamlMappingNode GetSchemaNode(string schemaName)
    {
        if (!_schemas.Children.TryGetValue(new YamlScalarNode(schemaName), out var schemaNode))
        {
            throw new InvalidOperationException(
                $"Schema '{schemaName}' was not found under components.schemas in the vendored spec.");
        }

        return (YamlMappingNode)schemaNode;
    }

    private SpecSchemaLookup(YamlMappingNode schemas)
    {
        _schemas = schemas;
    }

    private static YamlMappingNode ResolveSchemasSection(YamlStream yamlStream)
    {
        var rootMapping = (YamlMappingNode)yamlStream.Documents.Single().RootNode;
        var componentsSection = (YamlMappingNode)rootMapping.Children[new YamlScalarNode("components")];
        return (YamlMappingNode)componentsSection.Children[new YamlScalarNode("schemas")];
    }
}
