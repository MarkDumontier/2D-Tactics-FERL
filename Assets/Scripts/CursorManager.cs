using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class CursorManager : MonoBehaviour
{

    public GameObject blueTransparency;                             //The blue transparent tile to highlight movement;

    private PlayerUnit pinnedUnit;                                   //Holds the PlayerUnit script of the unit the cursor is pinned to. 

    private Rigidbody2D rb2D;                                       //The Rigidbody2D coponent attached to this object
    private bool pinned;
    private bool hasJustMoved;
    private Transform highlightHolder;                              //A variable to store a reference to the transform of our highlight object used for organizing the heiarchy.

    private FreeCursorMove freeMode;                                //Holds our FreeCursorMove script.
    private PinnedCursorMove pinnedMode;                            //Holds our PinnedCursorMove script.

    

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
    void Start()
    {
        //Get a reference to the Rigidbody2D of the cursor
        rb2D = GetComponent<Rigidbody2D>();
        //Initialize the transform to hold all our highlight tiles.
        highlightHolder = new GameObject("Highlight").transform;
        //Initialize the cursor move mode scripts.
        freeMode = GetComponent<FreeCursorMove>();
        pinnedMode = GetComponent<PinnedCursorMove>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (UnitAtLocation.IsPlayerUnit(rb2D.position))
            {
                //Debug.Log("player selected");

                SelectPlayer();
            }
        }

        HighlightMoveRange();

        //Fire2 cancels pinned mode if active
        if (Input.GetButtonDown("Fire2"))
        {
            if (pinnedMode.enabled)
            {
                Rigidbody2D pinnedUnitRB2D = pinnedUnit.GetComponent<Rigidbody2D>();
                rb2D.MovePosition(pinnedUnitRB2D.position);
                SetMode("free");
            }
        }

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
        //Only need to update if the cursor has just moved.
        if (hasJustMoved == true)
        {
            //Only change the highlighting if cursor is in free move mode.
            if (freeMode.enabled)
            {
                //Clear any previous highlights if in free mode
                foreach (Transform child in highlightHolder)
                {
                    Destroy(child.gameObject);
                }

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
                    Debug.Log("Nothing here.");
                }
                hasJustMoved = false;
            }
        }
    }

    //This event occurs after the cursor moves.
    void MoveEvent()
    {
        hasJustMoved = true;
    }

    void SelectPlayer()
    {
        if (freeMode.enabled)
        {
            SetMode("pinned");
            pinnedUnit = UnitAtLocation.GetPlayerUnit(rb2D.position);
            Debug.Log(pinnedUnit);
        }
    }

    public void SetMode(string mode)
    {
        freeMode = GetComponent<FreeCursorMove>();
        pinnedMode = GetComponent<PinnedCursorMove>();
        switch (mode)
        {
            case "free":
                pinnedMode.enabled = false;
                freeMode.enabled = true;
                Debug.Log("Mode set to free");
                break;
            case "pinned":
                freeMode.enabled = false;
                pinnedMode.enabled = true;
                Debug.Log("Mode set to pinned");
                break;
            default:
                Debug.LogError("Error: Invalid Cursor Mode.");
                break;
        }
    }

    //Gets the PlayerUnit script pinnedUnit.
    public PlayerUnit GetPinnedUnit()
    {
        return pinnedUnit;
    }
}