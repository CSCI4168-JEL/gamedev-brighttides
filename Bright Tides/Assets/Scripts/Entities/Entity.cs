using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
	private EntityAttributes attributesTemplate;

    public bool isMoving = false; // Track if the entity is moving
    public Tile desinationTile; // The tile that the entity is moving towards

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
        }
    }

    private void Update() {
        MovementUpdate(); // Update the movement
    }

    public void Attack(Entity target) {
        Projectile.CreateProjectile(gameObject, target.gameObject); // Create a projectile heading towards the target
    }

    // Method to directly deal damage to an Entity
    public void DealDamage(int damage) {
        attributes.health -= damage;
        if (attributes.health <= 0) { // If fatal damage was dealt
            KillEntity(); // Kill self and cause any side-effects
        }
    }

    // Method to give loot from one entity to another
    public void TransferLoot(Entity looter) {
        looter.attributes.gold += attributes.gold;
        looter.attributes.gold += attributes.ammo;
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

    // Method to reset the remaining moves for an entity
    public void RefreshRemainingActions() {
        attributes.actionsRemaining = attributes.actionsPerTurn;
    }

    // Starts a movement and returns true if it was able to commence a movement
    public bool MoveToTile(Tile target) {
        if (attributes.movementSpeed > 0) {
            desinationTile = target;
            isMoving = true; // Begin the movement in the next update
        } else {
            Debug.LogWarning("Attempted to move an entity that has no movement speed!");
        }

        return isMoving;
    }

    // MoveToTile as a coroutine to allow for frame updates while moving the entity. Moves at a fixed speed
    public IEnumerator MoveToTileCoroutine(Tile target) {
        float totalDistance = Vector3.Distance(transform.position, target.tileTopPosition);

        while (target != null) {
            if (transform.position == target.tileTopPosition) {
                Debug.Log("Moving entitiy " + name + " complete.");
                GetComponentInParent<Tile>().LeaveTile(this); // Remove this from the previous tile
                target.SetTileAsParent(this); // After the movement is complete, update the parent of the entity and the pathability of the tile
                target = null; // Clear the reference to the tile
                yield return null;
            }
            else {
                transform.position = Vector3.MoveTowards(transform.position, target.tileTopPosition, totalDistance * Time.deltaTime);
                yield return waitForEndOfFrame; // Wait til frame advances, then continue
            }
        }
    }

    // Move the entity towards the top of the tile via update. Kept this to allow mousecontroller to work, but could change player movement to use the coroutine as well
    private void MovementUpdate() {
        if (isMoving && desinationTile != null) {
            if (transform.position == desinationTile.tileTopPosition) {
                Debug.Log("Moving entitiy " + name + " complete.");
                desinationTile.SetTileAsParent(this); // After the movement is complete, update the parent of the entity and the pathability of the tile
                desinationTile = null; // Clear the reference to the tile
                isMoving = false;
            }
            else {
                transform.position = Vector3.MoveTowards(transform.position, desinationTile.tileTopPosition, attributes.movementSpeed * Time.deltaTime);
            }
        }
    }
}
