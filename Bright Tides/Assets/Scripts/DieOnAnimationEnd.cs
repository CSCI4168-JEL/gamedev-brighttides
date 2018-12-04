using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DieOnAnimationEnd : MonoBehaviour {
	private float timeToLive;

	// Use this for initialization
	void Start () {
		timeToLive = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;
		Destroy(this.gameObject, timeToLive);
	}
}
