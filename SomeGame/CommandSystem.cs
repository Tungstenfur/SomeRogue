namespace SomeGame;
using SomeGame.Interfaces;
public class CommandSystem
{
    public static bool MovePlayer( Directions direction )
    {
        int x = Game.player.X;
        int y = Game.player.Y;
 
        switch ( direction )
        {
            case Directions.Up:
            {
                y = Game.player.Y - 1;
                break;
            }
            case Directions.Down:
            {
                y = Game.player.Y + 1;
                break;
            }
            case Directions.Left:
            {
                x = Game.player.X - 1;
                break;
            }
            case Directions.Right:
            {
                x = Game.player.X + 1;
                break;
            }
            default:
            {
                return false;
            }
        }
 
        if ( Game.mainMap.SetActorPosition( Game.player, x, y ) )
        {
            return true;
        }
 
        return false;
    }
}