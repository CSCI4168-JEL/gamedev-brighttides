using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
	private EntityAttributes attributesTemplate;

	public EntityAttributes AttributesTemplate
	{
		get { return this.attributesTemplate; }
		set
		{
			attributesTemplate = value;
			if (attributesTemplate != null)
			{
				attributes = ScriptableObject.Instantiate(attributesTemplate);
			}
		}
	}

	public EntityAttributes attributes;

	private void Awake()
	{
		Debug.Log("Setting attributes for " + gameObject.name);
		if (attributesTemplate != null)
		{
			attributes = ScriptableObject.Instantiate(attributesTemplate);
		}
	}
}
