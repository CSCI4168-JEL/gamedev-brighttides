using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The class responsible for populating a map with entities based on the valid spawn points and the set of enemies available for the region
// TODO: figure out some way to select the number of entities to spawn per type. does it just fill all spawn points, or does it choose some number randomly? where would 'some' come from? 
public class EntityGenerator {

    private readonly EntitySet entitySet; // The selection of entities the generate will choose from
    private int enemyCount;
    private int treasureCount;

    public EntityGenerator(EntitySet entitySet, int enemyCount, int treasureCount) {
        this.entitySet = entitySet;
        this.enemyCount = enemyCount;
        this.treasureCount = treasureCount;
    }

    public void PopulateEntities(Dictionary<EntityType, List<GameObject>> entitySpawns) {
        foreach (KeyValuePair<EntityType, List<GameObject>> entry in entitySpawns) {
            List<GameObject> spawns = entry.Value;
            EntityType type = entry.Key;

            switch(type) {
                case EntityType.Player: // Choose one spawn randomly and place the player there
                    GameManager.instance.InstantiatePlayer(spawns[Random.Range(0, spawns.Count)].transform);
                    break;
                case EntityType.Enemy: // Choose a spawn until all enemies are placed (or we run out of spawns)
                    foreach(GameObject spawn in spawns) {
                        if (enemyCount > 0) {
                            entitySet.CreateEntity(type, spawn.transform); // Create the entity at the given tile
                            enemyCount--; // Reduce the enemies left to spawn
                        } else {
                            continue; // No more enemies remain to place, leave the loop
                        }
                    }
                    break;
                case EntityType.Treasure: // Choose a spawn until all treasures are placed (or we run out of spawns)
                    foreach (GameObject spawn in spawns) {
                        if (treasureCount > 0) {
                            entitySet.CreateEntity(type, spawn.transform); // Create the entity at the given tile
                            treasureCount--; // Reduce the treasure left to spawn
                        }
                        else {
                            continue; // No more treasure remain to place, leave the loop
                        }
                    }
                    break;
            }
        }
    }
}
