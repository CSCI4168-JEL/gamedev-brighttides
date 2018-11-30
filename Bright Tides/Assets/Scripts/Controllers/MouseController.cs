﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MouseMode {
    move, // player is in moving mode
    explore, // player is in exploring mode
    attack // player is in attacking mode
}

public class MouseController : MonoBehaviour
{
    [Header("Indicators")]
    public GameObject selectedIndicator; // indicator game object for a tile that is selected
    public GameObject straightMovementIndicator;
    public GameObject diagonalMovementIndicator;

    [Header("Mouse Selection Materials")]
    public Material defaultMaterial;
    public Material mouseOverMaterial;
    public Material validMoveLocationrMaterial;
    public Material invalidMoveLocationrMaterial;
    public Material targetModeMaterial;

    public GameObject selectedObject; // object that is currently selected
    private GameObject selectedObjectIndicator; // instance of overlay that provides visual cue that tile is selected
    private List<GameObject> movementIndicators;

    public GameObject mouseOverObject; // object that the mouse is currently over

    private MouseMode mouseMode;
    private bool moveToIndicatorsDrawn = false;


    /*
     * Initialize the mouse controller
     * */
    void Awake()
    {
        this.GetComponent<Renderer>().material = defaultMaterial;
        mouseMode = MouseMode.explore; // Exploring by default

        movementIndicators = new List<GameObject>();
    }

    /*
     * Sets the mouses rendered material to the predefined moving indicator
     * */
    public void SetIndicatorToMove()
    {
        Debug.Log("Setting mouse indicator to moving...");
        mouseMode = MouseMode.move;
    }

    /*
     * Sets the mouse rendered material to the predefined exploring indicator
     * */
    public void SetIndicatorToExplore()
    {
        Debug.Log("Setting mouse indicator to exploring...");
        mouseMode = MouseMode.explore;
    }

    /*
     * Sets the mouse rendered material to the predefined attack indicator
     * */
    public void SetIndicatorToAttack()
    {
        Debug.Log("Setting mouse indicator to attacking...");
        mouseMode = MouseMode.attack;
    }

