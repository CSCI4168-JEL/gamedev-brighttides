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
    public TileSet tileSet;

    [Header("Scene Data")]
    public GameObject map;
    public bool mapGenerated = false;

    public LEVELS previousLevel;
    public LEVELS nextLevel;


    

    

}
