using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Photon.MonoBehaviour {

    public float health = 100;
    public int earthDwellerDmg = 20;

    private Text healthText;
    private Slider healthBar;
    private bool tookDmg = false;
	CoreControl corecontrol;
    private float timeColliding;
    public float timeThreshold = 1f;
	private GameObject peace;

    public EnemyAttackType enemyAttackType = EnemyAttackType.NONE;

    //Enemy type with corresponding attack values (how much damage is done to player)
    public enum EnemyAttackType
    {
        NONE = 0,
        CRAB_ALIEN = 10,
        SPIDER_BRAIN = 20
    };

    // Update is called once per frame
    void Update () {
		// This is required in Update instead of LateStart since the prefab
		// is instantiated, not loaded in with the scene
		if (healthText == null || healthBar == null || corecontrol == null) {
			corecontrol = GetComponent<CoreControl> ();
			healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<Text>();
			healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
		}
        if (enemyAttackType != EnemyAttackType.NONE)
        {
            updateHealthText();
            updateHealthBar();
            enemyAttackType = EnemyAttackType.NONE;
        }
        checkHealth();

		//for the second ability of doctor to use
		if (PlayerManager.LocalPlayerInstance.Equals (this.gameObject)) {
			peace = transform.GetChild (5).gameObject;
			if (peace.activeSelf) {
				health += 0.1025f;
				updateHealthText ();
				updateHealthBar ();
			}
		}
	}

	public void updateHealthText()
    {
		//if (photonView.isMine) {
		health -= (int)enemyAttackType;
		enemyAttackType = EnemyAttackType.NONE;

		if (health < 0) {
			health = 0;
		} else if (health > 100) {
			health = 100;
		}
		if (PhotonNetwork.connected) {
			// Only necessary in multiplayer
			PhotonNetwork.player.SetScore ((int)health);
			UIManager.updateUI = true;
		}
		healthText.text = Mathf.Floor (health).ToString () + "/100";
		//}
    }

	public void updateHealthBar()
    {
		//if (photonView.isMine) {
		healthBar.value = health;
		//}
    }

    private void checkHealth()
    {
        if (health <= 0)
        {
            //player dies
			corecontrol.DieOnGround();
			this.gameObject.layer = 16;
		}
    }
		
}
