using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public static TileManager singleton; // The singleton instance

    // Enum to store all tile types for map generation
    public enum TileTypes : int {
        playerSpawnTile,
        playerExitTile,
        waterTile,
        obstacleTile,
        enemySpawnTile,
        treasureTile
    }

    [HideInInspector]
    public Vector3 tileSize;

    // Editor parameters, visible in inspector

    [Header("Required Prefab References")]
    [Tooltip("Water Tile")]
    public GameObject waterTile;

    [Tooltip("Obstacle Tiles")]
    public List<GameObject> obstacleTiles;

    void Awake() {
        bool execute = SetSingleton();

        if (!execute) {
            return;
        }

        tileSize = GetTileSize(); // Initialize the tile size
    }

    bool SetSingleton() {
        // Ensure no other instance exists
        if (singleton == null) {
            singleton = this;
            DontDestroyOnLoad(gameObject);
            return true;
        }
        else {
            Destroy(gameObject);
            return false;
        }
    }


    // Generate a temporary tile to get runtime size for uniform placement
    private Vector3 GetTileSize() {
        GameObject tile = CreateWaterTile(Vector3.zero, Quaternion.AngleAxis(0, Vector3.up), transform); // Generate temporary tile
        Vector3 tileSize = tile.GetComponent<Renderer>().bounds.size;
        GameObject.Destroy(tile);  // Get rid of temporary tile

        return tileSize;
    }

    // Factory methods

    // Water tile
    public GameObject CreateWaterTile(Vector3 position, Quaternion rotation, Transform parent) {
        return Instantiate(waterTile, position, Quaternion.AngleAxis(0, Vector3.up), parent);
    }

    // Obstacle Tile
    public GameObject CreateIslandTile(Vector3 position, Quaternion rotation, Transform parent) {
        GameObject selectedPrefab = obstacleTiles[Random.Range(0, obstacleTiles.Count - 1)];
        return Instantiate(selectedPrefab, position, Quaternion.AngleAxis(0, Vector3.up), parent);
    }
}