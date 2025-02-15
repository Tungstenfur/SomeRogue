using RogueSharp;

namespace SomeGame;

public class MapGen
{
    private readonly int _width;
    private readonly int _height;
    private readonly MainMap _map;
    public MapGen(int width, int height)
    {
        _width = width;
        _height = height;
        _map = new MainMap();
    }
    public MainMap CreateMap()
    {
        // Initialize every cell in the map by
        // setting walkable, transparency, and explored to true
        _map.Initialize( _width, _height );
        foreach ( Cell cell in _map.GetAllCells() )
        {
            _map.SetCellProperties( cell.X, cell.Y, true, true );
        }
 
        // Set the first and last rows in the map to not be transparent or walkable
        foreach ( Cell cell in _map.GetCellsInRows( 0, _height - 1 ) )
        {
            _map.SetCellProperties( cell.X, cell.Y, false, false);
        }
 
        // Set the first and last columns in the map to not be transparent or walkable
        foreach ( Cell cell in _map.GetCellsInColumns( 0, _width - 1 ) )
        {
            _map.SetCellProperties( cell.X, cell.Y, false, false);
        }
 
        return _map;
    }
}