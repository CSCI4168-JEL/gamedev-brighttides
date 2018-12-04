using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))] // Must have an attached button
public class ItemDisplay : MonoBehaviour {

    public Item item;

    public Image itemIcon;
    public Text itemName;
    public Text purchasePrice;

    private Button button; // The attached button script

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>(); // Get the attached button

        if (item != null)
        {
            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
            purchasePrice.text = item.price.ToString();
            button.onClick.AddListener(() => PurchaseItem()); // Add the click listener to the button
        }
        else
        {
            Debug.Log("No item found.");
        }
	}

    public void PurchaseItem() {
        ShopManager shopManager = transform.root.GetComponentInChildren<ShopManager>(); // Get the Shop Manager in the scene
        if (shopManager) {
            shopManager.AddPlayerItem(item); // Attempt to purchase the item that is displayed
        } else {
            Debug.LogError("Unable to find the shop manager! Is the scene shop correctly set up?");
        }
    }
}
