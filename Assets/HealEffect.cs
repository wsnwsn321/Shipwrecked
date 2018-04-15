using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Heals the player when active
public class HealEffect : MonoBehaviour {

	PlayerHealth hp;
	public float regenAmount = 1f;

	// Use this for initialization
	void Start () {
		hp = this.gameObject.GetComponent<PlayerHealth> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		hp.RecoverHealthLocally(regenAmount);
	}
}
