﻿using UnityEngine;
using UnityEngine.UI;

public class AmmoRemaining : MonoBehaviour {

	public int ammo;
	public int ammoPerShot = 1;
	public string playerType;

	private Text ammoText;


	// Use this for initialization
	void LateStart () {
		ammoText = GameObject.FindGameObjectWithTag("AmmoText").GetComponent<Text>();
	
	}

	void Start(){
		string type = gameObject.tag;
		switch (type) {
		case "SargeGun":
			ammo = 20;
			playerType = "Sergeant";
			break;
		case "DoctorGun":
			ammo = 15;
			playerType = "Doctor";
			break;
		case "MechanicGun":
			ammo = 1;
			playerType = "Mechanic";
			break;
		case "CaptainGun":
			ammo = 10;
			playerType = "Captain";
			break;
		default:
			print ("Ope");
			break;
		}
	}

	// Update is called once per frame
	void Update () {
		updateAmmoText();
		//checkAmmo();
	}

	public void shotFired(){
		ammo -= 1;
	}

	private void updateAmmoText()
	{
		if (ammoText != null) {
			ammoText.text = ammo.ToString () + "/20";
		}
	}

	private void checkAmmo()
	{
		if (ammo <= 0)
		{
			reload ();
		}
	}

	public void reload(){
		ammo = 20;
	}
}
