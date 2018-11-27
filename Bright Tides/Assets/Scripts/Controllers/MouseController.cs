using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Selection Indicators")]
    public GameObject selectedIndicator; // indicator game object for a tile that is selected

    [Header("Mouse Selection Materials")]
    public Material mouseOverMaterial;
    public Material validMoveLocationrMaterial;
    public Material invalidMoveLocationrMaterial;
    public Material targetModeMaterial;


    public GameObject selectedObject; // object that is currently selected

    private GameObject selectedObjectIndicator;

    public GameObject mouseOverObject; // object that the mouse is currently over




    // Use this for initialization
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(gameManager.loadingGame);
        if (gameManager.loadingGame) return;

        RaycastHit hit; // ray cast collision information
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // get a ray from camera center through mouse position

        // compute if the ray collides with an object with a collider
        // if doesn't, clear the current mouse over object, othwerise continue processing
        if (Physics.Raycast(ray, out hit))
        {
            // if the fire button clicked, either select or deselect the tile that the mouse is over
            if (Input.GetButtonDown("Fire1"))
            {
                UpdateSelectObject(hit.transform.gameObject);
            }

            // update visual of where mouse is on the game map
            SetMouseOverObject(hit.transform.gameObject);
            this.gameObject.GetComponent<Renderer>().enabled = true;
            this.gameObject.transform.position = new Vector3(mouseOverObject.transform.position.x, this.transform.position.y, mouseOverObject.transform.position.z);
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
        if (newSelection != null)
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
                gameManager.moveToTransform = newSelection.transform;
                selectedObjectIndicator = Instantiate(selectedIndicator, selectedObject.transform);
            }
        }
    }

    /*
	 * Nulls out reference to object currently selected
	 * */
    private void ClearSelected()
    {
        gameManager.moveToTransform = null;
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
