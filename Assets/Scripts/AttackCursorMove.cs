using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCursorMove : CursorMove {


	// Use this for initialization
	void Start () {
        //Get a component refrence to this object's Rigidbody2D.
        rb2D = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

    public override bool Move(int xDir, int yDir)
    {
        Debug.Log("ATTACK");
        return base.Move(xDir, yDir);
    }
}
