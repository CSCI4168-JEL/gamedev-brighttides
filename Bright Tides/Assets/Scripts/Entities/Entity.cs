using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
	private EntityAttributes attributesTemplate;

    // Coroutine helpers
    private readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame(); // To prevent unecessary memory allocation

    public EntityAttributes AttributesTemplate
	{
		get { return this.attributesTemplate; }
		set
		{
			attributesTemplate = value;
			if (attributesTemplate != null)
			{
				attributes = ScriptableObject.Instantiate(attributesTemplate);
			}
		}
	}

	public EntityAttributes attributes;

    private void Awake() {
        Debug.Log("Setting attributes for " + gameObject.name);
        if (attributesTemplate != null) {
            attributes = ScriptableObject.Instantiate(attributesTemplate);
            attributes.Initialize();
        }
    }

    // Method to change the max health and update current health
    public void ModifyMaxHealth(int maxHealthChange) {
        attributes.maxHealth += maxHealthChange;

        if (attributes.maxHealth > attributes.health) { // If max health increased past current health
            attributes.health += maxHealthChange; // Modify the current health by the same amount
        }

        UpdateHealth(); // Ensure health is updated
    }

    // Method to directly deal damage to an Entity
    public void DealDamage(int damage) {
        attributes.health -= damage;
        Debug.Log(name + " just took " + damage + " damage!");
        UpdateHealth(); // Ensure health is updated
    }

    // Handle this entity entering the same tile as another entity
    public void Collide (Entity other) {
        if (this.attributes.entityType == EntityType.Player && other.attributes.entityType == EntityType.Treasure) {
            other.KillEntity(); // Destory the entity and give the loot to the player
        }
    }

    // Method to reset the remaining moves for an entity
    public void RefreshRemainingActions() {
        attributes.actionsRemaining = attributes.actionsPerTurn;
    }

    // Method to give loot from one entity to another
    private void TransferLoot(Entity looter) {
        GameManager.AddFloatingText(GameManager.instance.playerInstance.transform.position, new Vector3(0.3f, 0.4f, 0), "+" + attributes.gold + " gold!", "TMP_Positive");
        looter.attributes.gold += attributes.gold;
        looter.attributes.ammo += attributes.ammo;
        GameManager.AddFloatingText(GameManager.instance.playerInstance.transform.position, new Vector3(0.0f, 0.4f, 0.3f), "+" + attributes.ammo + " ammo!", "TMP_Positive");
        attributes.gold = 0;
        attributes.ammo = 0;
    }

    // Destory this entity and if it is not a player, give its loot to the player
    private void KillEntity() {
        Tile parentTile = GetComponentInParent<Tile>();
        if (parentTile) { // If the entity is on a tile (it probably should be)
            parentTile.LeaveTile(this); // Remove the entity from the tile and cause any side-effects
            if (attributes.entityType != EntityType.Player) {
                GameObject playerObject = GameManager.instance.playerInstance;
                if (playerObject) { // Check if the player instance is defined
                    Entity player = playerObject.GetComponent<Entity>();
                    TransferLoot(player); // Give the loot of this entity to the player
                }
            }
        }
        Destroy(gameObject); // If GameObject is a goner, Entity is a goner
    }

    // Ensure that health follows game rules. Future consideration is move this into the setter for health (which would require entity attributes to have access to the entity)
    private void UpdateHealth() {
        if (attributes.health > attributes.maxHealth) {
            attributes.health = attributes.maxHealth; // Cap the entity health
        }

        if (attributes.health <= 0) { // If fatal damage was dealt
            KillEntity(); // Kill self and cause any side-effects
        }
    }

    /** Entity Actions **/

    // MoveToTile as a coroutine to allow for frame updates while moving the entity. Moves at a fixed speed
    public IEnumerator MoveToTile(Tile target) {
        float totalDistance = Vector3.Distance(transform.position, target.tileTopPosition);

        while (target != null) {
            if (transform.position == target.tileTopPosition) {
                Debug.Log("Moving entitiy " + name + " complete.");
                GetComponentInParent<Tile>().LeaveTile(this); // Remove this from the previous tile
                target.EnterTile(this); // After the movement is complete, update the parent of the entity and the pathability of the tile
                target = null; // Clear the reference to the tile
                yield return null;  // Exit the coroutine
            }
            else {
                transform.position = Vector3.MoveTowards(transform.position, target.tileTopPosition, totalDistance * Time.deltaTime);
                yield return waitForEndOfFrame; // Wait until frame advances, then continue
            }
        }
    }

    public IEnumerator Attack(Entity target) {
        Projectile projectile = Projectile.CreateProjectile(this, target); // Create a projectile heading towards the target
        while (projectile != null) {
            yield return waitForEndOfFrame; // Allow the projectile to live out its lifetime
        }
    }
}
