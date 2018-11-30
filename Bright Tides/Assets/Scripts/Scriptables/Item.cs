using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bright Tides/Item", fileName = "New Item", order = 3)]
public class Item : ScriptableObject {
	public Sprite icon;
    public int price;
    public string itemName;

    public StatIncrease stats;
    public int minLevel;
}

[System.Serializable]
public class StatIncrease {
    public int attack;
    public int health;
    public int moveDistance;
}
