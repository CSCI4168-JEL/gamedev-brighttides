using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour {
    private MapGenerator mapGenerator;
    private EntityGenerator entityGenerator;
    private TileSet tileSet;
    private EntitySet entitySet;
    private TextAsset mapDefinitionFile;
    private Dictionary<EntityType, List<Tile>> entitySpawns;
    private int enemyPopulation; // Number of enemies to spawn in the region
    private int treasurePopulation; // Number of treaures to spawn in the region

    public void Awake() {
        // Get the scene information from the GameManager
        SceneState scene = GameManager.instance.sceneState;
        tileSet = scene.tileSet;
        entitySet = scene.entitySet;
        mapDefinitionFile = scene.mapDefinitionFile;
        enemyPopulation = scene.enemyPopulation;
        treasurePopulation = scene.treasurePopulation;

        // Create instances of the required region building classes
        mapGenerator = new MapGenerator(mapDefinitionFile, tileSet, gameObject);
        entityGenerator = new EntityGenerator(entitySet, enemyPopulation, treasurePopulation);

        entitySpawns = new Dictionary<EntityType, List<Tile>>();
    }

    // Call this to generate the map and populate it
    public void Initialize() {
        mapGenerator.Generate();
        entityGenerator.PopulateEntities(entitySpawns); // Spawn the player, enemies and treasure
        CameraController camera = GameObject.FindGameObjectWithTag("MainCamera").AddComponent<CameraController>(); // Attach the camera script after the player has been initialized
        camera.followPosition = new Vector3(-1f, 2f, 0.75f);
        camera.objectBeingFollowed = GameManager.instance.playerInstance;
    }

    public void RegisterSpawnTile(EntityType spawnType, Tile tile) {
        GetSpawnListByType(spawnType).Add(tile); // Get the list by type and add the spawn tile to it
    }

    // Add a spawnpoint to a list 
    private List<Tile> GetSpawnListByType(EntityType spawnType) {
        if (!entitySpawns.ContainsKey(spawnType)) {
            List<Tile> spawnList = new List<Tile>();
            entitySpawns.Add(spawnType, spawnList);
        }

        return entitySpawns[spawnType];
    }
}
