using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    protected Dictionary<string, int> moveCosts = new Dictionary<string, int>(); //A dictionary containing the movement cost to enter tiles of different types.
    
    [HideInInspector]
    public Rigidbody2D rb2D;       //The Rigidbody2D of the unit.

    //These will store the movement cost of each terrain type for this unit.
    public int grassMove;
    public int woodsMove;
    public int mountainMove;

    public int movement;                     //The movement speed of this unit.
    public GameObject[] equippedWeapons;     //An array storing the equipped weapons on this unit
    public List<int> attackRange;                //The ranges at which the unit can attack

    // Use this for initialization
    void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual bool Move(Vector2 end)
    {
        if (TerrainAtLocation.IsPresent(end) && !UnitAtLocation.IsPresent(end) && TerrainAtLocation.IsHighlightedBlue(end))
        {
             rb2D = this.GetComponent<Rigidbody2D>();
             rb2D.MovePosition(end);
             return true;
        }
        return false;
            
    }

    //Given a hypothetical unit location, returns a list of all terrain tiles within attack range.
    public virtual List<TerrainTile> GetThreatenedTiles(Vector2 location)
    {
        List<TerrainTile> tilesInRange = new List<TerrainTile>();
        foreach (int rangeValue in attackRange)
        {
            tilesInRange.AddRange(TerrainAtLocation.GetTilesAtDistance(location, rangeValue));
        }
        return tilesInRange;
    }
}
