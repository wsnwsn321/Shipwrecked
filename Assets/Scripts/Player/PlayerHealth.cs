using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Photon.PunBehaviour {

    public int earthDwellerDmg = 20;

	private float health = 100;
    private Text healthText;
    private Slider healthBar;
	private CoreControl corecontrol;
	private GameObject peace;
	private bool healthChanged = false;

    public EnemyAttackType enemyAttackType = EnemyAttackType.NONE;

    //Enemy type with corresponding attack values (how much damage is done to player)
    public enum EnemyAttackType
    {
        NONE = 0,
        CRAB_ALIEN = 10,
        SPIDER_BRAIN = 20
    };


	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (health);
		} else if (stream.isReading) {
			health = (float)stream.ReceiveNext ();
			healthChanged = true;
		}
	}

    // Update is called once per frame
    void Update () {
		// This is required in Update instead of LateStart since the prefab
		// is instantiated, not loaded in with the scene

		// Uses boolean logic of && instead of || to reduce checks per update
		if (!(healthText != null && healthBar != null && corecontrol != null && peace != null)) {
			corecontrol = GetComponent<CoreControl> ();
			healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<Text>();
			healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
			peace = transform.GetChild (5).gameObject;
		}

		// Damage is only taken for the local player to ensure damage doesn't stack incorrectly
        if (enemyAttackType != EnemyAttackType.NONE)
        {
			if (this.gameObject.Equals (PlayerManager.LocalPlayerInstance)) {
				TakeDamage ((float)enemyAttackType);
			}
			enemyAttackType = EnemyAttackType.NONE;
        }



		// Only happens for the local player
		if (healthChanged && (!PhotonNetwork.connected || this.gameObject.Equals(PlayerManager.LocalPlayerInstance))) {
			if (PhotonNetwork.connected) {
				// Only necessary in multiplayer
				PhotonNetwork.player.SetScore ((int)health);
				TeammateUI.Instance.HealthChanged();
			}
			updateHealthText ();
			updateHealthBar ();
			healthChanged = false;
		}
	
	}

	private void updateHealthText()
    {
		healthText.text = Mathf.Floor ((int)health).ToString () + "/100";
    }

	private void updateHealthBar()
    {
		healthBar.value = health;
    }

	public float GetHealth() {
		return health;
	}

	public void TakeDamage(float damage) {
		health -= damage;
		if (health <= 0) {
			health = 0;
		}
		healthChanged = true;
	}

	public void RecoverHealth(float rec) {
		health += rec;
		if (health > 100.0f) {
			health = 100;
		}
		healthChanged = true;
	}

	public void RecoverOrRevive(float rec) {
		if (health > 0) {
			RecoverHealth (rec);
		} else {
			corecontrol.Revived ();
		}
	}

    private void Die()
    {
        //player dies
		corecontrol.DieOnGround();
    }
		
				
}
