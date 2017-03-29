using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class CursorTest : MonoBehaviour {

    public GameObject blueTransparency; //The blue transparent tile to highlight movement;

    private Rigidbody2D rb2D; //The Rigidbody2D coponent attached to this object
    private bool pinned;      //



	// Use this for initialization
	void Start ()
    {
        rb2D = GetComponent<Rigidbody2D>();
	}
	
    void Update()
    {
        if (UnitAtLocation.IsPlayerUnit(rb2D.position))
        {
            PlayerUnit unit = UnitAtLocation.GetPlayerUnit(rb2D.position);
            //Debug.Log(unit);
            List<TerrainTile> inRange = unit.ShowMoveRange();
            foreach(TerrainTile tile in inRange)
            {
                GameObject blueMoveTile = Instantiate(blueTransparency, tile.GetLocation(), Quaternion.identity) as GameObject;
            }
        }
        else Debug.Log("Nothing here.");

        //if (TerrainAtLocation.IsPresent(rb2D.position))
        //{
        //    TerrainTile tile = TerrainAtLocation.GetTile(rb2D.position);
        //    Debug.Log(tile.terrainType);
        //}
        //else Debug.Log("Nothing here.");
    }

}
