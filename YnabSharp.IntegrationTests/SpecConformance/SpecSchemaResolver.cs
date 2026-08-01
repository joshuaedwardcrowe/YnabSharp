using YamlDotNet.RepresentationModel;

namespace YnabSharp.IntegrationTests.SpecConformance;

/// <summary>
/// Resolves a named schema from the vendored YNAB OpenAPI spec to its effective
/// properties and required fields, walking `$ref` and `allOf` composition. The
/// spec composes schemas this way (e.g. `Account` = `allOf: [AccountBase, {...}]`),
/// so a naive lookup on the named schema alone misses inherited fields.
/// </summary>
public sealed class SpecSchemaResolver
{
    private readonly YamlMappingNode _schemas;

    public static SpecSchemaResolver FromYaml(TextReader specYaml)
    {
        var yamlStream = new YamlStream();
        yamlStream.Load(specYaml);
        return new SpecSchemaResolver(ResolveSchemasSection(yamlStream));
    }

    public static SpecSchemaResolver FromFile(string specYamlPath)
    {
        using var reader = new StreamReader(specYamlPath);
        return FromYaml(reader);
    }

    public SpecSchema GetEffectiveSchema(string schemaName)
    {
        var properties = new HashSet<string>();
        var required = new HashSet<string>();
        MergeSchemaByName(schemaName, properties, required, [schemaName]);
        return new SpecSchema(properties, required);
    }

    private SpecSchemaResolver(YamlMappingNode schemas)
    {
        _schemas = schemas;
    }

    private static YamlMappingNode ResolveSchemasSection(YamlStream yamlStream)
    {
        var rootMapping = (YamlMappingNode)yamlStream.Documents.Single().RootNode;
        var componentsSection = (YamlMappingNode)rootMapping.Children[new YamlScalarNode("components")];
        return (YamlMappingNode)componentsSection.Children[new YamlScalarNode("schemas")];
    }

    private void MergeSchemaByName(
        string schemaName,
        HashSet<string> properties,
        HashSet<string> required,
        HashSet<string> visitedSchemaNames)
    {
        if (!_schemas.Children.TryGetValue(new YamlScalarNode(schemaName), out var schemaNode))
        {
            throw new InvalidOperationException(
                $"Schema '{schemaName}' was not found under components.schemas in the vendored spec.");
        }

        MergeSchemaMember((YamlMappingNode)schemaNode, properties, required, visitedSchemaNames);
    }

    private void MergeSchemaMember(
        YamlMappingNode memberNode,
        HashSet<string> properties,
        HashSet<string> required,
        HashSet<string> visitedSchemaNames)
    {
        if (TryMergeRef(memberNode, properties, required, visitedSchemaNames))
        {
            return;
        }

        if (TryMergeAllOf(memberNode, properties, required, visitedSchemaNames))
        {
            return;
        }

        MergeProperties(memberNode, properties);
        MergeRequired(memberNode, required);
    }

    private bool TryMergeRef(
        YamlMappingNode memberNode,
        HashSet<string> properties,
        HashSet<string> required,
        HashSet<string> visitedSchemaNames)
    {
        if (!memberNode.Children.TryGetValue(new YamlScalarNode("$ref"), out var refNode))
        {
            return false;
        }

        var referencedSchemaName = ((YamlScalarNode)refNode).Value!.Split('/')[^1];
        if (visitedSchemaNames.Add(referencedSchemaName))
        {
            MergeSchemaByName(referencedSchemaName, properties, required, visitedSchemaNames);
        }

        return true;
    }

    private bool TryMergeAllOf(
        YamlMappingNode memberNode,
        HashSet<string> properties,
        HashSet<string> required,
        HashSet<string> visitedSchemaNames)
    {
        if (!memberNode.Children.TryGetValue(new YamlScalarNode("allOf"), out var allOfNode))
        {
            return false;
        }

        foreach (var member in (YamlSequenceNode)allOfNode)
        {
            MergeSchemaMember((YamlMappingNode)member, properties, required, visitedSchemaNames);
        }

        return true;
    }

    private static void MergeProperties(YamlMappingNode memberNode, HashSet<string> properties)
    {
        if (!memberNode.Children.TryGetValue(new YamlScalarNode("properties"), out var propertiesNode))
        {
            return;
        }

        foreach (var key in ((YamlMappingNode)propertiesNode).Children.Keys)
        {
            properties.Add(((YamlScalarNode)key).Value!);
        }
    }

    private static void MergeRequired(YamlMappingNode memberNode, HashSet<string> required)
    {
        if (!memberNode.Children.TryGetValue(new YamlScalarNode("required"), out var requiredNode))
        {
            return;
        }

        foreach (var item in (YamlSequenceNode)requiredNode)
        {
            required.Add(((YamlScalarNode)item).Value!);
        }
    }
}
