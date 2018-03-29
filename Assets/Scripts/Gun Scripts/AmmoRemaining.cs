﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using PlayerAbilities;
using UnityEngine;

public class AmmoRemaining : MonoBehaviour {

	public int ammo;
	public int ammoPerShot = 1;
	private int originalAmmo;
	public string playerType;
	public float reloadTime;
	private Text ammoText;


	// Use this for initialization
	void LateStart () {
		ammoText = GameObject.FindGameObjectWithTag("AmmoText").GetComponent<Text>();
	
	}

	void Start(){
		string type = gameObject.tag;
		switch (type) {
		case "SargeGun":
			ammo = 15;
			playerType = "Sergeant";
			reloadTime = 1.5f;
			break;
		case "DoctorGun":
			ammo = 12;
			playerType = "Doctor";
			reloadTime = 1f;
			break;
		case "MechanicGun":
			ammo = 2;
			playerType = "Mechanic";
			reloadTime = 3f;
			break;
		case "CaptainGun":
			ammo = 20;
			playerType = "Captain";
			reloadTime = 2f;
			break;
		default:
			print ("Ope");
			break;
		}
		originalAmmo = ammo;
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
			ammoText.text = ammo.ToString () + "/" + originalAmmo.ToString ();
		} else {
			ammoText = GameObject.FindGameObjectWithTag("AmmoText").GetComponent<Text>();
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
		StartCoroutine(WaitForReload ());
	}

	IEnumerator WaitForReload()
	{
		yield return new WaitForSeconds(reloadTime);
		ammo = originalAmmo;
	}
}
