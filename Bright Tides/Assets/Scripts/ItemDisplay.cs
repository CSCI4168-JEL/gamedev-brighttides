using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour {

    public Item item;

    public Image itemIcon;
    public Text itemName;
    public Text purchasePrice;

	// Use this for initialization
	void Start () {
        if (item != null)
        {
            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
            purchasePrice.text = item.price.ToString();
        }
        else
        {
            Debug.Log("No item found.");
        }
	}
	
	
}
