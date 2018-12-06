using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionsBarUIHandler : MonoBehaviour {
	// Use this for initialization
	void Awake () {
		
	}
	
	public void OnEndTurnButtonClick()
	{
		GameManager.instance.EndPlayerTurn();
	}

	public void OnMoveButtonClick()
	{

	}

	public void OnAttackButtonClick()
	{

	}

	public void OnExploreButtonClick()
	{

	}

	public void OnInventoryButtonClick()
	{
		Debug.LogWarning("Inventory button is not implemented...");
	}

	public void OnRestButtonClick()
	{
		Entity playerEntity = GameManager.instance.playerInstance.GetComponent<Entity>();

		if (playerEntity.attributes.health == playerEntity.attributes.maxHealth)
		{
			Debug.LogWarning("Player at max health");
		}

		if (playerEntity.attributes.actionsRemaining > 0)
		{
			int healAmount = playerEntity.Heal();
			GameManager.AddFloatingText(GameManager.instance.playerInstance.transform.position, new Vector3(0, 0.4f, 0), "+" + healAmount + " HP!", "TMP_Positive");

			playerEntity.attributes.actionsRemaining--;
		}
	}
}
