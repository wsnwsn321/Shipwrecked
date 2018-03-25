using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Photon.PunBehaviour {

	private PhotonPlayer[] teammates;

	[HideInInspector]
	public static bool updateUI;

	public Text teammateOneName;
	public Text teammateTwoName;
	public Text teammateThreeName;

	public Slider teammateOneHealth;
	public Slider teammateTwoHealth;
	public Slider teammateThreeHealth;


	// Use this for initialization
	void LateStart () {
		teammates = PhotonNetwork.otherPlayers;
		InitializeUI ();
	}

	void InitializeUI() {
		int numberOfTeammates = teammates.Length;
		switch (numberOfTeammates) {
		case 3:
			teammateThreeName.enabled = true;
			teammateThreeHealth.enabled = true;
			teammateTwoName.enabled = true;
			teammateTwoHealth.enabled = true;
			teammateOneName.enabled = true;
			teammateOneHealth.enabled = true;
			break;
		case 2:
			teammateTwoName.enabled = true;
			teammateTwoHealth.enabled = true;
			teammateOneName.enabled = true;
			teammateOneHealth.enabled = true;
			break;
		case 1:
			teammateOneName.enabled = true;
			teammateOneHealth.enabled = true;
			break;
		default:
			break;
		}
	}

	void Update() {
		if (updateUI) {
			HealthChanged ();
			updateUI = false;
		}
	}


	public void HealthChanged() {
		this.photonView.RPC ("UpdateUI", PhotonTargets.Others, null);
	}

	[PunRPC]
	private void UpdateUI () {
		// Updates the UI for other teammates, aka "Teammate Health bars"
		int numberOfTeammates = teammates.Length;
		switch (numberOfTeammates) {
		case 3:
			teammateThreeName.text = teammates [2].NickName;
			teammateThreeHealth.value = teammates [2].GetScore ();
			teammateTwoName.text = teammates [1].NickName;
			teammateTwoHealth.value = teammates [1].GetScore ();
			teammateOneName.text = teammates [0].NickName;
			teammateOneHealth.value = teammates [0].GetScore ();
			break;
		case 2:
			teammateTwoName.text = teammates [1].NickName;
			teammateTwoHealth.value = teammates [1].GetScore ();
			teammateOneName.text = teammates [0].NickName;
			teammateOneHealth.value = teammates [0].GetScore ();
			break;
		case 1:
			teammateOneName.text = teammates [0].NickName;
			teammateOneHealth.value = teammates [0].GetScore ();
			break;
		default:
			break;
		}
	}



}
