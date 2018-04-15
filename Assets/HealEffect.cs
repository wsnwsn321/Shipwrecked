using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Heals the player when active
public class HealEffect : MonoBehaviour {

	[HideInInspector]
	public PlayerHealth hp;
	public float regenAmount = 0.1f;

	// Update is called once per frame
	void FixedUpdate () {
		if (hp != null) {
			hp.RecoverHealthLocally (regenAmount);
		} else {
			Debug.Log ("Failed to heal player! PlayerHealth not set!");
		}
	}
}
