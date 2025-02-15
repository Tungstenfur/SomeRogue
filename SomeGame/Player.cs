using RLNET;

namespace SomeGame;

public class Player:Actor
{
    public Player()
    {
        Name = "Rogue";
        Awareness = 15;
        Color = RLColor.Cyan;
        Symbol = (char)2;
        X = 10;
        Y = 10;
    }
}