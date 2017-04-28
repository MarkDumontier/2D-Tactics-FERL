using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;
using System;
using System.Linq;

public class CursorManager : MonoBehaviour
{

    public GameObject blueTransparency;                             //The blue transparent tile to highlight movement;
    public GameObject redTransparency;                              //The red transparent tile to highlight attack range;
    public GameObject redThreatTile;                                //The red transparent tile to highlight enemy threat range;
    public GameObject redThreatBorder;                              //The red border line to highlight enemy threat range;


    private PlayerUnit pinnedUnit;                                  //Holds the PlayerUnit script of the unit the cursor is pinned to.
    private Vector2 unitStart;                                      //Holds the start location of the pinned unit.

    private Rigidbody2D rb2D;                                       //The Rigidbody2D coponent attached to this object
    private Transform highlightHolder;                              //A variable to store a reference to the transform of our highlight object used for organizing the heiarchy.
    private Transform threatZoneHolder;                             //A variable to store a reference to the transform of our enemy threat zones.

    private List<Unit> threatHighlightedEnemies;

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

        //Initialize the transforms to hold all our highlight tiles.
        highlightHolder = new GameObject("Highlight").transform;
        threatZoneHolder = new GameObject("ThreatZone").transform;
        threatHighlightedEnemies = new List<Unit>();

