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

    public void PopulateEntities(Dictionary<EntityType, List<Tile>> entitySpawns) {
        foreach (KeyValuePair<EntityType, List<Tile>> entry in entitySpawns) {
            List<Tile> spawns = entry.Value;
            EntityType type = entry.Key;

            switch(type) {
                case EntityType.Player: // Choose one spawn randomly and place the player there
                    GameManager.instance.InstantiatePlayer(spawns[Random.Range(0, spawns.Count)]);
                    break;
                case EntityType.Enemy: // Choose a spawn until all enemies are placed (or we run out of spawns)
                    while (spawns.Count > 0 && enemyCount > 0) {
                        int index = Random.Range(0, spawns.Count);

                        Tile spawn = spawns[index]; // Get a spawn from the list using the chosen index
                        spawns.RemoveAt(index); // Remove that spawn from available spawns

                        if (enemyCount > 0) {
                            EntityAttributes selectedAttributes = entitySet.GetEntityAttributesForType(type);
                            Entity entityInstance = entitySet.CreateEntity(selectedAttributes); // Create the entity at the given tile
                            spawn.SetTileAsParent(entityInstance); // Set the tile as the parent along with any side-effects
                            enemyCount--; // Reduce the enemies left to spawn
                        }
                        else {
                            continue; // No more enemies remain to place, leave the loop
                        }
                    }
                    break;
                case EntityType.Treasure: // Choose a spawn until all treasures are placed (or we run out of spawns)
                    while (spawns.Count > 0 && treasureCount > 0) {
                        int index = Random.Range(0, spawns.Count);

                        Tile spawn = spawns[index]; // Get a spawn from the list using the chosen index
                        spawns.RemoveAt(index); // Remove that spawn from available spawns

                        if (treasureCount > 0) {
                            EntityAttributes selectedAttributes = entitySet.GetEntityAttributesForType(type);
                            Entity entityInstance = entitySet.CreateEntity(selectedAttributes); // Create the entity at the given tile
                            spawn.SetTileAsParent(entityInstance); // Set the tile as the parent along with any side-effects
                            treasureCount--; // Reduce the treasures left to spawn
                        }
                        else {
                            continue; // No more treasures remain to place, leave the loop
                        }
                    }
                    break;
            }
        }
    }
}
