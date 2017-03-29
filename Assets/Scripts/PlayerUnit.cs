using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class PlayerUnit : Unit {

    //These will store the movement cost of each terrain type for this unit.
    public int grassMove;
    public int woodsMove;
    public int mountainMove;

    public int movement;            //The movement speed of this unit.

    private Rigidbody2D rb2D;       //The Rigidbody2D of the unit.

    // Use this for initialization
    void Awake () {
        //Add the movement cost of each terrain type to the moveCosts dictonary.
        moveCosts.Add("grass", grassMove);
        moveCosts.Add("woods", woodsMove);
        moveCosts.Add("mountain", mountainMove);

        rb2D = this.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<TerrainTile> ShowMoveRange()
    {
        //rb2D = this.GetComponent<Rigidbody2D>();
        //Initialize the frontier queue to store the tiles we have yet to investigate.
        SimplePriorityQueue<TerrainTile> frontier = new SimplePriorityQueue<TerrainTile>();

        //Initialize a dictionary to store our costs for each tile.
        Dictionary<TerrainTile, int> costSoFar = new Dictionary<TerrainTile, int>();
        costSoFar.Add(TerrainAtLocation.GetTile(rb2D.position), 0);

        //Initialize a dictionary to store our route information.
        Dictionary<TerrainTile, TerrainTile> cameFrom = new Dictionary<TerrainTile, TerrainTile>();

        //Initialize a list of all the tiles we have been to.
        List<TerrainTile> inRange = new List<TerrainTile>();

        //Put the start tile (the tile the unit is currently on) in the frontier at priority 0.
        frontier.Enqueue(TerrainAtLocation.GetTile(rb2D.position), 0);

        //Continue until we are out of tiles to explore.
        while(frontier.Count != 0)
        {
            //Get the next tile in the queue.
            TerrainTile current = frontier.Dequeue();
            //Find its neighbors.
            List<TerrainTile> neighbors = TerrainAtLocation.GetNeighbors(current);
            //Add each neighbor to the frontier
            foreach(TerrainTile next in neighbors)
            {
                string tileType = next.terrainType;

                //Calculate the cost to move into the next tile.
                int newCost = costSoFar[current] + moveCosts[tileType];
                //Only need to work with tiles that are within the move range of our unit.
                if(newCost <= movement)
                {
                    //Check to make sure that the tile is not already in the list at a lower cost.
                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        //Change the value of cost in costSoFar to the new value.
                        costSoFar[next] = newCost;
                        //Add next to our frontier with a priority equal to the new cost.
                        frontier.Enqueue(next, newCost);
                        //Add a record of where we came from to cameFrom.
                        cameFrom.Add(next, current);
                        //Add the tile to inRange.
                        inRange.Add(next);
                    }
                }     
            }
        }

        return inRange;

    }

}
