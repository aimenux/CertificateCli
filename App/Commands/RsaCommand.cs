using System.ComponentModel.DataAnnotations;
using Lib.Configuration;
using Lib.Helpers;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands
{
    [Command(Name = "Rsa", FullName = "Generate RSA certificate", Description = "Generate RSA certificate.")]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    [HelpOption]
    public class RsaCommand : AbstractCommand
    {
        private readonly IRsaHelper _rsaHelper;

        public RsaCommand(IRsaHelper rsaHelper, IConsoleHelper consoleHelper) : base(consoleHelper)
        {
            _rsaHelper = rsaHelper;
        }

        [Option("-f|--file", "CertificateFile", CommandOptionType.SingleValue)]
        public string CertificateFile { get; set; } = $"Certificate-{DateTime.Now:yyyyMMddTHHmmss}.pfx";

        [Required]
        [Option("-p|--password", "CertificatePassword", CommandOptionType.SingleValue)]
        public string CertificatePassword { get; set; }

        [Option("-d|--dns", "DnsName", CommandOptionType.SingleValue)]
        public string DnsName { get; set; } = "localhost";

        [Option("-s|--size", "KeySize", CommandOptionType.SingleValue)]
        public int KeySize { get; set; } = 2048;

        [Option("-v|--validity", "ValidityInYears", CommandOptionType.SingleValue)]
        public int ValidityInYears { get; set; } = 1;

        protected override void Execute(CommandLineApplication app)
        {
            var parameters = new RsaParameters
            {
                DnsName = DnsName,
                KeySize = KeySize,
                ValidityInYears = ValidityInYears,
                CertificateFile = CertificateFile,
                CertificatePassword = CertificatePassword
            };

            _rsaHelper.GenerateRsa(parameters);
            ConsoleHelper.RenderRsa(parameters);
        }

        protected override bool HasValidOptions()
        {
            return !string.IsNullOrWhiteSpace(CertificateFile)
                   && !string.IsNullOrWhiteSpace(CertificatePassword)
                   && !string.IsNullOrWhiteSpace(DnsName)
                   && KeySize is 1024 or 2048 or 4096
                   && ValidityInYears is >= 1 and <= 10
                   && IsCertificateFilePathValid();
        }

        protected static string GetVersion() => GetVersion(typeof(RsaCommand));

        private bool IsCertificateFilePathValid()
        {
            var directoryName = GetDirectoryName(CertificateFile);
            if (!Directory.Exists(directoryName)) return false;
            var fileName = Path.GetFileName(CertificateFile);
            return !string.IsNullOrWhiteSpace(fileName) && Path.HasExtension(fileName);
        }

        private static string GetDirectoryName(string path)
        {
            var directoryName = Path.GetDirectoryName(path);

            if (string.IsNullOrEmpty(directoryName))
            {
                directoryName = "./";
            }

            return directoryName;
        }
    }
}
