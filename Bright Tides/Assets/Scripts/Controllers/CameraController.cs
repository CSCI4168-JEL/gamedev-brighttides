using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("Game object that the camera will follow.")]
    public GameObject objectBeingFollowed;

    private Transform objectBeingFollowedTransform; // The target that the camera will follow

    [Tooltip("Camera position relative to the object being followed.")]
    public Vector3 followPosition;

    private Vector3 newPosition; // position that the camera will move to at the end of each frame

    // Use this for initialization
    void Start()
    {
        objectBeingFollowed = GameManager.instance.playerInstance;
        objectBeingFollowedTransform = GameManager.instance.playerInstance.transform; //PlayerManager.singleton.playerInstance.transform;
                                                                                      // offset = transform.position - target.position; // Get the offset as the difference between target and camera position
        transform.LookAt(objectBeingFollowedTransform);

        newPosition = objectBeingFollowedTransform.position;
        newPosition += followPosition.x * objectBeingFollowedTransform.right;
        newPosition += followPosition.z * objectBeingFollowedTransform.forward;
        newPosition += followPosition.y * objectBeingFollowedTransform.up;

        transform.position = newPosition;
		transform.LookAt(objectBeingFollowedTransform.position);
	}


    void LateUpdate()
    {
        //compute new camera position
        newPosition = objectBeingFollowedTransform.position + followPosition;


        //zoom in or out based on mouse wheel direction
        if (Input.GetAxis("Mouse ScrollWheel") != 0) //&& transform.rotation.x > 0)
        {
            //followPosition.y += -Input.GetAxis("Mouse ScrollWheel") * cameraZoomSpeed;
            //followPosition.z -= -Input.GetAxis("Mouse ScrollWheel") * cameraZoomSpeed;
        }

        //set the new camera position and look toward the object being followed
        transform.position = Vector3.MoveTowards(transform.position, newPosition, 0.1f);


        // turn left
        if (Input.GetAxis("Mouse X") < 0)
        {
            // GetComponent<Transform>().Rotate(Vector3.up, Input.GetAxis("Mouse X") * objectBeingFollowed.GetComponent<PlayerControl>().rotationSpeed * Time.deltaTime, Space.World);
        }

        // turn right
        else if (Input.GetAxis("Mouse X") > 0)
        {
            // GetComponent<Transform>().Rotate(Vector3.up, Input.GetAxis("Mouse X") * objectBeingFollowed.GetComponent<PlayerControl>().rotationSpeed * Time.deltaTime, Space.World);
        }

        // look up
        if (Input.GetAxis("Mouse Y") > 0)
        {

            // GetComponent<Transform>().Rotate(transform.right, Input.GetAxis("Mouse Y") * objectBeingFollowed.GetComponent<PlayerControl>().rotationSpeed * Time.deltaTime, Space.World);
        }

        // look down
        else if (Input.GetAxis("Mouse Y") < 0)
        {
            // GetComponent<Transform>().Rotate(transform.right, Input.GetAxis("Mouse Y") * objectBeingFollowed.GetComponent<PlayerControl>().rotationSpeed * Time.deltaTime, Space.World);
        }

        //transform.LookAt(objectBeingFollowedTransform.position);
		
    }
}
