using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour {

    private CharacterController characterController;

    [Tooltip("How fast the player will move in units per second")]
    public float moveSpeed = 7.0f;

    [Tooltip("The expected height that a player will climb to, dependent on gravityScale")]
    public float jumpForce = 21f; // The height that a player should climb to

    [Tooltip("The proportion that gravity will affect the player. Best results are a number between 0.1 and 1")]
    public float gravityScale = 0.2f; // Floatier character

    // Movement fields
    private Vector3 currentMovement = new Vector3(0f, 0f, 0f);
    private bool isMoving; // Signify if the player is currently moving
    private Vector3 startPos;
    private Vector3 endPos;


    // Use this for initialization
    void Start() {
        characterController = GetComponent<CharacterController>(); // Get a reference to the object's CharacterController
    }

    // Update is called once per frame
    void Update() {
        
    }
}
