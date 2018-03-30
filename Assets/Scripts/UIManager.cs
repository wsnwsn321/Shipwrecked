﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Photon.PunBehaviour {

	private PhotonPlayer[] teammates;

	[HideInInspector]
	public static bool updateUI = false;

	public Text teammateOneName;
	public Text teammateTwoName;
	public Text teammateThreeName;

	public Slider teammateOneHealth;
	public Slider teammateTwoHealth;
	public Slider teammateThreeHealth;


	void Update() {
		// Need to consider if otherPlayers will exist w/ connected check
		if (teammates == null && PhotonNetwork.connected) {
			teammates = PhotonNetwork.otherPlayers;
			InitializeUI ();
		}
		// It will never enter this statement 
		if (updateUI) {
			HealthChanged ();
			updateUI = false;
		}
	}

	void InitializeUI() {
		int numberOfTeammates = 0;
		if(teammates != null) {
			numberOfTeammates = teammates.Length;
		}
		Debug.Log ("Teammate Count: " + numberOfTeammates);
		switch (numberOfTeammates) {
		case 3:
			// Three teammates, view 3 total teammate health bars
			teammateThreeName.gameObject.SetActive (true);
			teammateThreeHealth.gameObject.SetActive (true);
			teammateTwoName.gameObject.SetActive (true);
			teammateTwoHealth.gameObject.SetActive (true);
			teammateOneName.gameObject.SetActive (true);
			teammateOneHealth.gameObject.SetActive (true);
			break;
		case 2:
			teammateThreeName.gameObject.SetActive (false);
			teammateThreeHealth.gameObject.SetActive (false);
			// Two teammates, view 2 total teammate health bars
			teammateTwoName.gameObject.SetActive (true);
			teammateTwoHealth.gameObject.SetActive (true);
			teammateOneName.gameObject.SetActive (true);
			teammateOneHealth.gameObject.SetActive (true);
			break;
		case 1:
			teammateThreeName.gameObject.SetActive (false);
			teammateThreeHealth.gameObject.SetActive (false);
			teammateTwoName.gameObject.SetActive (false);
			teammateTwoHealth.gameObject.SetActive (false);
			// One teammate, view 1 total teammate health bar
			teammateOneName.gameObject.SetActive (true);
			teammateOneHealth.gameObject.SetActive (true);
			break;
		default:
			// View no teammate health bars
			teammateThreeName.gameObject.SetActive (false);
			teammateThreeHealth.gameObject.SetActive (false);
			teammateTwoName.gameObject.SetActive (false);
			teammateTwoHealth.gameObject.SetActive (false);
			teammateOneName.gameObject.SetActive (false);
			teammateOneHealth.gameObject.SetActive (false);
			break;
		}
	}



	public void HealthChanged() {
		// Handles the RPC for the UI updating
		this.photonView.RPC ("UpdateUI", PhotonTargets.Others, null);
	}

	[PunRPC]
	private void UpdateUI () {
		// Updates the UI for other teammates, aka "Teammate Health bars"
		teammates = PhotonNetwork.otherPlayers;
		int numberOfTeammates = teammates.Length;
		switch (numberOfTeammates) {
		case 3:
			teammateThreeName.text = teammates [2].NickName;
			teammateThreeHealth.value = (float)teammates [2].GetScore ();
			teammateTwoName.text = teammates [1].NickName;
			teammateTwoHealth.value = (float)teammates [1].GetScore ();
			teammateOneName.text = teammates [0].NickName;
			teammateOneHealth.value = (float)teammates [0].GetScore ();
			break;
		case 2:
			teammateTwoName.text = teammates [1].NickName;
			teammateTwoHealth.value = (float)teammates [1].GetScore ();
			teammateOneName.text = teammates [0].NickName;
			teammateOneHealth.value = (float)teammates [0].GetScore ();
			break;
		case 1:
			teammateOneName.text = teammates [0].NickName;
			teammateOneHealth.value = (float)teammates [0].GetScore ();
			break;
		default:
			break;
		}
	}



}
