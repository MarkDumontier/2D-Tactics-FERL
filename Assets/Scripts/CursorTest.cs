using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class CursorTest : MonoBehaviour {

    public GameObject blueTransparency; //The blue transparent tile to highlight movement;

    private Rigidbody2D rb2D; //The Rigidbody2D coponent attached to this object
    private bool pinned;      
    private bool hasJustMoved;
    private Transform highlightHolder;                              //A variable to store a reference to the transform of our highlight object used for organizing the heiarchy.

    //Subscribe to the OnMoved event.
    void OnEnable()
    {
        CursorMove.OnMoved += MoveEvent;
    }


    void OnDisable()
    {
        CursorMove.OnMoved -= MoveEvent;
    }

    // Use this for initialization
    void Start ()
    {
        rb2D = GetComponent<Rigidbody2D>();
        highlightHolder = new GameObject("Highlight").transform;
    }
	
    void Update()
    {
        HighlightMoveRange();
        
        //if (TerrainAtLocation.IsPresent(rb2D.position))
        //{
        //    TerrainTile tile = TerrainAtLocation.GetTile(rb2D.position);
        //    Debug.Log(tile.terrainType);
        //}
        //else Debug.Log("Nothing here.");
    }

    //If the cursor has just moved, check if there is a player unit at the cursor's location. If there is, highlight its move range.
    void HighlightMoveRange()
    {

        if (hasJustMoved == true)
        {
            rb2D = GetComponent<Rigidbody2D>();
            if (UnitAtLocation.IsPlayerUnit(rb2D.position))
            {
                PlayerUnit unit = UnitAtLocation.GetPlayerUnit(rb2D.position);
                //Debug.Log(unit);
                List<TerrainTile> inRange = unit.ShowMoveRange();
                foreach (TerrainTile tile in inRange)
                {
                    GameObject blueMoveTile = Instantiate(blueTransparency, tile.GetLocation(), Quaternion.identity) as GameObject;
                    blueMoveTile.transform.SetParent(highlightHolder);
                }
            }
            else
            {
                foreach (Transform child in highlightHolder)
                {
                    Destroy(child.gameObject);
                }
                Debug.Log("Nothing here.");
            }
            hasJustMoved = false;
        }
    }

    //This event occurs after the cursor moves.
    void MoveEvent()
    {
        hasJustMoved = true;
    }

}
