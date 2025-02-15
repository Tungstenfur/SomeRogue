using System;
using System.Diagnostics;
using RLNET;
using RogueSharp;
using SomeGame;
using SomeGame.Interfaces;
using System.Windows.Input;
public class Game
{
    public static MainMap mainMap { get; private set; }
    private static RLRootConsole rootConsole;
    private static RLConsole statConsole;
    private static RLConsole messageConsole;
    private static RLConsole inventoryConsole;
    private static RLConsole mapConsole;
    private static string[] messages = new string[10];
    public static Player player = new Player();
    public static Random random = new Random();
    public static CommandSystem commandSystem { get; private set; }
    string message;
    static private int i=0;
    const int mainWidth = 100;
    const int mainHeight = 70;
    const int statWidth = 20;
    const int statHeight = 70;
    const int messageWidth = 80;
    const int messageHeight = 10;
    const int inventoryWidth = 80;
    const int inventoryHeight = 8;

    void playerMoveTick()
    {

    }
    static public void displayMessage(string message)
    {
        for (int i = messages.Length - 1; i > 0; i--)
        {
            messages[i] = messages[i - 1];
        }
        messages[0] = message;
    }
    private static bool _renderRequired = true;
    static bool didPlayerAct = false;
    public static void Main()
    {
        
        
        rootConsole = new RLRootConsole("terminal8x8.png", mainWidth, mainHeight, 8, 8, 1f, "SomeGame");
        statConsole = new RLConsole(statWidth, statHeight);
        messageConsole = new RLConsole(messageWidth, messageHeight);
        inventoryConsole = new RLConsole(inventoryWidth, inventoryHeight);
        mapConsole = new RLConsole(mainWidth - statWidth, mainHeight - messageHeight - inventoryHeight);
        MapGen mapGenerator = new MapGen(mainWidth - statWidth, mainHeight - messageHeight - inventoryHeight);
        mainMap = mapGenerator.CreateMap();
        rootConsole.Update += OnRootConsoleUpdate;
        rootConsole.Render += OnRootConsoleRender;
        commandSystem = new CommandSystem();
        Debug.WriteLine("Starting game");
        displayMessage("Initialization completed");
        displayMessage("You wake up in unknown place");
        mainMap.UpdatePlayerFieldOfView();
        rootConsole.Run();
        
    }

    private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
    {
        RLKeyPress keyPress = rootConsole.Keyboard.GetKeyPress();
        if (KeyPress.IsKeyPressed(ConsoleKey.UpArrow))
        {
            Debug.WriteLine("It worked");
        }
        if ( keyPress != null )
        {
            if ( keyPress.Key == RLKey.Up )
            {
                didPlayerAct = CommandSystem.MovePlayer( Directions.Up );
            }
            else if ( keyPress.Key == RLKey.Down )
            {
                didPlayerAct = CommandSystem.MovePlayer( Directions.Down );
            }
            else if ( keyPress.Key == RLKey.Left )
            {
                didPlayerAct = CommandSystem.MovePlayer( Directions.Left );
            }
            else if ( keyPress.Key == RLKey.Right )
            {
                didPlayerAct = CommandSystem.MovePlayer( Directions.Right );
            }
            else if ( keyPress.Key == RLKey.Escape )
            {
                rootConsole.Close();
            }
        }
 
        if ( didPlayerAct )
        {
            _renderRequired = true;
        }
        statConsole.SetBackColor(0, 0, statWidth, statHeight, RLColor.White);
        statConsole.Print(1, 1, "Stats", RLColor.Black);
        messageConsole.SetBackColor(0, 0, messageWidth, messageHeight, RLColor.Cyan);
        messageConsole.Print(1, 1, "Messages", RLColor.Black);
        i = 0;
        foreach (string message in messages)
        {
            messageConsole.Print(1, i+2, message, RLColor.Black);
            i++;
        }
        inventoryConsole.SetBackColor(0, 0, inventoryWidth, inventoryHeight, RLColor.Brown);
        inventoryConsole.Print(1, 1, "Inventory", RLColor.Black);
        //mapConsole.SetBackColor(0, 0, mainWidth - statWidth, mainHeight - messageHeight - inventoryHeight, RLColor.LightRed);
        //mapConsole.Print(1, 1, "Map", RLColor.White);
    }

    private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
    {
        RLConsole.Blit(statConsole, 0, 0, statWidth, statHeight, rootConsole, 80, 0);
        RLConsole.Blit(messageConsole, 0, 0, messageWidth, messageHeight, rootConsole, 0, 60);
        RLConsole.Blit(inventoryConsole, 0, 0, inventoryWidth, inventoryHeight, rootConsole, 0, 0);
        RLConsole.Blit(mapConsole, 0, 0, mainWidth - statWidth, mainHeight - messageHeight - inventoryHeight, rootConsole, 0, 8);
        mainMap.Draw(mapConsole);
        player.Draw(mapConsole, mainMap);
        rootConsole.Draw();
    }
}