using System.Text.Json;
using Lib.Configuration;
using Spectre.Console;
using TextCopy;

namespace Lib.Helpers;

public class ConsoleHelper : IConsoleHelper
{
    public void RenderTitle(string text)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new FigletText(text).LeftAligned());
        AnsiConsole.WriteLine();
    }

    public void CopyTextToClipboard(string text)
    {
        ClipboardService.SetText(text);
    }

    public void RenderSettingsFile(string filepath)
    {
        var name = Path.GetFileName(filepath);
        var json = File.ReadAllText(filepath);
        var formattedJson = PrettifyJson(json);
        var header = new Rule($"[yellow]({name})[/]");
        header.Centered();
        var footer = new Rule($"[yellow]({filepath})[/]");
        footer.Centered();

        AnsiConsole.WriteLine();
        AnsiConsole.Write(header);
        AnsiConsole.WriteLine(formattedJson);
        AnsiConsole.Write(footer);
        AnsiConsole.WriteLine();
    }

    public void RenderRsa(RsaParameters parameters)
    {
        var dns = parameters.DnsName;
        var keySize = parameters.KeySize;
        var years = parameters.ValidityInYears;
        var file = parameters.CertificateFile;
        var password = parameters.CertificatePassword;

        var table = new Table()
            .BorderColor(Color.White)
            .Border(TableBorder.Square)
            .Title("[yellow]1 RSA certificate(s) generated[/]")
            .AddColumn(new TableColumn("[u]File[/]").Centered())
            .AddColumn(new TableColumn("[u]Password[/]").Centered())
            .AddColumn(new TableColumn("[u]DnsName[/]").Centered())
            .AddColumn(new TableColumn("[u]KeySize[/]").Centered())
            .AddColumn(new TableColumn("[u]ValidityInYears[/]").Centered())
            .AddRow(file, password, dns, $"{keySize}", $"{years}");

        AnsiConsole.WriteLine();
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    public void RenderJwk(JwkParameters parameters, string jwk)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(
            new Panel(new Text(jwk).LeftAligned())
                .Expand()
                .Padding(1, 1)
                .SquareBorder()
                .Header($"[b][yellow] {parameters.CertificateFile.ToUpper()} [/][/]"));
        AnsiConsole.WriteLine();
    }

    public void RenderException(Exception exception)
    {
        const ExceptionFormats formats = ExceptionFormats.ShortenTypes
                                         | ExceptionFormats.ShortenPaths
                                         | ExceptionFormats.ShortenMethods;

        AnsiConsole.WriteLine();
        AnsiConsole.WriteException(exception, formats);
        AnsiConsole.WriteLine();
    }

    private static string PrettifyJson(string json)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        using var jDoc = JsonDocument.Parse(json);
        return JsonSerializer.Serialize(jDoc, options);
    }
}