        //Initialize the cursor move mode scripts.
        freeMode = GetComponent<FreeCursorMove>();
        pinnedMode = GetComponent<PinnedCursorMove>();
    }

    void Update()
    {
            //Watch for Fire1 press. If over a player unit, highlights its range and pins the cursor to it.
            if (Input.GetButtonDown("Fire1"))
        {
            //Free mode options
            if (freeMode.enabled)
            {
                //If the unit is a player unit, pin it.
                if (UnitAtLocation.IsPlayerUnit(rb2D.position))
                {
                    //Pin the cursor to the player unit.
                    PinPlayer();
                }
                //If the unit here is an enemy unit, create an attack range preview.
                else if (UnitAtLocation.IsEnemyUnit(rb2D.position))
                {
                    //Create enemy attack preview
                    Unit unit = UnitAtLocation.GetUnit(rb2D.position);
                    if (threatHighlightedEnemies.Contains(unit))
                    {
                        threatHighlightedEnemies.Remove(unit);
                    }
                    else
                    {
                        threatHighlightedEnemies.Add(unit);
                    }
                    EnemyAttackPreview();
                }
                //If there is nothing here, open menu.
                else if (!UnitAtLocation.IsPresent(rb2D.position))
                {
                    //Open menu
                }
            }

            //Pinned mode options
            else if (pinnedMode.enabled)
            {
                if (!UnitAtLocation.IsPresent(rb2D.position))
                {
                    //Get the Rigidbody2D of the pinned unit.
                    Rigidbody2D unitRB2D = pinnedUnit.GetComponent<Rigidbody2D>();
                    //Save the unit's start position
                    unitStart = unitRB2D.position;
                    //Move the unit
                    if (pinnedUnit.Move(rb2D.position))
                    {
                        //Clear any highlights.
                        foreach (Transform child in highlightHolder)
                        {
                            Destroy(child.gameObject);
                        }
                        //Redraw the threat highlighting of enemy units
                        StartCoroutine("RefreshAttackPreview");

                        //set mode attack

                    }
                   
                }
            }   
        }

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
        //Only change the highlighting if cursor is in free move mode.
        if (freeMode.enabled)
        {
            //Clear any previous highlights if in free mode
            foreach (Transform child in highlightHolder)
            {
                Destroy(child.gameObject);
            }

            rb2D = GetComponent<Rigidbody2D>();
            if (UnitAtLocation.IsPresent(rb2D.position))
            {
                //Get a reference to the unit the cursor is over.
                Unit unit = UnitAtLocation.GetUnit(rb2D.position);

                //Initialize a list to hold all tiles within move range.
                List<TerrainTile> inMoveRange = unit.ShowMoveRange();

                //For each tile in move range, display a blue transparency over the tile.
                foreach (TerrainTile tile in inMoveRange)
                {
                    GameObject blueMoveTile = Instantiate(blueTransparency, tile.GetLocation(), Quaternion.identity) as GameObject;
                    //Add the tile to the highlightHolder so it can be managed more easily
                    blueMoveTile.transform.SetParent(highlightHolder);
                }

                //Initialize a list to hold all tiles within attack range.
                List<TerrainTile> inAttackRange = new List<TerrainTile>();

                //For each tile in move range, find all tiles in attack range and add them to a list.
                foreach(TerrainTile tile in inMoveRange)
                {
                    inAttackRange.AddRange(unit.GetThreatenedTiles(tile.GetLocation()));
                }

                //For each tile in the attack range list, display a red transparency if the tile is not already highlighted.
                foreach (TerrainTile tile in inAttackRange)
                {
                    if (!TerrainAtLocation.IsHighlighted(tile.GetLocation())){
                        GameObject redMoveTile = Instantiate(redTransparency, tile.GetLocation(), Quaternion.identity) as GameObject;
                        //Add the tile to the highlightHolder so it can be managed more easily
                        redMoveTile.transform.SetParent(highlightHolder);
                    }
                }
            }
            else
            {
                Debug.Log("Nothing here.");
            } 
        }
    }

    //This event occurs after the cursor moves. Should only be thrown during the FixedUpdate so that the cursor has time to move before its position is checked again.
    void MoveEvent()
    {
        StartCoroutine("RefreshHighlight");
    }

    //Pin the cursor to the selected player unit.
    void PinPlayer()
    {
        //Check to make sure we were in free mode before selecting the unit.
        if (freeMode.enabled)
        {
            //Change the cursor move mode to pinned.
            SetMode("pinned");
            //Store a reference to the pinned unit.
            pinnedUnit = UnitAtLocation.GetPlayerUnit(rb2D.position);
        }      
    }

    //Changes the movement mode of our cursor.
    public void SetMode(string mode)
    {
        //Get a reference to our move mode scripts
        freeMode = GetComponent<FreeCursorMove>();
        pinnedMode = GetComponent<PinnedCursorMove>();

        //String input determines which mode to switch to.
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


    //Highlights the threat range of an enemy unit
    void EnemyAttackPreview()
    {
        //Clear old threat highlights
        foreach (Transform child in threatZoneHolder)
        {
            Destroy(child.gameObject);
        }

        List<TerrainTile> threatHighlight = new List<TerrainTile>();

        foreach (Unit unit in threatHighlightedEnemies)
        {
            List<TerrainTile> threatZone = unit.GetThreatZone();
            

            //For each tile in the attack range list, display a red transparency if the tile is not already highlighted.
            foreach (TerrainTile tile in threatZone)
            {
                if (!TerrainAtLocation.IsThreatHighlighted(tile.GetLocation()))
                {
                    GameObject threatTile = Instantiate(redThreatTile, tile.GetLocation(), Quaternion.identity) as GameObject;
                    //Add the tile to threatZoneHolder so it can be managed more easily
                    threatTile.transform.SetParent(threatZoneHolder);
                    threatHighlight.Add(tile);
                }
            }
        }

        //Add the red border around the threat zone.
        foreach (TerrainTile tile in threatHighlight)
        {
            Vector2 tileLocation = tile.GetLocation();
            //Neighbor holds the location of the neighboring tile.
            Vector2 neighbor = new Vector2();

            float xCoord = tileLocation.x;
            float yCoord = tileLocation.y;

            //North neighbor
            neighbor = new Vector2(xCoord, yCoord + 1);
            if (!TerrainAtLocation.IsThreatHighlighted(neighbor) && TerrainAtLocation.IsPresent(neighbor))
            {
                GameObject redBorderLine = Instantiate(redThreatBorder, tile.GetLocation(), Quaternion.Euler(0, 0, 270)) as GameObject;
                redBorderLine.transform.SetParent(threatZoneHolder);
            }

            //West neighbor
            neighbor = new Vector2(xCoord + 1, yCoord);
            if (!TerrainAtLocation.IsThreatHighlighted(neighbor) && TerrainAtLocation.IsPresent(neighbor))
            {
                GameObject redBorderLine = Instantiate(redThreatBorder, tile.GetLocation(), Quaternion.Euler(0, 0, 180)) as GameObject;
                redBorderLine.transform.SetParent(threatZoneHolder);
            }

            //South neighbor
            neighbor = new Vector2(xCoord, yCoord - 1);
            if (!TerrainAtLocation.IsThreatHighlighted(neighbor) && TerrainAtLocation.IsPresent(neighbor))
            {
                GameObject redBorderLine = Instantiate(redThreatBorder, tile.GetLocation(), Quaternion.Euler(0, 0, 90)) as GameObject;
                redBorderLine.transform.SetParent(threatZoneHolder);
            }

            //East neighbor
            neighbor = new Vector2(xCoord - 1, yCoord);
            if (!TerrainAtLocation.IsThreatHighlighted(neighbor) && TerrainAtLocation.IsPresent(neighbor))
            {
                GameObject redBorderLine = Instantiate(redThreatBorder, tile.GetLocation(), Quaternion.Euler(0, 0, 0)) as GameObject;
                redBorderLine.transform.SetParent(threatZoneHolder);
            }
        }
    }

    IEnumerator RefreshAttackPreview()
    {
        yield return new WaitForFixedUpdate();
        EnemyAttackPreview();
        yield return null;
    }

    IEnumerator RefreshHighlight()
    {
        yield return new WaitForFixedUpdate();
        HighlightMoveRange();
        yield return null;
    }
}