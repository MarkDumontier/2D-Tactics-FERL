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

    //Takes a Vector2 location. Returns the movement cost of the tile on this location as an int.
    public static int MoveCost(Vector2 location)
    {
        //Get the Collider2D of the terrain tile.
        colliderOnSpace = GetCollider(location);
        //Get the GameObject associated with that collider.
        GameObject gameOb = colliderOnSpace.gameObject;
        //Get the TerrainTile script from the tile.
        TerrainTile tile = gameOb.GetComponent<TerrainTile>();
        //Get the moveCost from the TerrainTile script.
        moveCost = tile.moveCost;
        //Return the moveCost.
        return moveCost;
    }
}