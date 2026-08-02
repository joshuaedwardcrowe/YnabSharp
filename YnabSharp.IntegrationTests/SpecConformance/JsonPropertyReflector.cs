using System.Reflection;
using System.Text.Json.Serialization;

namespace YnabSharp.IntegrationTests.SpecConformance;

/// <summary>
/// Reads a response DTO's [JsonPropertyName]-attributed properties, including
/// whether each is nullable (Nullable&lt;T&gt; or a nullable reference type).
/// Properties without [JsonPropertyName] (e.g. computed convenience properties
/// like CategoryResponse.Assigned) are not part of the wire format and skipped.
/// </summary>
public static class JsonPropertyReflector
{
    public static IReadOnlyList<JsonPropertyInfo> GetJsonProperties(Type dtoType)
    {
        var nullabilityContext = new NullabilityInfoContext();

        return dtoType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(property => (property, attribute: property.GetCustomAttribute<JsonPropertyNameAttribute>()))
            .Where(x => x.attribute is not null)
            .Select(x => new JsonPropertyInfo(
                x.property.Name,
                x.attribute!.Name,
                IsNullable(x.property, nullabilityContext)))
            .ToList();
    }

    private static bool IsNullable(PropertyInfo property, NullabilityInfoContext nullabilityContext)
    {
        if (Nullable.GetUnderlyingType(property.PropertyType) is not null)
        {
            return true;
        }

        if (property.PropertyType.IsValueType)
        {
            return false;
        }

        var nullabilityInfo = nullabilityContext.Create(property);
        return nullabilityInfo.WriteState == NullabilityState.Nullable
            || nullabilityInfo.ReadState == NullabilityState.Nullable;
    }
}
