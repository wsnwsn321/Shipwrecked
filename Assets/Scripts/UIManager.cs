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


	void Update() {
		if (teammates == null) {
			teammates = PhotonNetwork.otherPlayers;
			InitializeUI ();
		}
		if (updateUI) {
			HealthChanged ();
			updateUI = false;
		}
	}

	void InitializeUI() {
		int numberOfTeammates = teammates.Length;
		Debug.Log ("Teammate Count: " + numberOfTeammates);
		switch (numberOfTeammates) {
		case 3:
			teammateThreeName.gameObject.SetActive(true);
			teammateThreeHealth.enabled = true;
			teammateTwoName.enabled = true;
			teammateTwoHealth.enabled = true;
			teammateOneName.enabled = true;
			teammateOneHealth.enabled = true;
			break;
		case 2:
			teammateThreeName.gameObject.SetActive(false);
			teammateThreeHealth.enabled = false;
			teammateTwoName.enabled = true;
			teammateTwoHealth.enabled = true;
			teammateOneName.enabled = true;
			teammateOneHealth.enabled = true;
			break;
		case 1:
			teammateThreeName.gameObject.SetActive(false);
			teammateThreeHealth.enabled = false;
			teammateTwoName.enabled = false;
			teammateTwoHealth.enabled = false;
			teammateOneName.enabled = true;
			teammateOneHealth.enabled = true;
			break;
		default:
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
