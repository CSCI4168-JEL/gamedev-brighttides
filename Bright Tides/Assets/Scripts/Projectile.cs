using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	private const float RAD_DEGREES = Mathf.PI / 180;

	public GameObject MoveToTarget { get; set; }
	public float speed = 1.0f;

	private Vector3 startPosition;
	private Vector3 newPosition;
	private float travelDistance = -1f;
	private float startTime;

	private int baseDamage;
	private float damageModifier;
	
	

	// Use this for initialization
	void Awake () {
		GameManager.instance.isPerformingAction = true;
		startPosition = transform.parent.position;
		baseDamage = transform.parent.gameObject.GetComponent<Entity>().attributes.baseAttackDamage;
		damageModifier = 0.4f;
		startTime = Time.time;		
		
	}
	
	// Update is called once per frame
	void Update () {
		if (MoveToTarget != null)
		{
			if (travelDistance == -1f)
			{
				travelDistance = Vector3.Distance(startPosition, MoveToTarget.transform.position);
			}

			float distCovered = (Time.time - startTime) * speed;
			float percentComplete = distCovered / travelDistance;

			if (percentComplete > 1.0f)
			{

				MoveToTarget.GetComponent<Entity>().DealDamage((int) (baseDamage * Random.Range(damageModifier, 1.0f))); // Deal the appropriate damage to the target on impact
				Destroy(this.gameObject); // Remove the projectile. Apply earth-shattering kaboom here?
				GameManager.instance.isPerformingAction = false;
			}
			else
			{
				newPosition = Vector3.Lerp(startPosition, MoveToTarget.transform.position, percentComplete);
				newPosition.y += Mathf.Sin(Mathf.Lerp(0f, 180f, percentComplete) * RAD_DEGREES) / (4 /  1 / travelDistance);

				transform.LookAt(MoveToTarget.transform, Vector3.up);
				transform.position = newPosition;
			}
		}
	}

}
