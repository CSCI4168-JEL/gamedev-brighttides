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
		Debug.LogWarning("Rest button is not implemented...");
	}
}
