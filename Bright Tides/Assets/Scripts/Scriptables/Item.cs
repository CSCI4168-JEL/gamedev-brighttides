using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bright Tides/Item", fileName = "New Item", order = 1)]
public class Item : ScriptableObject
{
    public EntityType entityType;

    public string itemName;
    public string description;
    public int price;
    public Sprite icon;

    public int rangeModifier;
    public int damageModifier;
    public int speedModifier;
    public int healthModifier;
    public int maxHealthModifier;
    public int ammoModifier;

}
