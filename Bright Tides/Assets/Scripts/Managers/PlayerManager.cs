using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : Singleton<PlayerManager> {

    public static PlayerManager singleton;
    public GameObject playerInstance;

    private string playerTag = "Player";

    private GameObject _player;
    public GameObject Player {
        get {
            if (_player == null) {
                _player = GameObject.FindGameObjectWithTag(playerTag);

                if (_player) {
                    Debug.LogError("No instance of GameObject with tag: " + playerTag + " exists in the scene.");
                }
            }
            return _player;
        }
    }

    void Start() {

    }

    void Update() {

    }
}
