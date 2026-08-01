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
        if (TryGetRefTarget(memberNode, out var referencedSchemaName))
        {
            MergeReferencedSchema(referencedSchemaName, properties, required, visitedSchemaNames);
            return;
        }

        if (TryGetAllOfMembers(memberNode, out var allOfMembers))
        {
            MergeAllOfMembers(allOfMembers, properties, required, visitedSchemaNames);
            return;
        }

        MergeInlineProperties(memberNode, properties);
        MergeInlineRequiredFields(memberNode, required);
    }

    private static bool TryGetRefTarget(YamlMappingNode memberNode, out string referencedSchemaName)
    {
        if (!memberNode.Children.TryGetValue(new YamlScalarNode("$ref"), out var refNode))
        {
            referencedSchemaName = "";
            return false;
        }

        referencedSchemaName = ((YamlScalarNode)refNode).Value!.Split('/')[^1];
        return true;
    }

    private void MergeReferencedSchema(
        string referencedSchemaName,
        HashSet<string> properties,
        HashSet<string> required,
        HashSet<string> visitedSchemaNames)
    {
        if (visitedSchemaNames.Add(referencedSchemaName))
        {
            MergeSchemaByName(referencedSchemaName, properties, required, visitedSchemaNames);
        }
    }

    private static bool TryGetAllOfMembers(YamlMappingNode memberNode, out YamlSequenceNode allOfMembers)
    {
        if (!memberNode.Children.TryGetValue(new YamlScalarNode("allOf"), out var allOfNode))
        {
            allOfMembers = null!;
            return false;
        }

        allOfMembers = (YamlSequenceNode)allOfNode;
        return true;
    }

    private void MergeAllOfMembers(
        YamlSequenceNode allOfMembers,
        HashSet<string> properties,
        HashSet<string> required,
        HashSet<string> visitedSchemaNames)
    {
        foreach (var member in allOfMembers)
        {
            MergeSchemaMember((YamlMappingNode)member, properties, required, visitedSchemaNames);
        }
    }

    private static void MergeInlineProperties(YamlMappingNode memberNode, HashSet<string> properties)
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

    private static void MergeInlineRequiredFields(YamlMappingNode memberNode, HashSet<string> required)
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
