using System.Reflection;
using App.Commands;

namespace App.Configuration
{
    public class Settings
    {
        public static class Cli
        {
            public const string ToolName = @"CertificateCli";
            public const string Description = @"Generate RSA/ECC/JWK.";
            public static readonly string UserSecretsFile = $@"C:\Users\{Environment.UserName}\AppData\Roaming\Microsoft\UserSecrets\CertificateCli-UserSecrets\secrets.json";
            public static readonly string Version = GetInformationalVersion().Split("+").FirstOrDefault();
            
            private static string GetInformationalVersion()
            {
                return typeof(ToolCommand)
                    .Assembly
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                    .InformationalVersion;
            }
        }
    
        public static class ExitCode
        {
            public const int Ok = 0;
            public const int Ko = -1;
        }
        
        public static string GetDefaultOutputDirectory()
        {
            const string defaultDir = @"C:/Logs";

            return Directory.Exists(defaultDir)
                ? defaultDir
                : Path.GetFullPath("./");
        }

        public const string DefaultKeyUse = "sig";
        
        public const string DefaultDnsName = "localhost";
        
        public ICollection<string> KeyUsageFlags { get; set; }
    }
}
