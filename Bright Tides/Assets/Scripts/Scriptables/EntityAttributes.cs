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

	public int health;
	public int ammo;
	public int gold;

	public int actionsPerTurn;
	public int actionsRemaining;
	public float movementSpeed;

	public float baseAttackRange;

	public int strength; // modifier for attack damage
	public int agility; // modifier for taking damage reduction
	public int dexterity; // modifier for successful hit
	public int wisdom; // modifier for experience gain
	public int luck; // modifier for critical damage, finding treasure, escaping combat, etc.
	


}
