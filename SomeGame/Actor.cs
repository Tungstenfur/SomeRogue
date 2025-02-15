using RLNET;
using RogueSharp;

namespace SomeGame;
using SomeGame.Interfaces;
public abstract class Actor:IActor, IDrawable
{
    private MainMap _map = new MainMap();
    // IActor
    public string Name { get; set; }
    public int Awareness { get; set; }
 
    // IDrawable
    public RLColor Color { get; set; }
    public char Symbol { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public void Draw(RLConsole console, IMap map)
    {
        if(MainMap.FOV.IsInFov(X,Y))
            console.Set(X, Y, RLColor.Cyan, RLColor.Gray, Symbol);
        else
        {
            console.Set(X,Y,RLColor.Gray,RLColor.Black, '?');
        }
    }
}