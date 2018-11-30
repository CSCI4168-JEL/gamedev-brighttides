using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType : int {
    Player,
    Enemy,
    Treasure
}

[CreateAssetMenuAttribute(menuName = "Bright Tides/Entity Set", fileName = "New EntitySet", order = 2)]
public class EntitySet : ScriptableObject {

    [Header("Enemy Entity List")]
    [Tooltip("The list of all enemy entities for the region.")]
    public EntityAttributes[] enemyEntities = new EntityAttributes[0];

    [Header("Treasure Entity List")]
    [Tooltip("The list of all treasure entities for the region.")]
    public EntityAttributes[] treasureEntities = new EntityAttributes[0];

    public GameObject CreateEntity(EntityType type, Transform parent) {
        switch (type) {
            case EntityType.Enemy:
                if (enemyEntities.Length <= 0) {
                    break;
                }
                return CreateEnemyEntity(parent);
            case EntityType.Treasure:
                if (treasureEntities.Length <= 0) {
                    break;
                }
                return CreateTreasureEntity(parent);
        }

        return null; // If nothing selected in the create swtich, return null
    }

    // Type-specific creation methods. Separate to allow differences in how an entity is chosen from the set
    private GameObject CreateEnemyEntity (Transform parent) {
        EntityAttributes selectedAttributes = enemyEntities[Random.Range(0, enemyEntities.Length)]; // Choose randomly from all provided enemy attributes
        GameObject entityInstance = Instantiate(selectedAttributes.model, parent); // Create an enemy using the selected model
        entityInstance.transform.position += new Vector3(0, 0.52f, 0); // To place the enemy above the water, not inside
        Entity entityComponent = entityInstance.AddComponent(typeof(Entity)) as Entity;
        entityComponent.attributes = selectedAttributes; // Apply the attributes to the entity

        return entityInstance;
    }

    private GameObject CreateTreasureEntity(Transform parent) {
        EntityAttributes selectedAttributes = treasureEntities[Random.Range(0, treasureEntities.Length)]; // Choose randomly from all provided treasure attributes
        GameObject entityInstance = Instantiate(selectedAttributes.model, parent); // Create an treasure using the selected model
        entityInstance.transform.position += new Vector3(0, 0.52f, 0); // To place the treasure above the water, not inside
        Entity entityComponent = entityInstance.AddComponent(typeof(Entity)) as Entity;
        entityComponent.attributes = selectedAttributes; // Apply the attributes to the entity

        return entityInstance;
    }
}
