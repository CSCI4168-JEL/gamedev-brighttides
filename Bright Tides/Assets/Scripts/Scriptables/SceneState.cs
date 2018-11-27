using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LEVELS : int
{
    none = -1,
    mainMenu = 0,
    level1 = 1,
    level2 = 2
}

[CreateAssetMenu(menuName = "Bright Tides/Scene Data", fileName = "Scene State", order = 1)]
public class SceneState : ScriptableObject  {
    [Header("Scene Data")]
    public string sceneName;

    public LEVELS previousLevel;
    public LEVELS nextLevel;

    [Header("Player Settings")]
    public GameObject playerModel;
    public GameObject playerInstance;
    public float movementSpeed = 0.5f;

    public Transform moveToTransform;

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
