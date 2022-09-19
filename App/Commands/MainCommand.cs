using Lib.Helpers;
using McMaster.Extensions.CommandLineUtils;
using static App.Extensions.PathExtensions;

namespace App.Commands;

[Command(Name = "CertificateCli", FullName = "Generate RSA/JWK", Description = "Generate RSA/JWK.")]
[Subcommand(typeof(JwkCommand), typeof(RsaCommand), typeof(EccCommand))]
[VersionOptionFromMember(MemberName = nameof(GetVersion))]
public class MainCommand : AbstractCommand
{
    public MainCommand(IConsoleHelper consoleHelper) : base(consoleHelper)
    {
    }

    [Option("-s|--settings", "Show settings information.", CommandOptionType.NoValue)]
    public bool ShowSettings { get; set; }

    protected override void Execute(CommandLineApplication app)
    {
        if (ShowSettings)
        {
            var filepath = GetSettingFilePath();
            ConsoleHelper.RenderSettingsFile(filepath);
        }
        else
        {
            const string title = "CertificateCli";
            ConsoleHelper.RenderTitle(title);
            app.ShowHelp();
        }
    }

    protected static string GetVersion() => GetVersion(typeof(MainCommand));
}