using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseUIHandler : MonoBehaviour
{
	public SceneState mainMenuSceneState;

	public void OnContinueButtonClick()
	{
		this.gameObject.SetActive(false);
	}

	public void OnMainMenuButtonClick()
	{
		this.gameObject.SetActive(false);
		SceneManager.LoadScene("MainMenu");
		GameManager.instance.LoadLevel(mainMenuSceneState);

	}
}
