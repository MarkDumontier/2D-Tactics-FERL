using System.Collections;
using System;                       //Allows us to use Serializable.
using System.Collections.Generic;   //Allows us to use lists.
using UnityEngine;
using Random = UnityEngine.Random;  //Tells Random to use the Unity Engine random number generator.

public class BoardManager : MonoBehaviour {

    public int columns = 8;                                     //Number of columns in our game board.
    public int rows = 8;                                        //Number of rows in our game board.
    public GameObject[] grassTiles;                             //Array of grass prefabs.
    public GameObject[] woodsTiles;                             //Array of woods prefabs.
    public GameObject cursor;                                   //Cursor
    public Vector3 cursorStartPosition;                         //Cursor Start Position

    public GameObject playerUnit;                               //Player unit (used for testing)
    public Vector3 playerStartPosition;                         //Player unit start position (used for testing)

    private Transform boardHolder;                              //A variable to store a reference to the transform of our Board object.
    private List<Vector3> gridPositions = new List<Vector3> (); //A list of possible locations to place tiles.

    //Clears our list gridPositions and prepares it to generate a new board.
    void InitialiseList()
    {
        //Clear our list gridPositions.
        gridPositions.Clear();

        //Loop through x axis (columns).
        for(int x = 0; x < columns; x++)
        {
            //Within each column, loop through y axis (rows).
            for(int y = 0; y < rows; y++)
            {
                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    //Sets up the floor of the game board.
    void BoardSetup()
    {
        //Instantiate Board and set boardHolder to its transform.
        boardHolder = new GameObject ("Board").transform;

        //Create a list of yet to be filled spaces.
        InitialiseList();

        //Add a forest at (2,1).
        Vector3 woodsPosition = new Vector3(2, 1, 0f);
        GameObject woods = Instantiate(woodsTiles[0], woodsPosition, Quaternion.identity) as GameObject;
        woods.transform.SetParent(boardHolder);
        gridPositions.Remove(woodsPosition);

        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for (int x = 0; x < columns; x++)
        {
            //Loop along y axis, starting from -1 to place floor or outerwall tiles.
            for (int y = 0; y < rows; y++)
            {
                //Check to make sure we don't already have a tile there.
                if(gridPositions.Contains(new Vector3(x, y, 0f)))
                {
                    //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                    GameObject toInstantiate = grassTiles[Random.Range(0, grassTiles.Length)];

                    //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                    //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                    instance.transform.SetParent(boardHolder);

                    //Remove this position from our list of empty positions.
                    gridPositions.Remove(new Vector3(x, y, 0f));
                }
               
            }
        }

        //Instantiate cursor at (0, 0). Set only FreeCursorMove to be active at start
        GameObject cursorInstance = Instantiate(cursor, cursorStartPosition, Quaternion.identity) as GameObject;

        //Disable all cursor modes besides free
        CursorManager cursorManager = cursor.GetComponent<CursorManager>();
        cursorManager.SetMode("free");

        //Instantiate a playerUnit at (1, 1)
        GameObject playerUnitInstance = Instantiate(playerUnit, playerStartPosition, Quaternion.identity) as GameObject;

        //Set the parent of cursor and player to boardHolder to avoid cluttering hierarchy.
        cursorInstance.transform.SetParent(boardHolder);
        playerUnitInstance.transform.SetParent(boardHolder);
    }

    //SetupScene initializes our level and calls the previous functions to lay out the game board
    public void SetupScene()
    {
        //Creates the outer walls and floor.
        BoardSetup();

        //Reset our list of gridpositions.
        InitialiseList();
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
