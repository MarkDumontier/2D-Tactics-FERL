using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTile : MonoBehaviour
{
    public int moveCost; //The amount of movement needed to enter this tile.

    private Vector2 location; //The location of the tile.
    private Rigidbody2D rb2D; //The Rigidbody2D of the tile.

    void Awake()
    {
        //Initialize location.
        rb2D = this.GetComponent<Rigidbody2D>();
        location = rb2D.position;
    }

    public Vector2 GetLocation()
    {
        return location;
    }
}