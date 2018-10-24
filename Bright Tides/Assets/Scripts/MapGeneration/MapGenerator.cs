using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    [Header("Tile Prefabs")]

    [Tooltip("The basic map tile for generation")]
    public GameObject basicTile;


    [Header("Map Parameters")]

    [Tooltip("The maximum number of tiles in the x direction for the map")]
    public int mapWidth;

    [Tooltip("The maximum number of tiles in the z direction for the map")]
    public int mapHeight;


    private GameObject[,] map; // Array of GameObjects (the map)
    private Vector3 tileSize; // The size of the tiles' bounds for uniform tile placement

    private void Awake() {
        if (mapWidth <= 0) {
            throw new System.Exception("mapHeight cannot be 0 or less");
        }

        if (mapHeight <= 0) {
            throw new System.Exception("mapWidth cannot be 0 or less");
        }

        map = new GameObject[mapWidth, mapHeight]; // Instantiate the array with the proper size
        GameObject tile = Instantiate(basicTile, Vector3.zero, Quaternion.AngleAxis(0, Vector3.up)); // Generate temporary tile
        tileSize = tile.GetComponent<Renderer>().bounds.size;
        GameObject.Destroy(tile);  // Get rid of temporary tile
    }

    // Use this for initialization
    void Start () {

        for (int i = 0; i < mapWidth; i++) {
            for (int j = 0; j < mapHeight; j++) {
                Vector3 tilePosition = new Vector3(tileSize.x * i, transform.position.y, tileSize.z * j); // Use the y value of the MapGenerator for tile height
                GameObject currentTile = Instantiate(basicTile, tilePosition, Quaternion.AngleAxis(0, Vector3.up));
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
