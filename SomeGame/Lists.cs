using System.ComponentModel.DataAnnotations;

namespace SomeGame;

public class Lists
{
    // Format: Name, Health, Attack, Awareness, char
    public Dictionary<string, (int Health, int Attack, int Awareness, char c)> EnemyList = new();
    public Dictionary<string,(string stat, int value)> ItemList = new();
    public Lists()
    {
        EnemyList.Add("Zombie", (2, 1, 1, 'Z'));
        EnemyList.Add("Orc", (3, 2, 2, 'O'));
        EnemyList.Add("Goblin", (1, 1, 1, 'G'));

    }
}