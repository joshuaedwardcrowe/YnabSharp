
using KitCli;
using YnabSharp.Seeder;

var app = new CliAppBuilder()
    .WithCli<YnabSharpSeederCliApp>()
    .WithUserSecretSettings()
    .WithSettings<YnabSharpSeederSettings>()
    .WithRegistry<YnabRegistry>()
    .WithRegistry<YnabSharpSeederRegistry>();

await app.Run();