using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buff_script : MonoBehaviour {

    // Use this for initialization
	public LayerMask layerMask;
	private GameObject buffc;
	private Enemy en;
	private bool brainNearby;
	void Start () {
		buffc = this.transform.GetChild (1).gameObject;
		brainNearby = false;
	}

	// Update is called once per frame
	void Update () {
		if (en == null) {
			en = this.GetComponent<Enemy> ();
		}
		if (!en.isDead) {
			Collider[] enemies = Physics.OverlapSphere (this.transform.position, 8f, layerMask, QueryTriggerInteraction.Collide);
			if (enemies != null) {
				for (int i = 0; i < enemies.Length; i++) {
					if (enemies [i].tag == "SpiderBrain") {
						brainNearby = true;
						break;
						}
					brainNearby = false;
				}

			}
		} 
		else {
			buffc.SetActive (false);
		}

		if (brainNearby) {
			buffc.SetActive (true);
		} else {
			buffc.SetActive (false);
		}
			
       
	}
}
