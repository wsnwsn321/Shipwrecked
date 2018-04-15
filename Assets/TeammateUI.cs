using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeammateUI : Photon.PunBehaviour {

	private UIManager ui;

	// MAKE THIS THING A SINGLETON
	private static volatile TeammateUI instance;
	private static object syncRoot = new Object();

	private TeammateUI() {}

	public static TeammateUI Instance {
		get {
			if (instance == null) {
				lock (syncRoot) {
					if (instance == null)
						instance = new TeammateUI ();
				}
			}
		}
	}
			
	public void InitializeManager() {
		ui = GameObject.Find ("UIManager").GetComponent<UIManager> ();
	}


	public void HealthChanged() {
		// Handles the RPC for the UI updating
		// RPC CALL DOESNT WORK cORRECTLY SinCE GAMEOBJECTS DONT MATCH
		this.photonView.RPC ("UpdateTeammateUI", PhotonTargets.Others, null);
	}

	[PunRPC]
	public void UpdateTeammateUI() {
		ui.UpdateUI ();
	}

}
