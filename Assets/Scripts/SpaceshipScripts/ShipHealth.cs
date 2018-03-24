using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipHealth : Photon.MonoBehaviour {

	public int health = 1000;
	public int earthDwellerDmg = 10;

	private Text healthText;
	private Slider healthBar;
	private bool tookDmg = false;

	private float timeColliding;
	public float timeThreshold = 1f;

	// Use this for initialization
	void Start () {
		healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<Text>();
		healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
	}

	// Update is called once per frame
	void Update () {
		if (tookDmg)
		{
			updateHealthText();
			updateHealthBar();
		}
		checkHealth();
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (health);
		} else if (stream.isReading) {
			health = (int)stream.ReceiveNext ();
		}
	}


	private void OnCollisionStay(Collision collision)
	{
		switch (collision.gameObject.tag)
		{
		case "EarthDweller":
			if (timeColliding < timeThreshold)
			{
				timeColliding += Time.deltaTime;
			} else if (health > 0)
			{                    
				health -= earthDwellerDmg;
				tookDmg = true;
				timeColliding = 0f;
			}
			break;
		default:
			// Players bring items to the ship
			break;
		}

	}

	private void updateHealthText()
	{
		healthText.text = health.ToString() + "/1000";
	}

	private void updateHealthBar()
	{
		healthBar.value = health;
	}

	private void checkHealth()
	{
		if (health <= 0)
		{
			// Players lose
		}
	}
}
