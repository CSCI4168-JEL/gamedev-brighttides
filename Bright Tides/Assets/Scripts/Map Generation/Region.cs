using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapGenerator))]
[RequireComponent(typeof(EntityGenerator))]
public class Region : MonoBehaviour {

    public GameObject StartingPosition { get; private set; }

    private MapGenerator mapGenerator;
    private EntityGenerator entityGenerator;
    private TileSet tileSet;
    private EntitySet entitySet;
    private TextAsset mapDefinitionFile;
    private Dictionary<EntityType, List<GameObject>> entitySpawns;

    public void Start() {
        // Get the scene information from the GameManager
        SceneState scene = GameManager.instance.scene;
        tileSet = scene.tileSet;
        entitySet = scene.entitySet;
        mapDefinitionFile = scene.mapDefinitionFile;


        // Create instances of the required region building classes
        mapGenerator = new MapGenerator(mapDefinitionFile, tileSet);
        entityGenerator = new EntityGenerator(entitySet);

        entitySpawns = new Dictionary<EntityType, List<GameObject>>();

    }

    // Call this to generate the map and populate it
    public void Initialize() {
        mapGenerator.Generate();
        entityGenerator.PopulateEntities(entitySpawns);
    }

    public void RegisterSpawnTile(EntityType spawnType, GameObject tile) {
        GetSpawnListByType(spawnType).Add(tile); // Get the list by type and add the spawn tile to it
    }

    private List<GameObject> GetSpawnListByType(EntityType spawnType) {
        List<GameObject> spawnList = entitySpawns[spawnType];

        if (spawnList == null) {
            spawnList = new List<GameObject>();
            entitySpawns[spawnType] = spawnList;
        }

        return entitySpawns[spawnType];
    }
}
