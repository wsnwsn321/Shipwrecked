using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPickupItem : MonoBehaviour {

	void OnTriggerEnter(Collider collider){

		if (collider.gameObject.layer != 10) //Character layer
			return;

		PickUp (collider.transform);
		Destroy (gameObject);

	}

	public virtual void OnPickup(Transform item){
		//nothing for now

	}

	void PickUp(Transform item) {
		OnPickup (item);
	}

}