    /*
     * Updates the mouse rendered material to the correct material based on the players desired action 
     * */
    public void UpdateMouseIndicator()
    {
        // if the player is trying to move, then set the material to valid or invalid move, according
        // to whether or not the tile the mouse is over is pathable or not
        // otherwise, change to attacking or exploring material accordingly
        if (mouseMode == MouseMode.move)
        {
            if (mouseOverObject.GetComponent<Tile>().TileProperties.IsPathableByPlayer  && 
                Vector3.Distance(mouseOverObject.transform.position, GameManager.instance.playerInstance.transform.position) < 2.0f)
            {
                //Debug.Log("Tile is traversible");
                this.GetComponent<Renderer>().material = this.validMoveLocationrMaterial;
            }
            else
            {
                //Debug.Log("Tile is blocked");
                this.GetComponent<Renderer>().material = this.invalidMoveLocationrMaterial;
            }
        }
        else if (mouseMode == MouseMode.attack)
        {
            this.GetComponent<Renderer>().material = this.targetModeMaterial;
        }
        else if (mouseMode == MouseMode.explore)
        {
            this.GetComponent<Renderer>().material = this.defaultMaterial;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
		if (EventSystem.current.IsPointerOverGameObject()) return;

		// early exit condition: the game is loading a scene
		if (GameManager.instance.loadingGame) return;

        RaycastHit hit; // ray cast collision information
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // get a ray from camera center through mouse position

        if (mouseMode == MouseMode.move && !moveToIndicatorsDrawn)
        {
            

            HighlightMoveToTiles();
            moveToIndicatorsDrawn = true;
        }

        if (mouseMode != MouseMode.move || GameManager.instance.moveToTransform != null)
        {
            moveToIndicatorsDrawn = false;
            ClearMovementIndicators();
        }



        // compute if the ray collides with an object with a collider
        // if doesn't, clear the current mouse over object, othwerise continue processing
        if (Physics.Raycast(ray, out hit))
        {
            // update visual of where mouse is on the game map
            SetMouseOverObject(hit.transform.gameObject);
            this.gameObject.GetComponent<Renderer>().enabled = true;
            this.gameObject.transform.position = new Vector3(mouseOverObject.transform.position.x, this.transform.position.y, mouseOverObject.transform.position.z);

            // if the fire button clicked, either select or deselect the tile that the mouse is over
            if (Input.GetButtonDown("Fire1"))
            {
                switch (mouseMode)
                {
                    case MouseMode.explore:
                        UpdateSelectObject(hit.transform.gameObject);
                        break;
                    case MouseMode.move:
                        if (this.mouseOverObject.GetComponent<Tile>().TileProperties.IsPathableByPlayer && Vector3.Distance(mouseOverObject.transform.position, GameManager.instance.playerInstance.transform.position) < 2.0f)
                        {
                            GameManager.instance.moveToTransform = mouseOverObject.transform;
                        }
                        break;
                    default:
                        Debug.LogError("MouseController.Update() - Invalid mouse mode detected...");
                        break;
                }
            }

            // update mouse indicator
            UpdateMouseIndicator();
        }
        else // ray did not collide, clear mouse over object
        {
            this.gameObject.GetComponent<Renderer>().enabled = false; // disable renderer if no collision occurred (i.e. mouse if off map)
            ClearMouseOverObject();
        }
    }

    private GameObject findBaseTile(GameObject obj)
    {
        // traverse game object tree until we get a map tile
        while (obj.tag != "MapTile")
        {
            obj = obj.transform.parent.gameObject;
        }

        return obj;
    }

    /*
	 * Saves object mouse is currently over as the currently selected item
	 * */
    private void UpdateSelectObject(GameObject newSelection)
    {
        if (newSelection != null && mouseMode == MouseMode.explore)
        {
            newSelection = findBaseTile(newSelection);

            // clear the selection indicator on the currently selected object if one exists
            if (selectedObject != null)
            {
                Destroy(selectedObjectIndicator);
            }

            // if the newly selected item is already the selected item, then clear it as selected
            // otherwise, set it as the currently selected item and add the selection indicator
            if (newSelection == selectedObject)
            {
                ClearSelected();
                return;
            }
            else
            {
                selectedObject = newSelection;
                    selectedObjectIndicator = Instantiate(selectedIndicator, selectedObject.transform);
            }
        }
    }

    /*
	 * Nulls out reference to object currently selected
	 * */
    private void ClearSelected()
    {
        GameManager.instance.moveToTransform = null;
        selectedObject = null;
    }

    /*
	 * Nulls out reference to object that the mouse is over
	 * */
    private void ClearMouseOverObject()
    {
        mouseOverObject = null;
    }

    /*
	 * Sets the map tile that the mouse is currently hovering over
	 * */
    private void SetMouseOverObject(GameObject newObject)
    {
        if (newObject != null)
        {
            // traverse game object tree until we get a map tile
            while (newObject.tag != "MapTile")
            {
                newObject = newObject.transform.parent.gameObject;
            }

            if (newObject == mouseOverObject) { return; }
            ClearMouseOverObject();
        }

        mouseOverObject = newObject;        
    }
    
    /*
     * Creates movement indicator game objects on tiles that the player can make a valid move to
     * */
    private void HighlightMoveToTiles()
    {
        Transform playerTransform = GameManager.instance.playerInstance.transform;

        // compute starting vectors for casting rays
        List<Vector3> startingPoints = new List<Vector3>();

        startingPoints.Add(new Vector3(1, 5, 0)); // up
        startingPoints.Add(new Vector3(1, 5, -1)); // up right
        startingPoints.Add(new Vector3(0, 5, -1)); // right
        startingPoints.Add(new Vector3(-1, 5, -1)); // down right
        startingPoints.Add(new Vector3(-1, 5, 0)); // down
        startingPoints.Add(new Vector3(-1, 5, 1)); // down left
        startingPoints.Add(new Vector3(0, 5, 1)); // left
        startingPoints.Add(new Vector3(1, 5, 1)); // up left


        // cast rays from 1 unit in each direction from the player to determine what tiles can be moved to        
        foreach (Vector3 startPoint in startingPoints)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(playerTransform.position + startPoint, Vector3.down, out hitInfo, 10.0f))
            {
                // if we are not colliding with a pathable game tile, then do nothing
                if (findBaseTile(hitInfo.collider.gameObject).GetComponent<Tile>().TileProperties.IsPathableByPlayer == false) { continue; }
                
                // instance a movement indicator with the correct orientation
                if (startPoint.x == 1 && startPoint.z == 0)
                {
                    movementIndicators.Add(CreateMoveToIndicator(straightMovementIndicator, hitInfo.collider.gameObject.transform, new Vector3(0, 180.0f, 0)));
                }
                else if (startPoint.x == 1 && startPoint.z == -1)
                {
                    movementIndicators.Add(CreateMoveToIndicator(diagonalMovementIndicator, hitInfo.collider.gameObject.transform, new Vector3(0, -90.0f, 0)));
                }
                else if (startPoint.x == 0 && startPoint.z == -1)
                {
                    movementIndicators.Add(CreateMoveToIndicator(straightMovementIndicator, hitInfo.collider.gameObject.transform, new Vector3(0, -90.0f, 0)));
                }
                else if (startPoint.x == -1 && startPoint.z == -1)
                {
                    movementIndicators.Add(CreateMoveToIndicator(diagonalMovementIndicator, hitInfo.collider.gameObject.transform, new Vector3(0, 0, 0)));
                }
                else if (startPoint.x == -1 && startPoint.z == 0)
                {
                    movementIndicators.Add(CreateMoveToIndicator(straightMovementIndicator, hitInfo.collider.gameObject.transform, new Vector3(0, 0, 0)));
                }
                else if (startPoint.x == -1 && startPoint.z == 1)
                {
                    movementIndicators.Add(CreateMoveToIndicator(diagonalMovementIndicator, hitInfo.collider.gameObject.transform, new Vector3(0, 90.0f, 0)));
                }
                else if (startPoint.x == 0 && startPoint.z == 1)
                {
                    movementIndicators.Add(CreateMoveToIndicator(straightMovementIndicator, hitInfo.collider.gameObject.transform, new Vector3(0, 90.0f, 0)));
                }
                else if (startPoint.x == 1 && startPoint.z == 1)
                {
                    movementIndicators.Add(CreateMoveToIndicator(diagonalMovementIndicator, hitInfo.collider.gameObject.transform, new Vector3(0, 180.0f, 0)));
                }               
            }
        }
    }


    /*
     * Instantiates a movement indicator game object as a child of the specified transforms game object
     * */
    private GameObject CreateMoveToIndicator(GameObject original, Transform parentTransform, Vector3 axialRotation)
    {
        GameObject movementIndicator;
        movementIndicator = Instantiate(original, parentTransform);
        movementIndicator.transform.Rotate(axialRotation);// (0, 180.0f, 0);
    
        return movementIndicator;
    }

    /*
     * Destroys instances of movement indicators
     * */
    private void ClearMovementIndicators()
    {
        foreach (GameObject obj in movementIndicators)
        {
            Destroy(obj);
        }

        movementIndicators.Clear();

    }



}
