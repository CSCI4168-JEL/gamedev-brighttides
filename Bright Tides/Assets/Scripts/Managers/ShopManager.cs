using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (GameManager.instance) {
			GameManager.instance.ToggleActionsBarUI(false);
        } else {
            Debug.LogError("Unable to find GameManager... Are you trying to run the Shop Scene on its own?");
        }
    }

    // Adds the selected item to the player's inventory if they have enough gold
    public void AddPlayerItem(Item purchased) {
        if (purchased.price > GameManager.instance.playerInstance.GetComponent<Entity>().attributes.gold) {
            Debug.Log("Not enough gold");
            return;
        }

        Entity playerEntity = GameManager.instance.playerInstance.GetComponent<Entity>();

        playerEntity.attributes.gold -= purchased.price;
        Debug.Log("Purchase successful");

        switch (purchased.itemType) {
            case ItemType.Restore:
                playerEntity.attributes.ammo += purchased.ammoModifier;
                int healthMod = Math.Min(purchased.healthModifier, playerEntity.attributes.maxHealth - playerEntity.attributes.health);
                playerEntity.attributes.health += healthMod;
                break;

            case ItemType.Range:
                playerEntity.attributes.baseAttackRange += purchased.rangeModifier;
                playerEntity.attributes.inventory.SetValue(purchased, 0);
                break;

            case ItemType.Damage:
                playerEntity.attributes.baseAttackDamage += purchased.damageModifier;
                playerEntity.attributes.inventory.SetValue(purchased, 1);
                break;

            case ItemType.Health:
                playerEntity.ModifyMaxHealth(purchased.maxHealthModifier); // Modify the max health using the correct method
                playerEntity.attributes.inventory.SetValue(purchased, 2);
                break;
        }

    }

    // Exit the shop using the current GameManager instance
    public void ExitShop() {
        GameManager.instance.ExitShop();
    }
}
