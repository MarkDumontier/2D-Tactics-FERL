using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTile : MonoBehaviour
{
    public string terrainType; //The terrain type of this tile.

    private Vector2 location; //The location of the tile.
    private Rigidbody2D rb2D; //The Rigidbody2D of the tile.

    void Awake()
    {
        //Initialize location.
        rb2D = this.GetComponent<Rigidbody2D>();
        location = rb2D.position;
    }

    //Returns a Vector2 with the x and y position of the tile.
    //This is used instead of making location public because we don't want the terrain tile being moved around.
    public Vector2 GetLocation()
    {
        return location;
    }
}