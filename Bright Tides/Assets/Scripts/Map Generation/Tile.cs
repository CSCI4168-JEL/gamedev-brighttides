using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public TileProperties TileProperties { get; private set; } // TileProperties class defined using auto-implemented property

    public Vector3 tileTopPosition; // The position where entities can spawn

    private void Awake() {
        TileProperties = new TileProperties();
        tileTopPosition = transform.position + new Vector3(0f, 0.52f, 0f);
    }

    public void SetSpawnType(EntityType type) {
        TileProperties.IsSpawnPoint = true;
        TileProperties.spawnType = type;
        GameManager.instance.currentRegion.RegisterSpawnTile(type, this); // When the tile is set as a spawn point, register it current region in the game manager
    }

    public void SetTileAsParent(Entity entity) {
        entity.transform.SetParent(transform); // Set the tile transform as the parent of the entity
        entity.transform.position = tileTopPosition; // Move the entity to the top of the tile
        TileProperties.IsPathableByPlayer = entity.attributes.isPathableByPlayer; // Update if the tile is pathable by the player
        TileProperties.IsPathableByEnemy = entity.attributes.isPathableByEnemy; // Update if the tile is pathable by an enemy
    }
}

[System.Serializable]
public class TileProperties {
    public bool IsPathableByPlayer { get; set; }
    public bool IsPathableByEnemy { get; set; }
    public bool IsSpawnPoint { get; set; }
    public EntityType spawnType;
	public TileType tileType;

    // Default tile settings in the constructor
    public TileProperties() {
        IsPathableByPlayer = true;
        IsPathableByEnemy = true;
        IsSpawnPoint = false;
    }


}