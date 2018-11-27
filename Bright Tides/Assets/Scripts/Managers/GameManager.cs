using System.Collections;
using System.Collections.Generic;
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
    public SceneState scene; // current scene data

    // Use this for initialization
    void Awake () {
        // set the singleton reference if it isn't already set, otherwise destroy
        // the object attempting to be instantiated
	    if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject); // prevent garbage collection on scene transitions
    }
	
	/*
     * Loads the next scene indicated by the scene data
     * */
    public void LoadNextLevel()
    {
        if (this.scene.nextLevel != LEVELS.none)
        {
            this.LoadLevel(this.scene.nextLevel);
        }
    }

    /*
     * Loads the previous scene as specified by scene data
     * */
    public void LoadPreviousLevel()
    {
        if (this.scene.previousLevel != LEVELS.none)
        {
            this.LoadLevel(this.scene.previousLevel);
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
        Debug.Log(mode);

        // update the scene state in the game manager
        this.scene = (SceneState) Resources.Load("L1R1");

        // we have finished loading 
        this.loadingGame = false;        
    }
}
