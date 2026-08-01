namespace YnabSharp.IntegrationTests.SpecConformance;

public class SpecSchemaResolverTests
{
    [Test]
    public void GivenPlainObjectSchema_WhenGetEffectiveSchema_ThenReturnsItsOwnPropertiesAndRequired()
    {
        var resolver = SpecSchemaResolver.FromYaml(new StringReader(
            """
            components:
              schemas:
                Widget:
                  required:
                    - id
                  type: object
                  properties:
                    id:
                      type: string
                    note:
                      type: string
            """));

        var schema = resolver.GetEffectiveSchema("Widget");

        Assert.That(schema.Properties, Is.EquivalentTo(new[] { "id", "note" }));
        Assert.That(schema.Required, Is.EquivalentTo(new[] { "id" }));
    }

    [Test]
    public void GivenAllOfWithRefAndInlineMember_WhenGetEffectiveSchema_ThenMergesBothMembers()
    {
        var resolver = SpecSchemaResolver.FromYaml(new StringReader(
            """
            components:
              schemas:
                WidgetBase:
                  required:
                    - id
                  type: object
                  properties:
                    id:
                      type: string
                Widget:
                  allOf:
                    - $ref: "#/components/schemas/WidgetBase"
                    - type: object
                      required:
                        - label
                      properties:
                        label:
                          type: string
            """));

        var schema = resolver.GetEffectiveSchema("Widget");

        Assert.That(schema.Properties, Is.EquivalentTo(new[] { "id", "label" }));
        Assert.That(schema.Required, Is.EquivalentTo(new[] { "id", "label" }));
    }

    [Test]
    public void GivenUnknownSchemaName_WhenGetEffectiveSchema_ThenThrows()
    {
        var resolver = SpecSchemaResolver.FromYaml(new StringReader(
            """
            components:
              schemas:
                Widget:
                  type: object
                  properties:
                    id:
                      type: string
            """));

        Assert.That(
            () => resolver.GetEffectiveSchema("DoesNotExist"),
            Throws.InvalidOperationException);
    }

    [Test]
    public void GivenAccount_WhenGetEffectiveSchema_ThenResolvesInheritedAccountBaseFields()
    {
        var resolver = SpecSchemaResolver.FromFile(
            Path.Combine(AppContext.BaseDirectory, "ynab-openapi-spec.yaml"));

        var schema = resolver.GetEffectiveSchema("Account");

        // Inherited from AccountBase via allOf + $ref, not declared directly on Account.
        Assert.That(schema.Properties, Does.Contain("balance"));
        Assert.That(schema.Properties, Does.Contain("transfer_payee_id"));
        Assert.That(schema.Required, Is.EquivalentTo(new[]
        {
            "balance", "cleared_balance", "closed", "deleted", "id",
            "name", "on_budget", "transfer_payee_id", "type", "uncleared_balance",
        }));

        // Declared directly on Account, not on AccountBase.
        Assert.That(schema.Properties, Does.Contain("balance_formatted"));
    }
}
