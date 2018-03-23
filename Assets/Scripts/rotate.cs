using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class rotate : MonoBehaviour {
	private float wantedRotAngle = 0;
	private float startRot = 0;
	public bool butr = false;
	public bool butl = false;
	// Use this for initialization
	void Start () {
		startRot = transform.rotation.eulerAngles.y;
	}
	
	// Update is called once per frame
	void Update () {

		transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (new Vector3 (0, wantedRotAngle, 0)), 2*Time.deltaTime);

		if (Input.GetKeyDown (KeyCode.LeftArrow)||butl) {
				wantedRotAngle = startRot + 90 ;
				startRot = startRot+90;
			butl = false;
		}	
		
		if (Input.GetKeyDown (KeyCode.RightArrow)||butr) {
			wantedRotAngle = startRot - 90 ;
			startRot = startRot-90;
			butr = false;
	}

}


}
