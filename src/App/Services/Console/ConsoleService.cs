using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using App.Configuration;
using App.Extensions;
using App.Services.Certificate;
using App.Validators;
using Spectre.Console;
using TextCopy;

namespace App.Services.Console;

[ExcludeFromCodeCoverage]
public class ConsoleService : IConsoleService
{
    public void RenderTitle(string text)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new FigletText(text));
        AnsiConsole.WriteLine();
    }

    public void RenderVersion(string version)
    {
        var text = $"{Settings.Cli.ToolName} V{version}";
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Markup($"[bold {Color.White}]{text}[/]"));
        AnsiConsole.WriteLine();
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
        var formattedJson = json.JsonPrettify();
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

    public void RenderUserSecretsFile(string filepath)
    {
        if (!OperatingSystem.IsWindows()) return;
        if (!File.Exists(filepath)) return;
        if (!GetYesOrNoAnswer("display user secrets", false)) return;
        RenderSettingsFile(filepath);
    }

    public void RenderStatus(Action action)
    {
        var spinner = RandomSpinner();

        AnsiConsole.Status()
            .Start("Work is in progress ...", ctx =>
            {
                ctx.Spinner(spinner);
                action.Invoke();
            });
    }
    
    public bool GetYesOrNoAnswer(string text, bool defaultAnswer)
    {
        if (AnsiConsole.Confirm($"Do you want to [u]{text}[/] ?", defaultAnswer)) return true;
        AnsiConsole.WriteLine();
        return false;
    }

    public void RenderValidationErrors(ValidationErrors validationErrors)
    {
        var count = validationErrors.Count;

        var table = new Table()
            .BorderColor(Color.White)
            .Border(TableBorder.Square)
            .Title($"[red][bold]{count} error(s)[/][/]")
            .AddColumn(new TableColumn("[u]Name[/]").Centered())
            .AddColumn(new TableColumn("[u]Message[/]").Centered())
            .Caption("[grey][bold]Invalid options/arguments[/][/]");

        foreach (var error in validationErrors)
        {
            var failure = error.Failure;
            var name = $"[bold]{error.OptionName()}[/]";
            var reason = $"[tan]{failure.ErrorMessage}[/]";
            table.AddRow(ToMarkup(name), ToMarkup(reason));
        }

        AnsiConsole.WriteLine();
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    public void RenderPfx(PfxParameters parameters)
    {
        var dns = parameters.DnsName;
        var keySize = parameters.KeySize;
        var years = parameters.ValidityInYears;
        var file = parameters.CertificateFullPath;
        var password = parameters.CertificatePassword;
        var type = parameters.PfxType.ToString().ToUpper();

        var table = new Table()
            .BorderColor(Color.White)
            .Border(TableBorder.Square)
            .Title($"[yellow]1 {type} certificate(s) generated[/]")
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
            new Panel(new Text(jwk))
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

    private static Spinner RandomSpinner()
    {
        var values = typeof(Spinner.Known)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(x => x.PropertyType == typeof(Spinner))
            .Select(x => (Spinner)x.GetValue(null))
            .ToArray();

        var index = Random.Shared.Next(values.Length);
        var value = (Spinner)values.GetValue(index);
        return value;
    }
    
    private static Markup ToMarkup(string text)
    {
        try
        {
            return new Markup(text ?? string.Empty);
        }
        catch
        {
            return ErrorMarkup;
        }
    }

    private static readonly Markup ErrorMarkup = new(Emoji.Known.CrossMark);    
}