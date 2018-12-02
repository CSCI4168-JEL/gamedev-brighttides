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
	public bool isPerformingAction = false; // is an action (such as attacking or moving) currently happening

    [Header("Level State")]
    public SceneState sceneState; // current scene data
    public Region currentRegion; // The currently loaded region

    [Header("Player Settings")]
    public GameObject playerModel;
    public GameObject playerInstance;
	public EntityAttributes playerAttributesTemplates;
	
    public float movementSpeed = 0.5f;

    public Tile moveToTile;

	private GameObject userInterface;
	private GameObject playerInfoPanel;

	private UnityEngine.UI.Text uiPlayerHealth;
	private UnityEngine.UI.Text uiPlayerAmmo;
	private UnityEngine.UI.Text uiPlayerGold;
	private UnityEngine.UI.Text uiActionsRemaining;

	private UnityEngine.UI.Text uiTurnCount;
	private int turnCount;


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

		uiTurnCount = playerInfoPanel.transform.Find("TurnCount").Find("Count").gameObject.GetComponent<UnityEngine.UI.Text>();
		

		DontDestroyOnLoad(gameObject); // prevent garbage collection on scene transitions
    }

	public void Simulate()
	{
		GameManager.instance.simulateTurn = true; // turn on turn simulation to prevent user actions

		EntityAttributes playerAttributes = GameManager.instance.playerInstance.GetComponent<Entity>().attributes;

		// update player attributes before ending turn
		playerAttributes.actionsRemaining = playerAttributes.actionsPerTurn;
		int turnCount = int.Parse(instance.uiTurnCount.text);

		GameManager.instance.uiTurnCount.text = (++turnCount).ToString();
		GameManager.instance.simulateTurn = false; // turn is over, let player do stuff
	}




	public void StartGame()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.LoadScene("Game");
		
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.LoadLevel(this.sceneState.nextLevel);
	}

	/*
     * Loads the next scene indicated by the scene data
     * */
	public void LoadNextLevel()
    {
		if (this.sceneState.nextLevel != null)
		{
			Debug.Log("LoadNextLevel()");
			this.LoadLevel(this.sceneState.nextLevel);
		}
    }

    /*
     * Loads the previous scene as specified by scene data
     * */
    public void LoadPreviousLevel()
    {
		Debug.Log("LoadPreviousLevel()");
		if (this.sceneState.previousLevel != null)
		{
            this.LoadLevel(this.sceneState.previousLevel);
        }
    }

	/*
     * Loads the specific scene by index value.
     * 
     * Index values are defined in the build settings (File | Build Settings)
     * */
	private void LoadLevel(SceneState sceneState)
	{
		Debug.Log("LoadLevel()");
		this.sceneState = sceneState;
		this.sceneState.OnSceneTransition();
	}


    /*
     * Exits the game
     * */
    public void QuitGame()
    {
        Application.Quit();
    }

    public void InstantiatePlayer(Transform startingTileTransform)
    {
		// only create a new player instance if one doesn't exist
		// otherwise just update it's position
		if (playerInstance == null )
		{
			playerInstance = Instantiate(playerModel, startingTileTransform);
			playerInstance.GetComponent<Entity>().AttributesTemplate = ScriptableObject.Instantiate(GameManager.instance.playerAttributesTemplates);
			playerInstance.name = "Player";
		}
		else
		{
			playerInstance.transform.position = startingTileTransform.position;
			playerInstance.transform.parent = startingTileTransform;
		}
        

        playerInstance.transform.position += new Vector3(0, 0.52f, 0); // To place the player above the water, not inside
        
    }

    private void Update()
    {
        if (!this.simulateTurn)
        {
            if (this.moveToTile != null)
            {
                MovePlayerToTile();
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

    void MovePlayerToTile()
    {
		if (GameManager.instance.playerInstance.GetComponent<Entity>().attributes.actionsRemaining <= 0) return;

		Entity playerEntity = playerInstance.GetComponent<Entity>();
        MoveEntityToTile(playerEntity, moveToTile, playerEntity.attributes.movementSpeed);

        if (playerInstance.transform.parent == moveToTile.transform) // If the player has reached the tile, the tile becomes the parent
        {
			GameManager.instance.isPerformingAction = false;
			GameManager.instance.playerInstance.GetComponent<Entity>().attributes.actionsRemaining--;
			if (moveToTile.TileProperties.tileType == TileType.playerExitTile)
			{
				this.LoadNextLevel();
			}

            this.moveToTile = null;
            // simulateTurn = false;
        }
    }

    void MoveEntityToTile(Entity entity, Tile target, float speed)
    {
		GameManager.instance.isPerformingAction = true;
		if (entity.transform.position == target.tileTopPosition) // Move the entity towards the top of the tile
        {
            Debug.Log("Moving entitiy " + entity.name + " complete.");
            target.SetTileAsParent(entity); // After the movement is complete, update the parent of the entity and the pathability of the tile
			GameManager.instance.isPerformingAction = false;
		}
        else
        {
            entity.transform.position = Vector3.MoveTowards(entity.transform.position, target.tileTopPosition, speed * Time.deltaTime);
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