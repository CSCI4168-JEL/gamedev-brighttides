using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

/*
 * Game Manager class definition
 * 
 * The game manager is responsible for managing overall state in the game, 
 * including the transition from scene to scene
 * */
public class GameManager : MonoBehaviour
{
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

    // public Tile selectedMovementTile;

    private GameObject userInterface;
    private GameObject playerInfoPanel;

    private UnityEngine.UI.Text uiPlayerHealth;
    private UnityEngine.UI.Text uiPlayerAmmo;
    private UnityEngine.UI.Text uiPlayerGold;
    private UnityEngine.UI.Text uiActionsRemaining;

    private UnityEngine.UI.Text uiTurnCount;
    private int turnCount;

    // Use this for initialization
    void Awake()
    {
        // set the singleton reference if it isn't already set, otherwise destroy
        // the object attempting to be instantiated
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
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

    private void OnGUI() {
        UpdateUIPlayerInfo();
    }

    // Method to enable/disable the UI for the current scene
    public void ToggleUI(bool enabled) {
        if (userInterface) { // Get the current userInterface reference
            userInterface.gameObject.SetActive(enabled);
        } else {
            Debug.LogError("No UI found in the scene, cannot toggle its status!");
        }
    }

    // This is called to take control from the player for the enemy turn
    public void EndPlayerTurn()
	{
        if (isPerformingAction) {
            Debug.LogWarning("Player attempted to end turn in the middle of an action... Be patient :)");
            return;
        }

        if (simulateTurn) {
            Debug.LogWarning("Player attempted to end turn during the enemy's turn. Wow, rude.");
            return;
        }

        simulateTurn = true; // turn on turn simulation to prevent user actions
        if (currentRegion != null && currentRegion.enemyController != null) {
            currentRegion.enemyController.StartEnemyTurn(); // Begin the enemy turn
        }
        else {
            StartPlayerTurn(); // Enemies cannot take a turn, so start the player's next turn
        }
    }

    // This is called to give the player control again
    public void StartPlayerTurn() {
		ResetPlayerActions();

        simulateTurn = false; // turn is over, let player do stuff
    }

	private void ResetPlayerActions()
	{
		EntityAttributes playerAttributes = playerInstance.GetComponent<Entity>().attributes;

		// update player attributes before ending turn
		playerAttributes.actionsRemaining = playerAttributes.actionsPerTurn;
		int turnCount = int.Parse(uiTurnCount.text);

		uiTurnCount.text = (++turnCount).ToString();
		
	}

	public static void AddFloatingText(Vector3 position, Vector3 offset, string text, string materialName)
	{
		GameObject floatingText = Instantiate(Resources.Load<GameObject>("Prefabs/UI/FloatingText"));
		floatingText.transform.position = position + offset;
		
		if (materialName != null)
		{
			Material customMaterial = Resources.Load<Material>("Materials/UI/" + materialName);
			floatingText.GetComponent<Renderer>().material = customMaterial;
		}
		
		floatingText.GetComponent<TextMeshPro>().text = text;
	}

    public void StartGame()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Game");

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
			this.ResetPlayerActions();
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

    // Callback for when a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        this.LoadLevel(this.sceneState.nextLevel);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /*
     * Exits the game
     * */
    public void QuitGame()
    {
        Application.Quit();
    }

    public void InstantiatePlayer(Tile startingTile)
    {
		if (playerInstance == null) // Only create a new player instance if one doesn't exist
        {
			playerInstance = Instantiate(playerModel, startingTile.transform);
            playerInstance.GetComponent<Entity>().AttributesTemplate = ScriptableObject.Instantiate(GameManager.instance.playerAttributesTemplates);
            playerInstance.name = "Player";
		}
        startingTile.SetTileAsParent(playerInstance.GetComponent<Entity>()); // Update the player position and tile
    }

    private void SaveMapData()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream saveFile = File.Open(Application.persistentDataPath + "/" + this.sceneState.name + ".dat", FileMode.OpenOrCreate);

        foreach (Transform child in this.sceneState.map.transform)
        {
            Material b = child.gameObject.GetComponent<Material>();

            binaryFormatter.Serialize(saveFile, JsonUtility.ToJson(b));
        }
    }

    public void PlayerMoveToTile(Tile destination) {
        Entity playerEntity = playerInstance.GetComponent<Entity>();

        // Call the MoveToTile coroutine 
        StartCoroutine(PlayerPerformAction(playerEntity.MoveToTile(destination), () => {
            if (destination.TileProperties.tileType == TileType.playerExitTile) {
                this.LoadNextLevel(); // After the coroutine runs, if the tile reached is the goal, go to the next level
            }
        }));
    }

    public void PlayerAttackEntity(Entity target) {
        Entity playerEntity = playerInstance.GetComponent<Entity>();

        // If the player has ammo, call the Attack coroutine
        if (playerEntity.attributes.ammo > 0) {
            StartCoroutine(PlayerPerformAction(playerEntity.Attack(target), null, () => {
                playerEntity.attributes.ammo--; // Reduce the ammo before the attack coroutine runs
            }));
        }
    }

    // Wrapper method for state-controlled player actions with optional callbacks before and after the action
    private IEnumerator PlayerPerformAction(IEnumerator actionCallback, Action after = null, Action before = null) {
        if (playerInstance.GetComponent<Entity>().attributes.actionsRemaining <= 0) {
            Debug.LogWarning("Out of moves, unable to perform player action");
            yield break; // Exit early since no moves remain
        }

        if (isPerformingAction) {
            Debug.LogWarning("An action is already underway, unable to perform player action");
            yield break; // Exit early since action is already being performed
        }

        if (before != null) {
            before();
        }

        isPerformingAction = true; // Prevent further actions from starting
        yield return StartCoroutine(actionCallback); // Perform the specified action
        isPerformingAction = false; // The action is complete

        playerInstance.GetComponent<Entity>().attributes.actionsRemaining--; // Reduce remaining actions

        if (after != null) {
            after();
        }

    }

    private void UpdateUIPlayerInfo()
    {
        if (playerInstance != null)
        {
            uiPlayerHealth.text = playerInstance.GetComponent<Entity>().attributes.health.ToString();
            uiPlayerAmmo.text = playerInstance.GetComponent<Entity>().attributes.ammo.ToString();
            uiPlayerGold.text = playerInstance.GetComponent<Entity>().attributes.gold.ToString();
            uiActionsRemaining.text = playerInstance.GetComponent<Entity>().attributes.actionsRemaining.ToString();

        }
    }

    // Method called to proceed to next level from the shop
    public void ExitShop()
    {
        if (SceneManager.GetSceneByBuildIndex(3).isLoaded)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(3));
			if (sceneState.showUI)
			{
                ToggleUI(sceneState.showUI);
			}
		}
        else
        {
            Debug.Log("Shop scene is not currently loaded");
        }
    }
}