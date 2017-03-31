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
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //Checks for direction input. If there is direction input, attempt to move.
        WatchForMove();
    }

    public override bool Move(int xDir, int yDir)
    {
        return base.Move(xDir, yDir);
    }
}
