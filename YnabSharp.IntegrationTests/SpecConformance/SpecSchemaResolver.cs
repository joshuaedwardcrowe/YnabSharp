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

    private SpecSchemaResolver(YamlMappingNode schemas)
    {
        _schemas = schemas;
    }

    public static SpecSchemaResolver FromYaml(TextReader specYaml)
    {
        var yamlStream = new YamlStream();
        yamlStream.Load(specYaml);
        var root = (YamlMappingNode)yamlStream.Documents[0].RootNode;
        var components = (YamlMappingNode)root.Children[new YamlScalarNode("components")];
        var schemas = (YamlMappingNode)components.Children[new YamlScalarNode("schemas")];
        return new SpecSchemaResolver(schemas);
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
        CollectSchema(schemaName, properties, required, [schemaName]);
        return new SpecSchema(properties, required);
    }

    private void CollectSchema(
        string schemaName,
        HashSet<string> properties,
        HashSet<string> required,
        HashSet<string> visited)
    {
        if (!_schemas.Children.TryGetValue(new YamlScalarNode(schemaName), out var node))
        {
            throw new InvalidOperationException(
                $"Schema '{schemaName}' was not found under components.schemas in the vendored spec.");
        }

        CollectMember((YamlMappingNode)node, properties, required, visited);
    }

    private void CollectMember(
        YamlMappingNode memberNode,
        HashSet<string> properties,
        HashSet<string> required,
        HashSet<string> visited)
    {
        if (memberNode.Children.TryGetValue(new YamlScalarNode("$ref"), out var refNode))
        {
            var refName = ((YamlScalarNode)refNode).Value!.Split('/')[^1];
            if (!visited.Add(refName))
            {
                return;
            }

            CollectSchema(refName, properties, required, visited);
            return;
        }

        if (memberNode.Children.TryGetValue(new YamlScalarNode("allOf"), out var allOfNode))
        {
            foreach (var member in (YamlSequenceNode)allOfNode)
            {
                CollectMember((YamlMappingNode)member, properties, required, visited);
            }

            return;
        }

        if (memberNode.Children.TryGetValue(new YamlScalarNode("properties"), out var propsNode))
        {
            foreach (var key in ((YamlMappingNode)propsNode).Children.Keys)
            {
                properties.Add(((YamlScalarNode)key).Value!);
            }
        }

        if (memberNode.Children.TryGetValue(new YamlScalarNode("required"), out var requiredNode))
        {
            foreach (var item in (YamlSequenceNode)requiredNode)
            {
                required.Add(((YamlScalarNode)item).Value!);
            }
        }
    }
}
