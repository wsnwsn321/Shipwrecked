﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipHealth : Photon.MonoBehaviour {

	public int health = 1000;
	public int earthDwellerDmg = 10;

    private int maxHealth;
	private Text healthText;
	private Slider healthBar;
	private bool tookDmg = false;

	private float timeColliding;
	public float timeThreshold = 1f;

    void Start()
    {
        maxHealth = 1000;
    }

    // Update is called once per frame
    void Update () {
		if (healthText == null) {
			healthText = GameObject.FindGameObjectWithTag("ShipHealthText").GetComponent<Text>();
			healthBar = GameObject.FindGameObjectWithTag("ShipHealthBar").GetComponent<Slider>();
		}
		if (tookDmg)
		{
			updateHealthText();
			updateHealthBar();
		}
		loseGame ();
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (health);
		} else if (stream.isReading) {
			health = (int)stream.ReceiveNext ();
		}
	}


	public void TakeDamage(int damage) {
		health -= damage;
		if (health <= 0) {
			loseGame ();
		}
		tookDmg = true;
	}

	private void updateHealthText()
	{
		healthText.text = health.ToString() + "/" + maxHealth;
	}

	private void updateHealthBar()
	{
		healthBar.value = health;
	}

	private void loseGame()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			health = 0;
		}
	}
}
