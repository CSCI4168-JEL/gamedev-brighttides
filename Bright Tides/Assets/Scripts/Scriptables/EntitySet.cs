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

    public EntityAttributes GetEntityAttributesForType(EntityType type) {
        switch (type) {
            case EntityType.Enemy:
                if (enemyEntities.Length > 0) {
                    return enemyEntities[Random.Range(0, enemyEntities.Length)]; // Choose randomly from all provided enemy attributes
                }
                break;
            case EntityType.Treasure:
                if (treasureEntities.Length > 0) {
                    return treasureEntities[Random.Range(0, treasureEntities.Length)]; // Choose randomly from all provided treasure attributes
                }
                break;
        }

        return null; // If nothing selected in the create swtich, return null
    }

    public Entity CreateEntity(EntityAttributes entityAttributes) {
        GameObject entityInstance = Instantiate(entityAttributes.model); // Create an entity using the selected model
        Entity entityComponent = entityInstance.GetComponent<Entity>();
        if (!entityComponent) {
            entityComponent = entityInstance.AddComponent(typeof(Entity)) as Entity; // Create the entity instance component if it does not already exist on the prefab
        }
        entityComponent.attributes = entityAttributes; // Apply the attributes to the entity
        return entityComponent;
    }
}
