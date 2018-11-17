using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager singleton;

    [Header("Required Prefab References")]
    [Tooltip("A reference to the PlayerCharacter prefab")]
    public GameObject playerPrefab; // Should be assigned the player prefab

    void Awake() {
        bool execute = SetSingleton();

        if (!execute) {
            return;
        }
    }

    bool SetSingleton() {
        // Ensure no other instance exists
        if (singleton == null) {
            singleton = this;
            DontDestroyOnLoad(gameObject);
            return true;
        }
        else {
            Destroy(gameObject);
            return false;
        }
    }

    void Start() {

    }

    void Update() {
    }

    private void FixedUpdate() {
    }
}
