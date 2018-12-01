using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Bright Tides/Scene Data", fileName = "Scene State", order = 1)]
[System.Serializable]
public class SceneState : ScriptableObject
{
	[Header("Scene Definition")]
	public string sceneName;
	public TextAsset mapDefinitionFile;

	[Tooltip("The set of tile prefabs chosen for the scene.")]
	public TileSet tileSet; // The tiles used in the scene

	[Tooltip("The set of entities chosen for the scene.")]
	public EntitySet entitySet; // The entities used in the scene

	[Header("Scene Settings")]
	public bool showUI;

	[Tooltip("The number of enemies to spawn in the map if the spawn points are available.")]
	public int enemyPopulation;

	[Tooltip("The number of treasures to spawn in the map if the spawn points are available.")]
	public int treasurePopulation;

	[Header("Scene Data")]
	public GameObject map;
	public GameObject playerStartPosition;

	public SceneState previousLevel;
	public SceneState nextLevel;

	private bool mapGenerated = false;



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

		if (this.mapGenerated == false)
		{
			GameObject region = GameObject.FindGameObjectWithTag("Region");
			if (region == null)
			{
				Debug.Log("No Region GameObject present in scene, creating one now...");
				region = new GameObject("Region")
				{
					tag = "Region"
				};
				GameManager.instance.currentRegion = region.AddComponent<Region>(); // Attach the region script
			}
			else
			{
				GameManager.instance.currentRegion = region.GetComponent<Region>(); // Get the commponent from the region in the scene
			}
			Debug.Log("First time in level, generating map...");
			GameManager.instance.currentRegion.Initialize(); // Call the initialize method for the region
		}
		else
		{
			Debug.Log("Skipping map generation...");
		}


		//playerStartPosition = this.ConstructMap();

		//if (GameManager.instance.playerInstance == null)
		//{
		//	GameManager.instance.InstantiatePlayer(playerStartPosition.transform);
		//}
		//else
		//{
		//	GameManager.instance.transform.position = playerStartPosition.transform.position;
		//}

		if (this.showUI)
		{
			GameManager.instance.gameObject.transform.Find("UI").gameObject.SetActive(this.showUI);
		}

		// we have finished loading 
		GameManager.instance.loadingGame = false;
	}

	//GameObject ConstructMap()
	//{
	//	MapGenerator mg = new MapGenerator(this.mapDefinitionFile, this.tileSet);

	//	this.map = mg.Generate();
	//	return mg.StartingPosition;
	//}
}
