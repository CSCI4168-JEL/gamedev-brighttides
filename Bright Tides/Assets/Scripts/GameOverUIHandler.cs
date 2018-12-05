using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUIHandler : MonoBehaviour {
	public Text turnsText;
	public Text goldText;
	public Text ratingText;
	public SceneState mainMenuSceneState;

	// Use this for initialization
	void Awake () {
		int turns = GameManager.instance.GetTurnCount();
		int gold = GameManager.instance.playerInstance.GetComponent<Entity>().attributes.gold;
		int rating = (int) (turns * 10 + gold);
		string playerRatingName = "Swashbuckler";

		turnsText.text = "You survived for " + turns + " turns!" ;
		goldText.text = "You accumulated " + gold + " gold!";

		if (rating > 1000)
		{
			playerRatingName = "Admiral of the Black";
		}
		else if (rating > 800)
		{
			playerRatingName = "Buccaneer";
		}
		else if (rating > 600)
		{
			playerRatingName = "Privateer";
		}
		else if (rating > 400)
		{
			playerRatingName = "Interloper";
		}
		else if (rating > 200)
		{
			playerRatingName = "Scallywag";
		}
		else
		{
			// intentionally left blank, use default ratingText
		}
		
		ratingText.text = "You're a real " + playerRatingName + "!";

	}
	
	public void OnQuitButtonClick()
	{
		GameManager.instance.QuitGame();
	}

	public void OnMainMenuButtonClick()
	{
		SceneManager.LoadScene("MainMenu");
		GameManager.instance.LoadLevel(mainMenuSceneState);
		
	}
}
