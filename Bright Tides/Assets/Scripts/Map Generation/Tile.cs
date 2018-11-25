using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour {
    public TileProperties TileProperties { get; private set; } // TileProperties class defined using auto-implemented property

    private void Awake() {
        TileProperties = new TileProperties();
    }
}

[System.Serializable]
public class TileProperties {
    public bool IsPathable { get; set; }

    // Default tile settings in the constructor
    public TileProperties() {
        IsPathable = true;
    }
}
