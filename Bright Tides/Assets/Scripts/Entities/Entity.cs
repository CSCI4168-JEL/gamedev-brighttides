using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
	public EntityAttributes attributes;

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
}
