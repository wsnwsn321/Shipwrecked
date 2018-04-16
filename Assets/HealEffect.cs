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
			hp = gameObject.GetComponentInParent<PlayerHealth> ();
			if (hp == null) {
				Debug.Log ("Failed to heal player! PlayerHealth not set!");
			} else {
				Debug.Log ("Regen activated for "+ this.gameObject.transform.parent.gameObject.name + "!");
			}
		}
	}
}
