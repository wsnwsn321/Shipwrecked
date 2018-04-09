using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairColor : MonoBehaviour {

	private Camera newCamSpot;
	//public GameObject impactEffect;
	private float range = 100f;
	private LayerMask hitMask;
	public Renderer rend;
	

	// Use this for initialization
	void Start () {
		newCamSpot = this.GetComponentInParent<Control> ().main_c;
		hitMask = this.GetComponentInParent<NewGun> ().hitMask;
		rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		ShootCast ();
		print (Time.time);
	}

	void ShootCast(){

		//gets camera for center of screen
		Vector3 rayOrigin = this.GetComponentInParent<Control> ().main_c.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 3.3f));

		//Debug.DrawRay(rayOrigin, newCamSpot.transform.forward, Color.red);
		RaycastHit hit;
		//if (Physics.Raycast (charLocation.transform.position + charOffset, camera.transform.forward, out hit, range, hitMask)) {
		if (Physics.Raycast (rayOrigin, newCamSpot.transform.forward, out hit, range, hitMask)) {	
			//Debug.DrawRay(charLocation.transform.position + charOffset, cameraLocation.transform.forward, Color.green);
			Debug.Log (hit.transform.name); //This will display what is hit by the raycast
			Enemy enemy = hit.transform.GetComponent<Enemy> ();
			if (hit.transform.name.Equals("Spider_Brain")) {
				rend.material.color = Color.red;
				print ("Color is red");
			} else {
				rend.material.color = Color.grey;
				print ("Color is grey");
			}
			//GameObject impactGO = Instantiate (impactEffect, hit.point, Quaternion.LookRotation (hit.normal));
			//Destroy (impactGO, 1f);

		}


	}


}
