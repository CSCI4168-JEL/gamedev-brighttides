using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
	private EntityAttributes attributesTemplate;

    public bool isMoving = false; // Track if the entity is moving
    public Tile desinationTile; // The tile that the entity is moving towards

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

    public IEnumerator MoveToTileCoroutine(Tile target) {
        while (target != null) {
            if (transform.position == target.tileTopPosition) {
                Debug.Log("Moving entitiy " + name + " complete.");
                GetComponentInParent<Tile>().LeaveTile(this); // Remove this from the previous tile
                target.SetTileAsParent(this); // After the movement is complete, update the parent of the entity and the pathability of the tile
                target = null; // Clear the reference to the tile
                yield return true;
            }
            else {
                transform.position = Vector3.MoveTowards(transform.position, target.tileTopPosition, attributes.movementSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    // Move the entity towards the top of the tile
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
