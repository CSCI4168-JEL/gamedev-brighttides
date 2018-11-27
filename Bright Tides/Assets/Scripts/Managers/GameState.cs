using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LEVELS : int
{
    none = -1,
    mainMenu = 0,
    level1 = 1,
    level2 = 2
}

[CreateAssetMenu(menuName = "Bright Tides/Game Manager", fileName = "Game State", order = 1)]
public class GameState : ScriptableObject  {
    [Header("Scene Data")]
    public string sceneName;

    public LEVELS previousLevel;
    public LEVELS nextLevel;

    [Header("Game State")]
    public bool loadingGame = true;
    public bool simulateTurn = false;

    [Header("Player Settings")]
    public GameObject playerModel;
    public GameObject playerInstance;
    public float movementSpeed = 0.5f;

    public Transform moveToTransform;


    public void LoadNextLevel()
    {
        if (this.nextLevel != LEVELS.none)
        {
            this.LoadLevel(this.nextLevel);
        }
    }

    public void LoadPreviousLevel()
    {
        if (this.previousLevel != LEVELS.none)
        {
            this.LoadLevel(this.previousLevel);
        }
    }

    private void LoadLevel(LEVELS levelIndex)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene((int) levelIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // call back when scene is finished loading
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        this.loadingGame = false;
    }

    void MovePlayer()
    {
        // The step size is equal to speed times frame time.
        float step = movementSpeed * Time.deltaTime;

        // Move our position a step closer to the target.
        playerInstance.transform.position = Vector3.MoveTowards(playerInstance.transform.position, moveToTransform.position + new Vector3(0, 0.52f, 0), step);

    }

    public void InstantiatePlayer(Transform startingTileTransform)
    {
        playerInstance = Instantiate(playerModel, startingTileTransform);
        playerInstance.transform.position += new Vector3(0, 0.52f, 0);
        playerInstance.name = "Player";
    }

}
