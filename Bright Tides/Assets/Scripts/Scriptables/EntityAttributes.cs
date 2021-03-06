﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bright Tides/Entity Attributes", fileName = "Entity Attributes", order = 1)]
public class EntityAttributes : ScriptableObject {
	public EntityType entityType;

    public GameObject model;
		
	public string captainName;

    public int maxHealth;
    [HideInInspector]
    public int health;
	public int ammo;
	public int gold;

    public Item[] inventory;

	public int actionsPerTurn;
    [HideInInspector]
	public int actionsRemaining;
	public float movementSpeed;

	public float baseAttackRange;
	public int baseAttackDamage;
	
	public int constitution; // modifier for health regeneration
	
    public bool isPathableByPlayer; // If the entity occupies a tile, can it be pathed through by the player
    public bool isPathableByEnemy; // If the entity occupies a tile, can it be pathed through by the player

    // Intialize the dependent values
    public void Initialize() {
        health = maxHealth;
        actionsRemaining = actionsPerTurn;
    }
}
