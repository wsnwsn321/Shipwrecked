using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public int health = 100;
    public int earthDwellerDmg = 20;

    private Text healthText;
    private Slider healthBar;
    private bool tookDmg = false;
	CoreControl corecontrol;
    private float timeColliding;
    public float timeThreshold = 1f;

    public static EnemyAttackType enemyAttackType = EnemyAttackType.NONE;

    //Enemy type with corresponding attack values (how much damage is done to player)
    public enum EnemyAttackType
    {
        NONE = 0,
        CRAB_ALIEN = 20,
        SPIDER_BRAIN = 10
    };

    // Use this for initialization
    void Start () {
        healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<Text>();
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
		corecontrol = GetComponent<CoreControl> ();
			}
	
	// Update is called once per frame
	void Update () {
        if (enemyAttackType != EnemyAttackType.NONE)
        {
            updateHealthText();
            updateHealthBar();
            enemyAttackType = EnemyAttackType.NONE;
        }
        checkHealth();
	}

    private void updateHealthText()
    {
        health -= (int)enemyAttackType;
        /*if (health < 0)
        {
            health = 0;
        }*/
        healthText.text = health.ToString() + "/100";
    }

    private void updateHealthBar()
    {
        healthBar.value = health;
    }

    private void checkHealth()
    {
        if (health <= 0)
        {
            //player dies
			corecontrol.DieOnGround();
		}

	

    }
}
