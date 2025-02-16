using System.Net;
using System.Net.NetworkInformation;
using Spectre.Console;
Main();
void Main()
{
    Console.Title = "SomeRogue Installer";
    var font = FigletFont.Load("Doom.flf");
    var rule = new Rule("[White]Installer[/]");
    AnsiConsole.Write(new FigletText(font, "SomeRogue").Centered().Color(Color.White));
    AnsiConsole.Write(rule);
    var op = AnsiConsole.Prompt( new SelectionPrompt<string>()
        .AddChoices(new[] { "Install", "Uninstall","Update", "Exit" }));
    if(op=="Install")
        Install();
}

void Install()
{
    AnsiConsole.Clear();
    AnsiConsole.Status().Start("Installing", ctx =>
    {
        if (OperatingSystem.IsWindows())
        {
            AnsiConsole.MarkupLine("[blue]Operating system is Windows[/]");
            if(new Ping().Send("github.com").Status==IPStatus.Success)
            {
                AnsiConsole.MarkupLine("[blue]Internet connection is available[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]Internet connection is not available[/]");
                Console.ReadKey();
            }
        }
    });
}
