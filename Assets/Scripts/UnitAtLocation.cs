using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

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

    //Takes a Vector2 location. Returns true if a player unit exists at this location, otherwise returns false.
    public static bool IsPlayerUnit(Vector2 location)
    {
        //Check if a unit exists here. If not, return false.
        if (GetCollider(location) == null)
        {
            return false;
        }
        //Get the Collider2D of the unit.
        colliderOnSpace = GetCollider(location);
        //Get the GameObject associated with that collider.
        GameObject gameOb = colliderOnSpace.gameObject;
        //Check the tag of gameOb. If it is PlayerUnit, return true. Otherwise return false.
        if (gameOb.tag == "PlayerUnit")
        {
            return true;
        }
        return false;
    }

    //Takes a Vector2 location. Returns true if a player unit exists at this location, otherwise returns false.
    public static bool IsEnemyUnit(Vector2 location)
    {
        //Check if a unit exists here. If not, return false.
        if (GetCollider(location) == null)
        {
            return false;
        }
        //Get the Collider2D of the unit.
        colliderOnSpace = GetCollider(location);
        //Get the GameObject associated with that collider.
        GameObject gameOb = colliderOnSpace.gameObject;
        //Check the tag of gameOb. If it is PlayerUnit, return true. Otherwise return false.
        if (gameOb.tag == "EnemyUnit")
        {
            return true;
        }
        return false;
    }

    //Takes a Vector2 location. Returns the Collider2D of the unit on this location.
    public static Collider2D GetCollider(Vector2 location)
    {
        //Check if there are any colliders in the unit layer within a 1-unit large box at this location.
        colliderOnSpace = Physics2D.OverlapBox(location, new Vector2(1f, 1f), 0f, 1 << 9);
        return colliderOnSpace;
    }

    //Takes a Vector2 location. Returns the PlayerUnit of the unit on this location.
    public static PlayerUnit GetPlayerUnit(Vector2 location)
    {
        //Get the Collider2D of the unit.
        colliderOnSpace = GetCollider(location);
        //Get the GameObject associated with that collider.
        GameObject gameOb = colliderOnSpace.gameObject;
        //Get the PlayerUnit script.
        PlayerUnit unit = gameOb.GetComponent<PlayerUnit>();
        //Return the PlayerUnit script.
        return unit;
    }

    //Takes a Vector2 location. Returns the PlayerUnit of the unit on this location.
    public static Unit GetUnit(Vector2 location)
    {
        //Get the Collider2D of the unit.
        colliderOnSpace = GetCollider(location);
        //Get the GameObject associated with that collider.
        GameObject gameOb = colliderOnSpace.gameObject;
        //Get the PlayerUnit script.
        Unit unit = gameOb.GetComponent<Unit>();
        //Return the PlayerUnit script.
        return unit;
    }
}