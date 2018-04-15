using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PartPickup : Photon.PunBehaviour {

    public Sprite shipPartImage;
    public static int slot = 0;

	[PunRPC]
	public void OnPickup ()
	{
		GameObject.Find ("MissionText").GetComponent<missionText> ().partNo = slot+1;
		GameObject.Find ("MissionText").GetComponent<missionText> ().hasMission = true;
        switch (slot)
        {
		case 0:
			Image slotImage = GameObject.Find ("Slot0").GetComponent<Image> ();
                slotImage.sprite = shipPartImage;
                break;
            case 1:
                slotImage = GameObject.Find("Slot1").GetComponent<Image>();
                slotImage.sprite = shipPartImage;
                break;
            case 2:
                slotImage = GameObject.Find("Slot2").GetComponent<Image>();
                slotImage.sprite = shipPartImage;
                break;
            case 3:
                slotImage = GameObject.Find("Slot3").GetComponent<Image>();
                slotImage.sprite = shipPartImage;
                break;
            case 4:
                slotImage = GameObject.Find("Slot4").GetComponent<Image>();
                slotImage.sprite = shipPartImage;
                break;
        }
        slot++;
		Experience.currentSlot++;
        if (slot >= 5)
        {
			SceneManager.LoadScene("EndScene");
        }
		Destroy (gameObject);
	}


	void OnTriggerEnter(Collider collider){

		if (collider.gameObject.layer != 10) //Character layer
			return;

		PickUp (collider.transform);

	}


	void PickUp(Transform item) {
		if (PhotonNetwork.connected) {
			photonView.RPC ("OnPickup", PhotonTargets.All, null);
		} else {
			OnPickup ();
		}
	}

}
