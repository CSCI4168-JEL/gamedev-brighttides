using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum to store all tile types for map generation
public enum TileType : int {
    playerSpawnTile,    // 0
    playerExitTile,     // 1
    waterTile,          // 2
    obstacleTile,       // 3
    enemySpawnTile,     // 4
    treasureSpawnTile   // 5
}

[CreateAssetMenuAttribute(menuName = "Bright Tides/Tile Set", fileName = "New TileSet", order = 2)]
public class TileSet : ScriptableObject {

    [Header("Water Tile")]
    [Tooltip("The basic tile that will appear for any pathable location on the grid.")]
    public GameObject waterTile;

    [Header("Player Exit Tile")]
    [Tooltip("The tile the represents the level exit")]
    public GameObject playerExitTile;

    [Header("Obstacle Tile List")]
    [Tooltip("The list of all non-pathable tile prefabs. Must contain at least one element")]
    public GameObject[] obstacleTiles = new GameObject[1]; // Must contain at least one obstacle tile definition

    public GameObject CreateTile(TileType type, Vector3 position, Quaternion rotation, Transform parent) {
        switch (type) {
            case TileType.waterTile:
                return CreateWaterTile(position, rotation, parent);

            case TileType.enemySpawnTile:
                return CreateEnemySpawnTile(position, rotation, parent);

            case TileType.treasureSpawnTile:
                return CreateTreasureSpawnTile(position, rotation, parent);

            case TileType.obstacleTile:
                return CreateObstacleTile(position, rotation, parent);

            case TileType.playerSpawnTile:
                return CreatePlayerSpawnTile(position, rotation, parent);

            case TileType.playerExitTile:
                return CreatePlayerExitTile(position, rotation, parent);

            default:
                Debug.LogError("No valid TileType provided, defaulting to waterTile");
                return CreateWaterTile(position, rotation, parent);
        }
    }

    /* 
     * Factory methods to generate the type of tile using the tile prefabs that are set in the editor
     * TODO: Make these distinctly choose the different properties/behaviors
     */

    GameObject CreatePlayerSpawnTile(Vector3 position, Quaternion rotation, Transform parent) {
        GameObject tile = Instantiate(waterTile, position, rotation, parent);
        tile.GetComponent<Tile>().TileProperties.IsPathableByPlayer = true;
        tile.GetComponent<Tile>().SetSpawnType(EntityType.Player); // Set the spawn type to player
        return tile;
    }

    GameObject CreatePlayerExitTile(Vector3 position, Quaternion rotation, Transform parent) {
        GameObject tile = Instantiate(playerExitTile, position, rotation, parent);
        tile.GetComponent<Tile>().TileProperties.IsPathableByPlayer = true;
        return tile;
    }

    GameObject CreateWaterTile(Vector3 position, Quaternion rotation, Transform parent) {
        GameObject tile = Instantiate(waterTile, position, rotation, parent);
        Tile tileComponent = tile.GetComponent<Tile>();
        tileComponent.TileProperties.IsPathableByPlayer = true;
        return tile;
    }

    GameObject CreateObstacleTile(Vector3 position, Quaternion rotation, Transform parent) {
        GameObject selectedPrefab = obstacleTiles[Random.Range(0, obstacleTiles.Length)]; // Choose randomly from all provided obstacle tile prefabs
        GameObject tile = Instantiate(selectedPrefab, position, rotation, parent);
        tile.GetComponent<Tile>().TileProperties.IsPathableByPlayer = false;
        return tile;
    }

    GameObject CreateEnemySpawnTile(Vector3 position, Quaternion rotation, Transform parent) {
        GameObject tile = Instantiate(waterTile, position, rotation, parent);
        tile.GetComponent<Tile>().TileProperties.IsPathableByPlayer = true;
        tile.GetComponent<Tile>().SetSpawnType(EntityType.Enemy); // Set the spawn type to enemy
        return tile;
    }

    GameObject CreateTreasureSpawnTile(Vector3 position, Quaternion rotation, Transform parent) {
        GameObject tile = Instantiate(waterTile, position, rotation, parent);
        tile.GetComponent<Tile>().TileProperties.IsPathableByPlayer = true;
        tile.GetComponent<Tile>().SetSpawnType(EntityType.Treasure); // Set the spawn type to treasure
        return tile;
    }
}
