using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
	private EntityAttributes attributesTemplate;

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

    public void MoveToTile(Tile target, float speed) {
        if (transform.position == target.tileTopPosition) // Move the entity towards the top of the tile
        {
            Debug.Log("Moving entitiy " + name + " complete.");
            target.SetTileAsParent(this); // After the movement is complete, update the parent of the entity and the pathability of the tile
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, target.tileTopPosition, speed * Time.deltaTime);
        }
    }

    // Completes the movement from the entity's position in a blocking loop. Temporary method to allow for AI testing
    public void MoveToTileBlocking(Tile target, float speed) {
        if (speed <= 0) {
            Debug.LogWarning("The entity's speed is 0, so it cannot move");
            return;
        }

        while (transform.position != target.tileTopPosition) {
            GetComponentInParent<Tile>().LeaveTile(this); // Get the parent tile and remove it
            transform.position = Vector3.MoveTowards(transform.position, target.tileTopPosition, speed); // Move the entity towards the top of the tile
        }

        Debug.Log("Moving entitiy " + name + " complete.");
        target.SetTileAsParent(this); // After the movement is complete, update the parent of the entity and the pathability of the tile
    }
}
