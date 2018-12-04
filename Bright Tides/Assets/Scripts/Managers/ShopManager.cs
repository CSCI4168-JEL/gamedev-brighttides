using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (GameManager.instance) {
            GameManager.instance.ToggleUI(false); // Disable the UI
        } else {
            Debug.LogError("Unable to find GameManager... Are you trying to run the Shop Scene on its own?")
        }
    }

    // Adds the selected item to the player's inventory if they have enough gold
    public void AddPlayerItem(Item purchased) {
        if (purchased.price > GameManager.instance.playerInstance.GetComponent<Entity>().attributes.gold) {
            Debug.Log("Not enough gold");
            return;
        }

        GameManager.instance.playerInstance.GetComponent<Entity>().attributes.gold -= purchased.price;
        Debug.Log("Purchase successful");

        switch (purchased.itemType) {
            case ItemType.Restore:
                GameManager.instance.playerInstance.GetComponent<Entity>().attributes.ammo += purchased.ammoModifier;
                int healthMod = Math.Min(purchased.healthModifier, GameManager.instance.playerInstance.GetComponent<Entity>().attributes.maxHealth - GameManager.instance.playerInstance.GetComponent<Entity>().attributes.health);
                GameManager.instance.playerInstance.GetComponent<Entity>().attributes.health += healthMod;
                break;

            case ItemType.Range:
                GameManager.instance.playerInstance.GetComponent<Entity>().attributes.baseAttackRange += purchased.rangeModifier;
                GameManager.instance.playerInstance.GetComponent<Entity>().attributes.inventory.SetValue(purchased, 0);
                break;
            case ItemType.Damage:
                GameManager.instance.playerInstance.GetComponent<Entity>().attributes.baseAttackDamage += purchased.damageModifier;
                GameManager.instance.playerInstance.GetComponent<Entity>().attributes.inventory.SetValue(purchased, 1);
                break;
            case ItemType.Health:
                GameManager.instance.playerInstance.GetComponent<Entity>().attributes.maxHealth += purchased.maxHealthModifier;
                GameManager.instance.playerInstance.GetComponent<Entity>().attributes.inventory.SetValue(purchased, 2);
                break;
            case ItemType.Speed:
                GameManager.instance.playerInstance.GetComponent<Entity>().attributes.movementSpeed += purchased.speedModifier;
                GameManager.instance.playerInstance.GetComponent<Entity>().attributes.inventory.SetValue(purchased, 3);
                break;
        }

    }

    // Exit the shop using the current GameManager instance
    public void ExitShop() {
        GameManager.instance.ExitShop();
    }
}
