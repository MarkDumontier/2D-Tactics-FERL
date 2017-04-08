using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCursorMove : CursorMove {

	// Use this for initialization
	void Start ()
    {
        //Get a component refrence to this object's Rigidbody2D.
        rb2D = GetComponent<Rigidbody2D>();
    }
	
	// This needs to be fixed update to ensure the cursor gets to its destination before we check the cursor's space.
	void FixedUpdate ()
    {
        //Checks for direction input. If there is direction input, attempt to move.
        WatchForMove();
    }

    public override bool Move(int xDir, int yDir)
    {
        Debug.Log("FREE");
        return base.Move(xDir, yDir);
    }

}
