using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TeammateUI : Photon.PunBehaviour {

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
			return instance;
		}
	}
			
	public void InitializeManager() {
		ui = GameObject.Find ("UIManager").GetComponent<UIManager> ();
		ui.InitializeUI ();
	}


	public void HealthChanged() {

		// Handles the RPC for the UI updating
		this.photonView.RPC ("UpdateTeammateUI", PhotonTargets.Others, null);
	}

	[PunRPC]
	public void UpdateTeammateUI() {
		if (ui == null) {
			InitializeManager ();
		}
		ui.UpdateUI ();
	}

}
