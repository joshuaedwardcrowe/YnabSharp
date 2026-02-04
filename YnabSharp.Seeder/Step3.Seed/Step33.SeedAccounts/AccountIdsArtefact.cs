using KitCli.Commands.Abstractions.Artefacts;

namespace YnabSharp.Seeder.Step3.Seed.Step33.SeedAccounts;

public class AccountIdsArtefact(List<Guid> accountIds)
    : ValuedCliCommandArtefact<List<Guid>>(nameof(Account.Id), accountIds);