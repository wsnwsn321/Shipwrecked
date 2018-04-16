using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PingViewer : Photon.MonoBehaviour {

	private Text pingText;
	private int updateCounter = 0;
	private int ping = 0;

	void Start() {
		pingText = gameObject.GetComponent<Text> ();
		pingText.text = "Ping: " + ping.ToString()+ " ms";
	}

	// Update is called once per frame
	void FixedUpdate () {
		updateCounter++;
		if (updateCounter > 20) {
			updateCounter = 0;
			ping = PhotonNetwork.GetPing ();
			if (ping < 70) {
				pingText.color = Color.green;
			} else if (ping < 120) {
				pingText.color = Color.yellow;
			} else {
				if (ping > 999) {
					ping = 999;
				} 
				pingText.color = Color.red;
			}
			pingText.text = "Ping: " + ping.ToString () + " ms";
		}
	}
}
