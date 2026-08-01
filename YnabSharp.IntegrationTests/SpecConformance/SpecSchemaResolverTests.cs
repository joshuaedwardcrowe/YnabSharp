namespace YnabSharp.IntegrationTests.SpecConformance;

public class SpecSchemaResolverTests
{
    [Test]
    public void GivenPlainObjectSchema_WhenGetEffectiveSchema_ThenReturnsItsOwnPropertiesAndRequired()
    {
        var fixturePath = GetFixtureFilePath("plain-object-schema.yaml");
        var resolver = SpecSchemaResolver.FromFile(fixturePath);

        var schema = resolver.GetEffectiveSchema("Widget");

        Assert.That(schema.Properties, Is.EquivalentTo(new[] { "id", "note" }));
        Assert.That(schema.Required, Is.EquivalentTo(new[] { "id" }));
    }

    [Test]
    public void GivenAllOfWithRefAndInlineMember_WhenGetEffectiveSchema_ThenMergesBothMembers()
    {
        var fixturePath = GetFixtureFilePath("all-of-with-ref-and-inline-member.yaml");
        var resolver = SpecSchemaResolver.FromFile(fixturePath);

        var schema = resolver.GetEffectiveSchema("Widget");

        Assert.That(schema.Properties, Is.EquivalentTo(new[] { "id", "label" }));
        Assert.That(schema.Required, Is.EquivalentTo(new[] { "id", "label" }));
    }

    [Test]
    public void GivenUnknownSchemaName_WhenGetEffectiveSchema_ThenThrows()
    {
        var fixturePath = GetFixtureFilePath("unknown-schema.yaml");
        var resolver = SpecSchemaResolver.FromFile(fixturePath);

        Assert.That(
            () => resolver.GetEffectiveSchema("DoesNotExist"),
            Throws.InvalidOperationException);
    }

    [Test]
    public void GivenAccount_WhenGetEffectiveSchema_ThenResolvesInheritedAccountBaseFields()
    {
        var vendoredSpecPath = Path.Combine(AppContext.BaseDirectory, "ynab-openapi-spec.yaml");
        var resolver = SpecSchemaResolver.FromFile(vendoredSpecPath);

        var schema = resolver.GetEffectiveSchema("Account");

        Assert.That(schema.Properties, Does.Contain("balance"));
        Assert.That(schema.Properties, Does.Contain("transfer_payee_id"));
        Assert.That(schema.Required, Is.EquivalentTo(new[]
        {
            "balance",
            "cleared_balance",
            "closed",
            "deleted",
            "id",
            "name",
            "on_budget",
            "transfer_payee_id",
            "type",
            "uncleared_balance",
        }));

        Assert.That(schema.Properties, Does.Contain("balance_formatted"));
    }

    private static string GetFixtureFilePath(string fileName) =>
        Path.Combine(AppContext.BaseDirectory, "SpecConformance", "Fixtures", fileName);
}
