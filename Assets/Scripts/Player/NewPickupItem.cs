using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPickupItem : Photon.PunBehaviour {

	void OnTriggerEnter(Collider collider){

		if (collider.gameObject.layer != 10) //Character layer
			return;

		PickUp (collider.transform);
		Destroy (gameObject);

	}

	public virtual void OnPickup(Transform item){
		//nothing for now

	}

	[PunRPC]
	void PickUp(Transform item) {
		if (PhotonNetwork.connected) {
			photonView.RPC ("OnPickup", PhotonTargets.All, item);
		} else {
			OnPickup (item);
		}
	}

}
