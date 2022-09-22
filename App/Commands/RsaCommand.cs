using Lib.Configuration;
using Lib.Helpers;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands
{
    [Command(Name = "Rsa", FullName = "Generate RSA certificate", Description = "Generate RSA certificate.")]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    [HelpOption]
    public class RsaCommand : PfxCommand
    {
        public RsaCommand(ICertificateHelper certificateHelper, IConsoleHelper consoleHelper) : base(certificateHelper, consoleHelper)
        {
            KeySize = 2048;
            CertificateFile = $"RSA-{DateTime.Now:yyyyMMddTHHmmss}.pfx";
        }

        protected override void Execute(CommandLineApplication app)
        {
            var parameters = new PfxParameters
            {
                DnsName = DnsName,
                KeySize = KeySize,
                ValidityInYears = ValidityInYears,
                CertificateFile = CertificateFile,
                CertificatePassword = CertificatePassword,
                OutputDirectory = OutputDirectory,
                PfxType = PfxType.Rsa
            };

            ConsoleHelper.RenderStatus(() =>
            {
                CertificateHelper.GenerateRsa(parameters);
                ConsoleHelper.RenderPfx(parameters);
            });
        }

        protected override bool IsValidKeySize()
        {
            return KeySize is 1024 or 2048 or 3072 or 7680 or 15360;
        }

        protected static string GetVersion() => GetVersion(typeof(RsaCommand));
    }
}
