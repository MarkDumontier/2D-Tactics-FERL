using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerrainAtLocation
{

    private static Collider2D colliderOnSpace; //Used to store the collider we are examining.
    private static Rigidbody2D rb2D;            //Used to store the Rigidbody2D of the tile.
    private static int moveCost;                //Used to store the move cost value of the tile. 


    //Takes a Vector2 location. Returns true if a terrain tile exists at this location, otherwise returns false.
    public static bool IsPresent(Vector2 location)
    {
        //Check if a terrain tile exists here. If not, return false.
        if (GetCollider(location) == null)
        {
            return false;
        }
        else return true;
    }

    //Takes a Vector2 location. Returns the Collider2D of the terrain tile on this location.
    public static Collider2D GetCollider(Vector2 location)
    {
        //Check if there are any colliders in the terrain layer within a 1-unit large box at this location.
        colliderOnSpace = Physics2D.OverlapBox(location, new Vector2(1f, 1f), 0f, 1 << 8);
        return colliderOnSpace;
    }

    //Takes a Vector2 location. Returns the Rigidbody2D of the terrain tile on this location.
    public static Rigidbody2D GetRigidbody2D(Vector2 location)
    {
        //Get the Collider2D of the terrain tile.
        colliderOnSpace = GetCollider(location);
        //Get the GameObject associated with that collider.
        GameObject gameOb = colliderOnSpace.gameObject;
        //Get the Rigidbody2D of the terrain tile.
        rb2D = colliderOnSpace.GetComponent<Rigidbody2D>();
        return rb2D;
    }

    //Takes a Vector2 location. Returns the TerrainTile of the terrain tile on this location.
    public static TerrainTile GetTile(Vector2 location)
    {
        //Get the Collider2D of the terrain tile.
        colliderOnSpace = GetCollider(location);
        //Get the GameObject associated with that collider.
        GameObject gameOb = colliderOnSpace.gameObject;
        //Get the TerrainTile script from the tile.
        TerrainTile tile = gameOb.GetComponent<TerrainTile>();
        //REturn the tile.
        return tile;
    }

    //Takes a TerrainTile. Returns a list of neighboring tiles.
    public static List<TerrainTile> GetNeighbors(TerrainTile tile)
    {
        List<TerrainTile> neighbors = new List<TerrainTile>();
        Vector2 tileLocation = tile.GetLocation();
        float xCoord;
        float yCoord;

        //Add the tile to the North.
        xCoord = tileLocation.x;
        yCoord = tileLocation.y + 1;
        Vector2 newTile = new Vector2(xCoord, yCoord);
        if (TerrainAtLocation.IsPresent(newTile))
        {
            neighbors.Add(TerrainAtLocation.GetTile(newTile));
        }

        //Add the tile to the East
        xCoord = tileLocation.x + 1;
        yCoord = tileLocation.y;
        newTile = new Vector2(xCoord, yCoord);
        if (TerrainAtLocation.IsPresent(newTile))
        {
            neighbors.Add(TerrainAtLocation.GetTile(newTile));
        }

        //Add the tile to the South
        xCoord = tileLocation.x;
        yCoord = tileLocation.y - 1;
        newTile = new Vector2(xCoord, yCoord);
        if (TerrainAtLocation.IsPresent(newTile))
        {
            neighbors.Add(TerrainAtLocation.GetTile(newTile));
        }

        //Add the tile to the West
        xCoord = tileLocation.x - 1;
        yCoord = tileLocation.y;
        newTile = new Vector2(xCoord, yCoord);
        if (TerrainAtLocation.IsPresent(newTile))
        {
            neighbors.Add(TerrainAtLocation.GetTile(newTile));
        }

        return neighbors;
    }

    //Takes a Vector2. Returns true if the tile is highlighted, otherwise returns false.
    public static bool IsHighlighted(Vector2 location)
    {
        colliderOnSpace = Physics2D.OverlapBox(location, new Vector2(1f, 1f), 0f, LayerMask.GetMask("Highlight"));
        if (colliderOnSpace == null)
        {
            return false;
        }
        else return true;
    }

}