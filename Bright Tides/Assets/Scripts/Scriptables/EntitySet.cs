using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType : int {
    Player,
    Enemy,
    Treasure
}

// TODO: should this be set up similar to tile set? enemies should be populated from some rules set per region

[CreateAssetMenuAttribute(menuName = "Bright Tides/Entity Set", fileName = "New EntitySet", order = 2)]
public class EntitySet : ScriptableObject {

    [Header("Player Model")]
    [Tooltip("The player prefab.")]
    public GameObject playerEntity;

    [Header("Enemy Entity List")]
    [Tooltip("The list of all enemy prefabs. Must contain at least one element")]
    public GameObject[] enemyPrefabs = new GameObject[1]; // Must contain at least one obstacle tile definition

    [Header("Treasure Entity List")]
    [Tooltip("The list of all treasure prefabs. Must contain at least one element")]
    public GameObject[] treasurePrefabs = new GameObject[1]; // Must contain at least one obstacle tile definition

    public GameObject CreateEntity(EntityType type, Transform parent) {
        switch (type) {
            case EntityType.Player:
                return CreatePlayerEntity(parent);
            case EntityType.Enemy:
                return CreateEnemyEntity(parent);
            case EntityType.Treasure:
                return CreateTreasureEntity(parent);
            default:
                return null;
        }
    }


    GameObject CreatePlayerEntity(Transform parent) {
        GameObject entity = Instantiate(playerEntity, parent);
        return entity;
    }

    GameObject CreateEnemyEntity (Transform parent) {
        GameObject selectedPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]; // Choose randomly from all provided enemy prefabs
        GameObject entity = Instantiate(selectedPrefab, parent);
        return entity;
    }

    GameObject CreateTreasureEntity(Transform parent) {
        GameObject selectedPrefab = treasurePrefabs[Random.Range(0, enemyPrefabs.Length)]; // Choose randomly from all provided treasure prefabs
        GameObject entity = Instantiate(selectedPrefab, parent);
        return entity;
    }
}
