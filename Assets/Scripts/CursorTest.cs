﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class CursorTest : MonoBehaviour {

    private Rigidbody2D rb2D; //The Rigidbody2D coponent attached to this object


	// Use this for initialization
	void Start ()
    {

        rb2D = GetComponent<Rigidbody2D>();
	}
	
    void Update()
    {
        //if (UnitAtLocation.IsPresent(rb2D.position))
        //{
        //    Debug.Log("Unit here.");
        //}
        //else Debug.Log("Nothing here.");

        if (TerrainAtLocation.IsPresent(rb2D.position))
        {
            TerrainTile tile = TerrainAtLocation.GetTile(rb2D.position);
            Debug.Log(tile.GetLocation());
        }
        else Debug.Log("Nothing here.");
    }

}
