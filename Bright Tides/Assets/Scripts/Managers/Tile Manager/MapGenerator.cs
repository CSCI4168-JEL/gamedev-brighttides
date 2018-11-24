using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    [Header("Map File")]
    [Tooltip("The text file to read for a map. Must have valid dimensions.")]
    public TextAsset mapFile;

    private GameObject[,] tileMap; // Array of GameObjects (the map)
    private int mapWidth;
    private int mapHeight;

    // Use this for initialization
    void Start () {
        string mapFileString = ParseTextFileToMapString(mapFile); // Validate and read the text file as string
        int[,] parsedMapFile = ParseMapStringTo2DIntArray(mapFileString); // Parse the string into 2d int array

        tileMap = GenerateTileMapFrom2DIntArray(parsedMapFile); // Generate the map and keep a reference
    }

    // Return parsed text asset as a string array, but throw an exception if it does not contain the required characters
    private string ParseTextFileToMapString(TextAsset textFile) {
        string parsedFile = textFile.ToString();

        string playerSpawn = ((int)TileManager.TileTypes.playerSpawnTile).ToString();
        string playerExit = ((int)TileManager.TileTypes.playerExitTile).ToString();

        if (!parsedFile.Contains(playerSpawn) || !parsedFile.Contains(playerExit)) {
            throw new System.Exception("Missing player spawn and/or exit!"); // Throw exception if the file is missing a player spawn and player exit
        }

        return parsedFile;
    }


    // Return 2D int array from map string, but throw an exception if it has inconsistent dimensions
    private int[,] ParseMapStringTo2DIntArray(string mapString) {

        string fileString = mapFile.ToString(); // Get the TextFile contents
        fileString = fileString.Replace("\r", ""); // Remove carriage returns
        string[] stringArray = fileString.Split('\n'); // Flatten map file into string array split by new line characters

        // Intiially assume the height and width for the int array
        mapHeight = stringArray.Length;
        mapWidth = stringArray[0].Length;

        int[,] intArray = new int[mapWidth, mapHeight];

        int i = 0;
        foreach (string line in stringArray) {
            int j = 0;
            if (line.Length != mapWidth) {
                throw new System.Exception("String map dimensions must be rectangular. Current line width was " + line.Length + ", but expected width " + mapWidth); // Throw exception if the line is inconsistent width
            }

            foreach (char c in line) {
                intArray[i, j] = (int)char.GetNumericValue(c); // Get the integer value of c

                j++;
            }
            i++;
        }

        return intArray;
    }

    // The generator method that will produce the actual tile map from the 2D array
    private GameObject[,] GenerateTileMapFrom2DIntArray(int[,] intArray) {
        Vector3 tileSize = TileManager.singleton.tileSize; // The size of the tiles' bounds for uniform tile placement

        GameObject[,] generatedTileMap = new GameObject[mapWidth, mapHeight]; // Instantiate the array with the proper size

        for (int i = 0; i < mapWidth; i++) {
            for (int j = 0; j < mapHeight; j++) {
                Vector3 tilePosition = new Vector3(tileSize.x * i, transform.position.y, tileSize.z * j); // Use the y value of the MapGenerator for tile height

                TileManager.TileTypes currentTile = (TileManager.TileTypes)intArray[i, j]; // Cast it to the enum type for easier reference
                GameObject generatedTile;

                switch(currentTile) {
                    case TileManager.TileTypes.waterTile:
                        generatedTile = TileManager.singleton.CreateWaterTile(tilePosition, Quaternion.AngleAxis(0, Vector3.up), transform);
                        break;
                    case TileManager.TileTypes.obstacleTile:
                        generatedTile =TileManager.singleton.CreateIslandTile(tilePosition, Quaternion.AngleAxis(0, Vector3.up), transform);
                        break;
                    default:
                        Debug.LogWarning("Ignored missing case during map generation, defaulting to water tile");
                        generatedTile = TileManager.singleton.CreateWaterTile(tilePosition, Quaternion.AngleAxis(0, Vector3.up), transform); // Temporarily create water tile until all types are implemented
                        break;
                }

                generatedTile.tag = "MapTile"; // Set the tag for the generated tile
            }
        }
        return generatedTileMap;
    }
}
