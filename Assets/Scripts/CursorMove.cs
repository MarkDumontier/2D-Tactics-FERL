using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMove : MonoBehaviour
{

    private Rigidbody2D rb2D;                               //The Rigidbody2D component attached to this object.
    private bool isPaused = false;                          //Bool used to keep delay movement so that cursor doesn't move too quickly.
    public float movementPauseTime = 0.1f;                  //Time to wait between movement steps.


    // Use this for initialization
    void Start()
    {
        //Get a component refrence to this object's Rigidbody2D.
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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
            //Check to see if we are waiting between movement steps.
            if (isPaused == false)
            {
                //Attempt to move. The cursor will not move if its destination is not on a terrain tile.
                Move(horizontal, vertical);
                //Disable movement until a given time has passed.
                isPaused = true;
                //Coroutine is used as a timer to re-enable movement after a given time.
                StartCoroutine(PauseMovement());
            }
        }
    }

    //Function to move our cursor. 
    private bool Move(int xDir, int yDir)
    {
        //Get the cursor's start location.
        Vector2 start = transform.position;

        //Get the cursor's desired end location.
        Vector2 end = start + new Vector2(xDir, yDir);

        //Check to make sure the cursor's end location is on the board. If it is, move the cursor.
        if (TerrainAtLocation.IsPresent(end))
        {
            rb2D.MovePosition(end);
            return true;
        }

        return false;
    }

    //Coroutine that re-enables movement after a given time
    IEnumerator PauseMovement()
    {
        yield return new WaitForSeconds(movementPauseTime);
        isPaused = false;
    }
}