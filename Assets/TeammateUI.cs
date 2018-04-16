using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeammateUI : Photon.PunBehaviour {

	private UIManager ui;
	private PhotonView p;

	public void InitializeManager() {
		ui = GameObject.Find ("UIManager").GetComponent<UIManager> ();
		if (ui == null) {
			Debug.Log ("UI was not found!");
		} else {
			Debug.Log ("UI was found. Initializing...");
			ui.InitializeUI ();
		}
	}


	public void HealthChanged() {

		// Handles the RPC for the UI updating
		photonView.RPC ("UpdateTeammateUI", PhotonTargets.Others, null);
	}

	[PunRPC]
	public void UpdateTeammateUI() {
		if (ui == null) {
			InitializeManager ();
		}
		ui.UpdateUI ();
	}

}
