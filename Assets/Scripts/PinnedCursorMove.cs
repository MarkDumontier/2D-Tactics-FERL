using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinnedCursorMove : CursorMove {

    CursorManager cursorManager;                        //Used tos tore a reference to our CursorManager script.

    private bool moveButtonHeld;

    // Use this for initialization
    void Start()
    {
        //Get a component refrence to this object's Rigidbody2D.
        rb2D = GetComponent<Rigidbody2D>();

        cursorManager = this.GetComponent<CursorManager>();
        moveButtonHeld = false;
    }

    // This needs to be fixed update to ensure the cursor gets to its destination before we check the cursor's space.
    void FixedUpdate()
    {
        //Checks for direction input. If there is direction input, attempt to move.
        WatchForMove();
    }

    public override void WatchForMove()
    {
        int horizontal = 0;     //Used to store the horizontal move direction
        int vertical = 0;       //Used to store the vertical move direction

        //Get input from the input manager, round it to an integer and store in horizontal to set x-axis move direction.
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));

        //Get input from the input manager, round it to an integer and store in vertical to set y-axis move direction.
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        //Check if we have a non-zero value for horizontal or vertical.
        if (horizontal != 0 || vertical != 0)
        {
            //When holding the movement keys, movement is locked to the highlighted area. Pausing at the boundary and then moving off allows free cursor movement.
            if( moveButtonHeld == false)
            {
                moveButtonHeld = true;
                //Check to see if we are waiting between movement steps.
                if (isPaused == false)
                {
                    //Attempt to move. The cursor will not move if its destination is not on a terrain tile.
                    bool didMove = Move(horizontal, vertical);

                    if (didMove)
                    {
                        //If the cursor moves, trigger the OnMove event as long as something is subscribed to it.
                        TriggerOnMovedEvent();
                        //Disable movement until a given time has passed.
                        isPaused = true;
                        //Coroutine is used as a timer to re-enable movement after a given time.
                        StartCoroutine(PauseMovement());
                    }
                }
            }
            else
            {
                Vector2 start = rb2D.position;
                Vector2 end = new Vector2(start.x + horizontal, start.y + vertical);

                //Prevent movement from a highlighted to non-highlighted space.
                if (!TerrainAtLocation.IsHighlighted(start) || TerrainAtLocation.IsHighlighted(end))
                {
                    //Check to see if we are waiting between movement steps.
                    if (isPaused == false)
                    {
                        //Attempt to move. The cursor will not move if its destination is not on a terrain tile.
                        bool didMove = Move(horizontal, vertical);

                        if (didMove)
                        {
                            //If the cursor moves, trigger the OnMove event as long as something is subscribed to it.
                            TriggerOnMovedEvent();
                            //Disable movement until a given time has passed.
                            isPaused = true;
                            //Coroutine is used as a timer to re-enable movement after a given time.
                            StartCoroutine(PauseMovement());
                        }
                    }
                }
                
            }
        }
        else
        {
            moveButtonHeld = false;
        }
    }
}
