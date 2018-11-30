using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The class responsible for populating a map with entities based on the valid spawn points and the set of enemies available for the region
// TODO: figure out some way to select the number of entities to spawn per type. does it just fill all spawn points, or does it choose some number randomly? where would 'some' come from? 
public class EntityGenerator {

    private EntitySet entitySet; // The selection of entities the generate will choose from

    public EntityGenerator(EntitySet entitySet) {
        this.entitySet = entitySet;
    }

    public void PopulateEntities(Dictionary<EntityType, List<GameObject>> entitySpawns) {
        foreach (KeyValuePair<EntityType, List<GameObject>> entry in entitySpawns) {
            switch (entry.Key) {
                case EntityType.Enemy:
                    break;
                case EntityType.Player:
                    break;
            }
        }
    }
}
