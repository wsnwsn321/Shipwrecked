using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buff_script : MonoBehaviour {

    // Use this for initialization
	public LayerMask layerMask;
	private GameObject buffc;
	private Enemy en;
	void Start () {
		buffc = this.transform.GetChild (1).gameObject;
		en = this.GetComponent<Enemy> ();
	}

	// Update is called once per frame
	void Update () {
		if (!en.isDead) {
			Collider[] enemies = Physics.OverlapSphere (this.transform.position, 8f, layerMask, QueryTriggerInteraction.Collide);
			if (enemies != null) {
				for (int i = 0; i < enemies.Length; i++) {
					if (enemies [i].tag == "SpiderBrain") {
						buffc.SetActive (true);
					} 
					else {
						buffc.SetActive (false);
					}
				}
			}
		} 
		else {
			buffc.SetActive (false);
		}
			
       
	}
}
