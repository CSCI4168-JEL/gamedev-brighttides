using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private Material currentIndicator = null;

    [Header("Selection Indicators")]
    public GameObject selectedIndicator; // indicator game object for a tile that is selected

    [Header("Mouse Selection Materials")]
    public Material defaultMaterial;
    public Material mouseOverMaterial;
    public Material validMoveLocationrMaterial;
    public Material invalidMoveLocationrMaterial;
    public Material targetModeMaterial;


    public GameObject selectedObject; // object that is currently selected

    private GameObject selectedObjectIndicator;

    public GameObject mouseOverObject; // object that the mouse is currently over

    private int mouseMode;


    // Use this for initialization
    void Awake()
    {
        this.GetComponent<Renderer>().material = defaultMaterial;
        mouseMode = 0;
    }

    public void SetIndicatorToMove()
    {
        Debug.Log("Setting mouse indicator to moving...");
        mouseMode = 1;
    }

    public void SetIndicatorToExplore()
    {
        Debug.Log("Setting mouse indicator to exploring...");
        mouseMode = 0;
    }

    public void SetIndicatorToAttack()
    {
        Debug.Log("Setting mouse indicator to attacking...");
        mouseMode = 2;
    }

    public void UpdateMouseIndicator()
    {
        if (mouseMode == 1)
        {
            if (mouseOverObject.GetComponent<Tile>().TileProperties.IsPathable)
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
        else if (mouseMode == 2)
        {
            this.GetComponent<Renderer>().material = this.targetModeMaterial;
        }
        else if (mouseMode == 0)
        {
            this.GetComponent<Renderer>().material = this.defaultMaterial;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(gameManager.loadingGame);
        if (GameManager.instance.loadingGame) return;

        RaycastHit hit; // ray cast collision information
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // get a ray from camera center through mouse position

        // compute if the ray collides with an object with a collider
        // if doesn't, clear the current mouse over object, othwerise continue processing
        if (Physics.Raycast(ray, out hit))
        {
            // if the fire button clicked, either select or deselect the tile that the mouse is over
            if (Input.GetButtonDown("Fire1") && mouseMode == 0)
            {
                UpdateSelectObject(hit.transform.gameObject);
            }

            // update visual of where mouse is on the game map
            SetMouseOverObject(hit.transform.gameObject);
            this.gameObject.GetComponent<Renderer>().enabled = true;
            this.gameObject.transform.position = new Vector3(mouseOverObject.transform.position.x, this.transform.position.y, mouseOverObject.transform.position.z);

            // set movement target if in move mode and player selected a tile that is pathable
            if (Input.GetButtonDown("Fire1") && mouseMode == 1 && this.mouseOverObject.GetComponent<Tile>().TileProperties.IsPathable)
            {
                GameManager.instance.moveToTransform = mouseOverObject.transform;
            }

            // update mouse indicator
            UpdateMouseIndicator();

        }
        else // ray did not collide, clear mouse over object
        {
            //Debug.Log("No ray collision on mouse.");
            this.gameObject.GetComponent<Renderer>().enabled = false; // disable renderer if no collision occurred (i.e. mouse if off map)
            ClearMouseOverObject();
        }
    }

    /*
	 * Saves object mouse is currently over as the currently selected item
	 * */
    private void UpdateSelectObject(GameObject newSelection)
    {
        if (newSelection != null && mouseMode == 0)
        {
            // traverse game object tree until we get a map tile
            while (newSelection.tag != "MapTile")
            {
                newSelection = newSelection.transform.parent.gameObject;
            }

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

    
}
