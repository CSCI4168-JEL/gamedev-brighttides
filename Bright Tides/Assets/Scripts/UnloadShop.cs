using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadShop : MonoBehaviour {
	private void Start()
	{
		GameManager.instance.transform.Find("UI").gameObject.SetActive(false);
	}
	public void OnProceedClick()
	{
		GameManager.instance.ExitShop();
	}
}
