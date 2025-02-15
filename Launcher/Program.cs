using System.Diagnostics;
using Spectre.Console;

Console.Title = "SomeRogue Launcher";
var font = FigletFont.Load("Doom.flf");
var rule = new Rule("[White]Launcher[/]");
showMain();

void showMain()
{
    AnsiConsole.Write(new FigletText(font, "SomeRogue").Centered().Color(Color.White));
    AnsiConsole.Write(rule);

    var op = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .AddChoices(new[] { "Start", "Settings", "Update", "Exit" })
    );
    AnsiConsole.Write(new Text(op).Centered());

    if (op == "Start")
    {
        Process.Start("Somegame.exe");
        return;
    }

    if (op == "Settings")
    {
        showSettings();
    }
    else if(op == "Update")
    {
        getUpdate();
    }
    else if (op == "Exit")
    {
        return;
    }
}

void showSettings()
{
    AnsiConsole.Clear();
    AnsiConsole.Write(new Rule("[White]Settings[/]").Centered());
    var op = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .AddChoices(new[] { "Change volume", "Change scaling", "Controls", "Random seed", "Fullscreen mode", "Import settings", "Back" })
    );
    if (op == "Back")
    {
        AnsiConsole.Clear();
        showMain();
    }
    if (op == "Change volume")
    {
        AnsiConsole.Clear();
        var volume = AnsiConsole.Ask<string>("Enter volume level [[0-100]]");
        if (int.TryParse(volume, out int vol))
        {
            if (vol >= 0 && vol <= 100)
            {
                // TODO: Logic for changing volume
                showSettings();
            }
            else
            {
                showError("Volume must be between 0 and 100");
                showSettings();
            }
        }
        else
        {
            showError("Invalid volume level");
            showSettings();
        }
    }

    if (op == "Change scaling")
    {
        AnsiConsole.Clear();
        var scaling = AnsiConsole.Ask<string>("Enter scaling level [[Decimal, 1 is default]]");
        if (double.TryParse(scaling, out double scale))
        {
            if (scale >= 0)
            {
                //TODO: Logic for changing scaling
                showSettings();
            }
            else
            {
                showError("Scaling must be greater than 0");
                showSettings();
            }
        }
        else
        {
            showError("Invalid scaling level");
            showSettings();
        }
    }

    if (op == "Controls")
    {
        AnsiConsole.Clear();
        var controls = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices(new[] { "WASD[[WIP]]", "Arrow keys", "Controller[[WIP]]" })
        );
        //TODO: Logic for changing controls
    }
}

void getUpdate()
{
    AnsiConsole.Status().Start("Checking for updates...", ctx =>
    {
        ctx.Spinner(Spinner.Known.BouncingBar);
    });
}

void showError(string text)
{
    AnsiConsole.Clear();
    AnsiConsole.Write(new Rule("[Red]Error[/]").Centered());
    AnsiConsole.Write(new Text(text).Centered());
    AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(new[] { "Ok" }));
}