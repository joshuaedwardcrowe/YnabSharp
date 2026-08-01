using YnabSharp.Responses.Accounts;
using YnabSharp.Responses.Categories;
using YnabSharp.Responses.Category;
using YnabSharp.Responses.Payees;
using YnabSharp.Responses.Plans;
using YnabSharp.Responses.ScheduledTransactions;
using YnabSharp.Responses.Transactions;

namespace YnabSharp.IntegrationTests.SpecConformance;

/// <summary>
/// Checks each existing response DTO's [JsonPropertyName] attributes against
/// the vendored spec's schema for that resource: every attributed property
/// must exist in the spec schema, every spec-required field must be present
/// on the DTO, and a spec-optional field's CLR property must be nullable.
///
/// DTO-to-schema names don't match 1:1 — YnabSharp's *Response DTOs mirror the
/// spec's "detail"/"summary" resource schemas, not the *Response wrapper
/// schemas (those wrap a `data` envelope YnabSharp's HTTP layer already
/// unwraps before deserializing into these DTOs).
/// </summary>
public class ResponseDtoSpecConformanceTests
{
    private static readonly SpecSchemaResolver Resolver = SpecSchemaResolver.FromFile(
        Path.Combine(AppContext.BaseDirectory, "ynab-openapi-spec.yaml"));

    private static IEnumerable<TestCaseData> DtoSchemaMappings()
    {
        yield return new TestCaseData(typeof(AccountResponse), "Account")
            .SetName("AccountResponse_vs_Account");
        yield return new TestCaseData(typeof(PayeeResponse), "Payee")
            .SetName("PayeeResponse_vs_Payee");
        yield return new TestCaseData(typeof(CategoryGroupResponse), "CategoryGroupWithCategories")
            .SetName("CategoryGroupResponse_vs_CategoryGroupWithCategories");
        yield return new TestCaseData(typeof(CategoryResponse), "Category")
            .SetName("CategoryResponse_vs_Category");
        yield return new TestCaseData(typeof(PlanResponse), "PlanSummary")
            .SetName("PlanResponse_vs_PlanSummary");
        yield return new TestCaseData(typeof(ScheduledTransactionsResponse), "ScheduledTransactionSummary")
            .SetName("ScheduledTransactionsResponse_vs_ScheduledTransactionSummary");
        yield return new TestCaseData(typeof(TransactionResponse), "TransactionDetail")
            .SetName("TransactionResponse_vs_TransactionDetail");
        yield return new TestCaseData(typeof(SplitTransactionResponse), "TransactionSummaryBase")
            .SetName("SplitTransactionResponse_vs_TransactionSummaryBase");
    }

    [TestCaseSource(nameof(DtoSchemaMappings))]
    public void GivenResponseDto_WhenComparedAgainstSpecSchema_ThenAttributesMatchSpec(Type dtoType, string schemaName)
    {
        var schema = Resolver.GetEffectiveSchema(schemaName);
        var dtoProperties = JsonPropertyReflector.GetJsonProperties(dtoType);

        Assert.Multiple(() =>
        {
            foreach (var dtoProperty in dtoProperties)
            {
                Assert.That(
                    schema.Properties,
                    Does.Contain(dtoProperty.JsonPropertyName),
                    $"{dtoType.Name}.{dtoProperty.ClrPropertyName} declares JSON property " +
                    $"'{dtoProperty.JsonPropertyName}', which is not part of the '{schemaName}' spec schema.");

                if (!schema.Required.Contains(dtoProperty.JsonPropertyName))
                {
                    Assert.That(
                        dtoProperty.IsNullable,
                        Is.True,
                        $"{dtoType.Name}.{dtoProperty.ClrPropertyName} maps to spec field " +
                        $"'{dtoProperty.JsonPropertyName}', which is optional in '{schemaName}' " +
                        "but the CLR property type is non-nullable.");
                }
            }

            var dtoJsonPropertyNames = dtoProperties.Select(p => p.JsonPropertyName).ToHashSet();
            var missingRequiredFields = schema.Required.Except(dtoJsonPropertyNames).ToList();

            Assert.That(
                missingRequiredFields,
                Is.Empty,
                $"{dtoType.Name} is missing spec-required field(s) from '{schemaName}': " +
                string.Join(", ", missingRequiredFields));
        });
    }
}
