using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    private Transform target; // The target that the camera will follow
    private Vector3 offset; // The initial distance of the camera from the target

	// Use this for initialization
	void Start () {
        target = PlayerManager.singleton.playerPrefab.transform;
        offset = transform.position - target.position; // Get the offset as the difference between target and camera position
        transform.LookAt(target);
    }

    // Update is called once per frame
    void Update () {
        transform.position = target.position + offset; // Move the camera by the offset
        transform.LookAt(PlayerManager.singleton.playerPrefab.transform);
    }
}
