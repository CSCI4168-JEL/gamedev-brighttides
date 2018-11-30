using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public TileProperties TileProperties { get; private set; } // TileProperties class defined using auto-implemented property

    private void Awake() {
        TileProperties = new TileProperties();
    }

    public void SetSpawnType(EntityType type) {
        TileProperties.IsSpawnPoint = true;
        TileProperties.spawnType = type;
        GameManager.instance.currentRegion.RegisterSpawnTile(type, gameObject); // When the tile is set as a spawn point, register it current region in the game manager
    }
}

[System.Serializable]
public class TileProperties {
    public bool IsPathable { get; set; }
    public bool IsSpawnPoint { get; set; }
    public EntityType spawnType;

    // Default tile settings in the constructor
    public TileProperties() {
        IsPathable = true;
        IsSpawnPoint = false;
    }


}