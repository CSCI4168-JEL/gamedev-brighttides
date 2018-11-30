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
[System.Serializable]
public class SceneState : ScriptableObject  {
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
    public bool mapGenerated = false;

    public LEVELS previousLevel;
    public LEVELS nextLevel;

}
