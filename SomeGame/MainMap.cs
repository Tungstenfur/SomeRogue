using RogueSharp;
using RLNET;
using System.Collections.Generic;

namespace SomeGame
{
    public class MainMap : Map, IMap // Implement IMap for RogueSharp FOV
    {
        public List<Rectangle> Rooms;
        public Color color;
        public static FieldOfView FOV;
        public MainMap()
        {
            FOV = new FieldOfView(this);
            color= new Color();
        }
        public bool SetActorPosition( Actor actor, int x, int y )
        {
            // Only allow actor placement if the cell is walkable
            if ( GetCell( x, y ).IsWalkable )
            {
                // The cell the actor was previously on is now walkable
                SetIsWalkable( actor.X, actor.Y, true );
                // Update the actor's position
                actor.X = x;
                actor.Y = y;
                // The new cell the actor is on is now not walkable
                SetIsWalkable( actor.X, actor.Y, false );
                // Don't forget to update the field of view if we just repositioned the player
                if ( actor is Player )
                {
                    UpdatePlayerFieldOfView();
                }
                return true;
            }
            return false;
        }
        public void SetIsWalkable( int x, int y, bool isWalkable )
        {
            Cell cell = GetCell( x, y );
            SetCellProperties( cell.X, cell.Y, cell.IsTransparent, isWalkable);
        }
        public void UpdatePlayerFieldOfView()
        {
            Player player = Game.player;
            // Compute the field-of-view based on the player's location and awareness
            FOV.ComputeFov( player.X, player.Y, player.Awareness, true );
            // Mark all cells in field-of-view as having been explored
            foreach ( Cell cell in GetAllCells() )
            {
                if (FOV.IsInFov( cell.X, cell.Y ) )
                {
                    SetCellProperties( cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable);
                }
            }
        }
        public void Draw(RLConsole mapConsole)
        {
            mapConsole.Clear();
            foreach (Cell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }
        }
        private void SetConsoleSymbolForCell(RLConsole console, Cell cell)
        {
// When a cell is currently in the field-of-view it should be drawn with ligher colors
            if (FOV.IsInFov(cell.X, cell.Y))
            {
                // Choose the symbol to draw based on if the cell is walkable or not
                // '.' for floor and '#' for walls
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, RLColor.White, RLColor.Gray, (char)255);
                }
                else
                {
                    console.Set(cell.X, cell.Y, color.Orange, RLColor.Black, (char)219);
                }
            }
            // When a cell is outside of the field of view draw it with darker colors
            else
            {
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, RLColor.Gray, RLColor.Black, (char)255);
                }
                else
                {
                    console.Set(cell.X, cell.Y, color.DarkOrange, RLColor.Black, (char)219);
                }
            }
        }
    }
}