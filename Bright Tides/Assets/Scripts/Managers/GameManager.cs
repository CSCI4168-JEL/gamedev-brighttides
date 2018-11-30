using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Game Manager class definition
 * 
 * The game manager is responsible for managing overall state in the game, 
 * including the transition from scene to scene
 * */
public class GameManager : MonoBehaviour {
    public static GameManager instance = null; // self reference for singleton pattern
    
    [Header("Game State")]
    public bool loadingGame = true; // is the game currently in a loading phase
    public bool simulateTurn = false; // are we currently actioning a turn

    [Header("Level State")]
    public SceneState sceneState; // current scene data
    public Region currentRegion; // The currently loaded region

    [Header("Player Settings")]
    public GameObject playerModel;
    public GameObject playerInstance;
    public float movementSpeed = 0.5f;

    public Transform moveToTransform;

	private GameObject userInterface;
	private GameObject playerInfoPanel;

	private UnityEngine.UI.Text uiPlayerHealth;
	private UnityEngine.UI.Text uiPlayerAmmo;
	private UnityEngine.UI.Text uiPlayerGold;
	private UnityEngine.UI.Text uiActionsRemaining;


	// Use this for initialization
	void Awake () {
        // set the singleton reference if it isn't already set, otherwise destroy
        // the object attempting to be instantiated
	    if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Debug.Log("Other GameManager instance already assgined, destroying this.");
            Destroy(gameObject);
        }

		userInterface = this.gameObject.transform.Find("UI").gameObject;
		playerInfoPanel = userInterface.transform.Find("PlayerInfo").gameObject;
		uiPlayerHealth = playerInfoPanel.transform.Find("Health").Find("Text").gameObject.GetComponent<UnityEngine.UI.Text>();
		uiPlayerAmmo = playerInfoPanel.transform.Find("Ammo").Find("Text").gameObject.GetComponent<UnityEngine.UI.Text>();
		uiPlayerGold = playerInfoPanel.transform.Find("Gold").Find("Text").gameObject.GetComponent<UnityEngine.UI.Text>();
		uiActionsRemaining = playerInfoPanel.transform.Find("ActionsRemaining").Find("Text").gameObject.GetComponent<UnityEngine.UI.Text>();

		DontDestroyOnLoad(gameObject); // prevent garbage collection on scene transitions
    }
	
	/*
     * Loads the next scene indicated by the scene data
     * */
    public void LoadNextLevel()
    {
        if (this.sceneState.nextLevel != LEVELS.none)
        {
            this.LoadLevel(this.sceneState.nextLevel);
        }
    }

    /*
     * Loads the previous scene as specified by scene data
     * */
    public void LoadPreviousLevel()
    {
        if (this.sceneState.previousLevel != LEVELS.none)
        {
            this.LoadLevel(this.sceneState.previousLevel);
        }
    }

    /*
     * Loads the specific scene by index value.
     * 
     * Index values are defined in the build settings (File | Build Settings)
     * */
    private void LoadLevel(LEVELS levelIndex)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene((int)levelIndex);
    }


    /*
     * Exits the game
     * */
    public void QuitGame()
    {
        Application.Quit();
    }

    /*
     * Call back function for when scene is finished loading.
     * 
     * This function handles additional loading and initialization of the scene
     * */
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);

        // update the scene state in the game manager
        switch (scene.buildIndex)
        {
            case (int) LEVELS.level1:
                this.sceneState = (SceneState)Resources.Load("L1R1");

                // TODO: add map data to scene state to allow for persistent maps
                // add condition check if map data is empty, if so then generate map
                // otherwise, do nothing unless a new game is chosen
                // if a new game is chosen, then map data would be erased
                if (this.sceneState.mapGenerated == false)
                {
                    GameObject region = GameObject.FindGameObjectWithTag("Region");
                    if (region == null) {
                        Debug.Log("No Region GameObject present in scene, creating one now...");
                        region = new GameObject("Region") {
                            tag = "Region"
                        };
                        this.currentRegion = region.AddComponent<Region>(); // Attach the region script
                    } else {
                        this.currentRegion = region.GetComponent<Region>(); // Get the commponent from the region in the scene
                    }
                    Debug.Log("First time in level, generating map...");
                    this.currentRegion.Initialize(); // Call the initialize method for the region
                } else
                {
                    Debug.Log("Skipping map generation...");
                }

                
                
                break;
            case (int) LEVELS.level2:
                this.sceneState = (SceneState)Resources.Load("L2R1");
                break;
            default:
                Debug.LogError("GameManager.OnSceneLoaded: Unknown scene index.  Did you remember to update to LEVELS enum?");
                break;
        }

        if (this.sceneState.showUI)
        {
            this.gameObject.transform.Find("UI").gameObject.SetActive(true);
        }

        // we have finished loading 
        this.loadingGame = false;        
    }

    public void InstantiatePlayer(Transform startingTileTransform)
    {
        playerInstance = Instantiate(playerModel, startingTileTransform);
        playerInstance.transform.position += new Vector3(0, 0.52f, 0); // To place the player above the water, not inside
        playerInstance.name = "Player";
    }

    private void Update()
    {
        if (this.simulateTurn)
        {
            if (this.moveToTransform != null)
            {
                MovePlayer();
            }
        }
    }

    private void OnGUI()
    {
		updateUIPlayerInfo();
		//GUI.Label(new Rect(10, 10, 400, 30), "Map Generated:" + this.scene.map.transform.childCount);
	}

    void SaveMapData()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream saveFile = File.Open(Application.persistentDataPath + "/" + this.sceneState.name + ".dat", FileMode.OpenOrCreate);

        foreach (Transform child in this.sceneState.map.transform)
        {
            Material b = child.gameObject.GetComponent<Material>();
            
            binaryFormatter.Serialize(saveFile, JsonUtility.ToJson(b));
        }
    }

    void MovePlayer()
    {
        Vector3 playerOffsetPosition = new Vector3(0, 0.52f, 0);
        MoveEntity(playerInstance, moveToTransform.gameObject, playerOffsetPosition, playerInstance.GetComponent<Entity>().attributes.movementSpeed);

        if (playerInstance.transform.position - playerOffsetPosition == moveToTransform.position)
        {
            this.moveToTransform = null;
            // simulateTurn = false;
        }
    }

    void MoveEntity(GameObject entity, GameObject target, Vector3 positionAdjustment, float speed)
    {
        if (entity.transform.position - positionAdjustment == target.transform.position)
        {
            Debug.Log("Moving entitiy " + entity.name + " complete.");
        }
        else
        {
            entity.transform.position = Vector3.MoveTowards(entity.transform.position, target.transform.position + positionAdjustment, speed * Time.deltaTime);
        }
    }

	void updateUIPlayerInfo()
	{
		if (playerInstance != null)
		{
			uiPlayerHealth.text = playerInstance.GetComponent<Entity>().attributes.health.ToString();
			uiPlayerAmmo.text = playerInstance.GetComponent<Entity>().attributes.ammo.ToString();
			uiPlayerGold.text = playerInstance.GetComponent<Entity>().attributes.gold.ToString();
			uiActionsRemaining.text = playerInstance.GetComponent<Entity>().attributes.actionsRemaining.ToString();
		}
	}

}
