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

        MonsterSpawnManager msm = GameObject.FindGameObjectWithTag("MonsterSpawnManager").GetComponent<MonsterSpawnManager>();
        int originalMaxMonsters = msm.originalMaxMonsters;

        float brainHealthIncrease = 20 * slot + 30;
        float critterHealthIncrease = 15 * slot + 25;

        IncreaseBrainStats(brainHealthIncrease, (slot + 1) * 2);
        IncreaseCritterStats(critterHealthIncrease, slot + 1);

        msm.timeBetweenSpawns -= 1f;

        switch (slot)
        {
		    case 0:
			    Image slotImage = GameObject.Find ("Slot0").GetComponent<Image> ();
                slotImage.sprite = shipPartImage;
                msm.maxNumberMonsters = (int)(1.2f * originalMaxMonsters);
                break;
            case 1:
                slotImage = GameObject.Find("Slot1").GetComponent<Image>();
                slotImage.sprite = shipPartImage;
                msm.maxNumberMonsters = (int)(1.5f * originalMaxMonsters);
                break;
            case 2:
                slotImage = GameObject.Find("Slot2").GetComponent<Image>();
                slotImage.sprite = shipPartImage;
                msm.maxNumberMonsters = (int)(1.8f * originalMaxMonsters);
                break;
            case 3:
                slotImage = GameObject.Find("Slot3").GetComponent<Image>();
                slotImage.sprite = shipPartImage;
                msm.maxNumberMonsters = (int)(2.1f * originalMaxMonsters);
                break;
            case 4:
                slotImage = GameObject.Find("Slot4").GetComponent<Image>();
                slotImage.sprite = shipPartImage;
                msm.maxNumberMonsters = (int)(2.5f * originalMaxMonsters);
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

    void IncreaseCritterStats(float health, float damage)
    {
        EnemyStats.Critter.Health += health;
        EnemyStats.Critter.Damage += damage;
    }

    void IncreaseBrainStats(float health, float damage)
    {
        EnemyStats.Brain.Health += health;
        EnemyStats.Brain.Damage += damage;
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
