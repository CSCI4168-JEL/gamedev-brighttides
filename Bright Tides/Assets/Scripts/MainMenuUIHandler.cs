using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIHandler : MonoBehaviour {
	// Use this for initialization
	void Awake () {
		
	}
	
	public void OnStartGameButtonClick()
	{
		GameManager.instance.StartGame();
	}

	public void OnQuitButtonClick()
	{
		GameManager.instance.QuitGame();
	}

	
}
