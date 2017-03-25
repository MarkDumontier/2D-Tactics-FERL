using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTest : MonoBehaviour {

    private BoxCollider2D boxCollider; //The BoxCollider2D component attached to this object
    private Collider2D[] colliderOnSpace; //A list of all Colliders on the cursor's space
    private Rigidbody2D rb2D; //The Rigidbody2D coponent attached to this object


	// Use this for initialization
	void Start ()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        Debug.Log(boxCollider.name, boxCollider);

	}
	
    void Update()
    {
        colliderOnSpace = Physics2D.OverlapBoxAll(rb2D.position, new Vector2(1f , 1f), 0f, 1 << 8);
        Debug.Log(colliderOnSpace[0].name);
        Debug.Log(colliderOnSpace[0].attachedRigidbody.position);
        Debug.Log(colliderOnSpace.Length);
    }

}
