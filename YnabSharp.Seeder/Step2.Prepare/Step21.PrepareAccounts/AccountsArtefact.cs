using KitCli.Commands.Abstractions.Artefacts;

namespace YnabSharp.Seeder.Step2.Prepare.Step21.PrepareAccounts;

public class AccountsArtefact(List<Account> accounts) 
    : ValuedCliCommandArtefact<List<Account>>(nameof(Account), accounts);