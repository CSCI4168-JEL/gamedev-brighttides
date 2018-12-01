using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Bright Tides/Scene Data", fileName = "Scene State", order = 1)]
[System.Serializable]
public class SceneState : ScriptableObject  {
	[Header("Scene Definition")]
	public string sceneName;
	public TextAsset mapDefinitionFile;
	public TileSet tileSet;

	[Header("Scene Settings")]
	public bool showUI;

	[Header("Scene Data")]
	public GameObject map;
	public GameObject playerStartPosition;

	public SceneState previousLevel;
	public SceneState nextLevel;

	public void OnSceneTransition()
	{
		Debug.Log("SceneState initializing..." + sceneName);
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.LoadScene(sceneName);
	}

	/*
	* Call back function for when scene is finished loading.
	* 
	* This function handles additional loading and initialization of the scene
	* */
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Debug.Log("OnSceneLoaded: " + scene.name);

		playerStartPosition = this.ConstructMap();

		if (GameManager.instance.playerInstance == null)
		{
			GameManager.instance.InstantiatePlayer(playerStartPosition.transform);
		}
		else
		{
			GameManager.instance.transform.position = playerStartPosition.transform.position;
		}

		if (this.showUI)
		{
			GameManager.instance.gameObject.transform.Find("UI").gameObject.SetActive(this.showUI);
		}

		// we have finished loading 
		GameManager.instance.loadingGame = false;
	}


	GameObject ConstructMap()
	{
		MapGenerator mg = new MapGenerator(this.mapDefinitionFile, this.tileSet);

		this.map = mg.Generate();
		return mg.StartingPosition;
	}
}
