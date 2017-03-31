using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinnedCursorMove : CursorMove {

    CursorManager cursorManager;                        //Used tos tore a reference to our CursorManager script.
    
    // Use this for initialization
    void Start()
    {
        //Get a component refrence to this object's Rigidbody2D.
        rb2D = GetComponent<Rigidbody2D>();

        cursorManager = this.GetComponent<CursorManager>();
    }

    // This needs to be fixed update to ensure the cursor gets to its destination before we check the cursor's space.
    void FixedUpdate()
    {
        //Checks for direction input. If there is direction input, attempt to move.
        WatchForMove();
    }

    public override bool Move(int xDir, int yDir)
    {
        Debug.Log("PINNED");
        //Get the cursor's start location.
        Vector2 start = transform.position;

        //Get the cursor's desired end location.
        Vector2 end = start + new Vector2(xDir, yDir);

        //Check to make sure the cursor's end location is highlighted or the pinned unit. If it is, move the cursor, otherwise return false.
        if (UnitAtLocation.IsPresent(end))
        {
            if (UnitAtLocation.GetPlayerUnit(end) == cursorManager.GetPinnedUnit())
            {
                rb2D.MovePosition(end);
                return true;
            }
        }
        else if (TerrainAtLocation.IsHighlighted(end))
        {
            rb2D.MovePosition(end);
            return true;
        }

        return false;
    }
}
