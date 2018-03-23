using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : Photon.MonoBehaviour {

	[SerializeField]Shooter assaultRifle;

	void Update(){
		// We only want to update our character! Added on 2/6/18
		if (photonView.isMine == false && PhotonNetwork.connected == true) {
			return;
		}

		if (GameManager.Instance.InputController.Fire1) {
			assaultRifle.Fire ();
		}
	}

}
