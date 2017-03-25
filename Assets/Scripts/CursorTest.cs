using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTest : MonoBehaviour {

    private Rigidbody2D rb2D; //The Rigidbody2D coponent attached to this object


	// Use this for initialization
	void Start ()
    {
        rb2D = GetComponent<Rigidbody2D>();
	}
	
    void Update()
    {
        if (TerrainAtLocation.IsPresent(rb2D.position))
        {
            Debug.Log(TerrainAtLocation.MoveCost(rb2D.position));
        }
        else Debug.Log("Nothing here.");

    }

}
