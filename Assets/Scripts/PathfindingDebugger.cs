using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindingDebugger : MonoBehaviour
{
    private static PathfindingDebugger instance;

    //Singleton Pattern - This limits the instantiation of a class to once so all current objects can access the resources within the single instance rather than repeatedly
    //Able to access PathfindingDebugger code without referencing an object directly that has these resources
    public static PathfindingDebugger MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PathfindingDebugger>();
            }

            return instance;
        }
    }

    //Allows for manipulation/use of a grid
    [SerializeField] private Grid grid;
    //Gives colour coding to the different squares on the grid for my reference
    [SerializeField] private Color startColour, endColour, currentColour, openColour, closedColour;
    //Gives access to the tilemap
    [SerializeField] private Tilemap tilemap;
    //The tile/square that will appear on the screen for the debugging
    [SerializeField] private Tile tile;
    //The text inside the tiles which will have the FCost, Gcost and HCost
    [SerializeField] private GameObject PathfindingInfo;

    //Holds the displayed squares on the screen - at each step through the pathfinding algorithm, the list is renewed
    private List<GameObject> debugObjects = new List<GameObject>();

    public void CreateTiles(Vector3Int start, Vector3Int end)
    {
        ColourTile(start, startColour);
        ColourTile(end, endColour);
    }

    public void ColourTile(Vector3Int position, Color colour)
    {
        //Instantiates the tile at position parameter
        tilemap.SetTile(position, tile);
        //Indicates that the current tile in the given position is to be changed
        tilemap.SetTileFlags(position, TileFlags.None);
        //The colour of the tile at the given position is changed to the new parameter colour
        tilemap.SetColor(position, colour);
    }
}
