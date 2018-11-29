using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EntityType
{
    Player,
    Enemy,
	Interactable

}

[CreateAssetMenu(menuName = "Bright Tides/Entity Attributes", fileName = "Entity Attributes", order = 1)]
public class EntityAttributes : ScriptableObject {
    public EntityType entityType;

    public string name;

    public int level;
    public int experience;

    public float health;

    public int movesPerTurn;
    public float movementSpeed;

    public float baseAttackRange;

    public float strength; // modifier for attack damage
    public float agility; // modifier for taking damage reduction
    public float dexterity; // modifier for successful hit
    public float wisdom; // modifier for experience gain
    public float luck; // modifier for critical damage, finding treasure, escaping combat, etc.
    


}
