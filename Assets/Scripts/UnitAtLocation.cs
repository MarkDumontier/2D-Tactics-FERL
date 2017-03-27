using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAtLocation
{

    private static Collider2D colliderOnSpace; //Used to store the collider we are examining


    //Takes a Vector2 location. Returns true if a unit exists at this location, otherwise returns false.
    public static bool IsPresent(Vector2 location)
    {
        //Check if a unit exists here. If not, return false.
        if (GetCollider(location) == null)
        {
            return false;
        }
        else return true;
    }

    //Takes a Vector2 location. Returns the Collider2D of the unit on this location.
    public static Collider2D GetCollider(Vector2 location)
    {
        //Check if there are any colliders in the unit layer within a 1-unit large box at this location.
        colliderOnSpace = Physics2D.OverlapBox(location, new Vector2(1f, 1f), 0f, 1 << 9);
        return colliderOnSpace;
    }

}
