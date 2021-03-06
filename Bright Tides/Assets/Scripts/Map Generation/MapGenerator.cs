﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator {
    private int mapWidth;
    private int mapHeight;
    private readonly TextAsset mapDefinitionFile;
    private readonly TileSet tileSet;
    private readonly GameObject mapRoot;

    public MapGenerator(TextAsset mapDefinitionFile, TileSet tileSet, GameObject mapRoot) {
        this.mapDefinitionFile = mapDefinitionFile;
        this.tileSet = tileSet;
        this.mapRoot = mapRoot;
    }

    // Use this for initialization
    public Tile[,] Generate() {
        string mapDefinitionFileString = ParseTextFileToMapString(mapDefinitionFile); // Validate and read the text file as string
        int[,] parsedmapDefinitionFile = ParseMapStringTo2DIntArray(mapDefinitionFileString); // Parse the string into 2d int array

        Tile[,] tileMap = GenerateTileMapFrom2DIntArray(parsedmapDefinitionFile); // Generate the map
        IntializeTileNeighbours(tileMap); // Make sure each tile has a reference to its neighbours

        return tileMap;
    }

    // Add references to the Tile's neighbours
    private void IntializeTileNeighbours(Tile[,] tileMap) {
        for (int i = 0; i < mapWidth; i++) {
            for (int j = 0; j < mapHeight; j++) {
                Tile currentTile = tileMap[i, j];
                bool hasLeft = i > 0;
                bool hasRight = i < mapWidth - 1;
                bool hasDown = j > 0;
                bool hasUp = j < mapHeight - 1;

                if (hasLeft) {
                    currentTile.neighbours.Add(tileMap[i - 1, j]);
                }
                if (hasRight) {
                    currentTile.neighbours.Add(tileMap[i + 1, j]);
                }
                if (hasDown) {
                    currentTile.neighbours.Add(tileMap[i, j - 1]);
                }
                if (hasUp) {
                    currentTile.neighbours.Add(tileMap[i, j + 1]);
                }
                if (hasLeft && hasDown) {
                    currentTile.neighbours.Add(tileMap[i - 1, j - 1]);
                }
                if (hasLeft && hasUp) {
                    currentTile.neighbours.Add(tileMap[i - 1, j + 1]);
                }
                if (hasRight && hasDown) {
                    currentTile.neighbours.Add(tileMap[i + 1, j - 1]);
                }
                if (hasRight && hasUp) {
                    currentTile.neighbours.Add(tileMap[i + 1, j + 1]);
                }
            }
        }
    }

    // Return parsed text asset as a string array, but throw an exception if it does not contain the required characters
    private string ParseTextFileToMapString(TextAsset textFile) {
        string parsedFile = textFile.ToString();

        string playerSpawn = ((int)TileType.playerSpawnTile).ToString();
        string playerExit = ((int)TileType.playerExitTile).ToString();

        if (!parsedFile.Contains(playerSpawn) || !parsedFile.Contains(playerExit)) {
            throw new System.Exception("Missing player spawn and/or exit!"); // Throw exception if the file is missing a player spawn and player exit
        }

        return parsedFile;
    }


    // Return 2D int array from map string, but throw an exception if it has inconsistent dimensions
    private int[,] ParseMapStringTo2DIntArray(string mapString) {
        mapString = mapString.Replace("\r", ""); // Remove carriage returns
        string[] stringArray = mapString.Split('\n'); // Flatten map file into string array split by new line characters

        // Intiially assume the height and width for the int array
        mapHeight = stringArray.Length;
        mapWidth = stringArray[0].Length;

        int[,] intArray = new int[mapWidth, mapHeight];

        int h = 0;
        foreach (string line in stringArray) { // Go through each line (height)
            int w = 0;
            if (line.Length != mapWidth) {
                throw new System.Exception("String map dimensions must be rectangular. Current line width was " + line.Length + ", but expected width " + mapWidth); // Throw exception if the line is inconsistent width
            }

            foreach (char c in line) { // Go through each character in each line (width)
                intArray[w, h] = (int)char.GetNumericValue(c); // Get the integer value of c

                w++;
            }
            h++;
        }

        return intArray;
        
    }

    // The generator method that will produce the actual tile map from the 2D array
    private Tile[,] GenerateTileMapFrom2DIntArray(int[,] intArray) {

        Vector3 tileSize = Vector3.one;

        Tile[,] generatedTileMap = new Tile[mapWidth, mapHeight]; // Instantiate the array with the proper size

        for (int i = 0; i < mapWidth; i++) {
            for (int j = 0; j < mapHeight; j++) {
                TileType currentTileType = (TileType)intArray[i, j]; // Cast it to the enum type for easier reference

                Vector3 tilePosition = new Vector3(tileSize.x * i, mapRoot.transform.position.y, tileSize.z * -j); // Use the y value of the MapGenerator for tile height and use array position for x and z
                Tile generatedTile = tileSet.CreateTile(currentTileType, tilePosition, Quaternion.AngleAxis(0, Vector3.up), mapRoot.transform);

                generatedTileMap[i, j] = generatedTile; // Add the generated tile to the map
            }
        }

        return generatedTileMap;
    }
}
