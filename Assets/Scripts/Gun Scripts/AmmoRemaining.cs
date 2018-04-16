using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using PlayerAbilities;
using UnityEngine;

public class AmmoRemaining : Photon.MonoBehaviour {

	public int ammo;
	public int ammoPerShot = 1;
	private int originalAmmo;
	public string playerType;
	public float reloadTime;
	private Text ammoText;
    [HideInInspector]
    public bool isReloading;
	public bool isFlaming;
	private bool isCaptain = false;
	private bool updateAmmo = false;

	public AudioClip shootingAudio;
	public AudioClip reloadAudio;
	public AudioClip captainShootingAudio;

	// Use this for initialization
	void LateStart () {
		//ammoText = GameObject.FindGameObjectWithTag("AmmoText").GetComponent<Text>();
	
	}

	void Start(){
		if (photonView.isMine || !PhotonNetwork.connected) {
			updateAmmo = true;
			string type = gameObject.tag;
			switch (type) {
			case "SargeGun":
				ammo = 15;
				playerType = "Sergeant";
				reloadTime = 2f;
				break;
			case "DoctorGun":
				ammo = 12;
				playerType = "Doctor";
				reloadTime = 2f;
				break;
			case "MechanicGun":
				ammo = 2;
				playerType = "Mechanic";
				reloadTime = 4f;
				break;
			case "CaptainGun":
				ammo = 20;
				playerType = "Captain";
				reloadTime = 2.5f;
				isCaptain = true;
				break;
			default:
				print ("Ope");
				break;
			}
			originalAmmo = ammo;
			isReloading = false;
			SetAmmoText ();
		}
	}

	// Update is called once per frame
	void Update () {
		if(updateAmmo) {
		updateAmmoText();
		if (isCaptain) {
			isFlaming = gameObject.GetComponentInParent<CaptainControl> ().isFlaming;
		}
		//checkAmmo();
		}
	}

	public void shotFired(){
		ammo -= 1;
		if (isFlaming) {
			AudioSource.PlayClipAtPoint (captainShootingAudio, transform.position, 1);
		} else if (shootingAudio) {
			AudioSource.PlayClipAtPoint (shootingAudio, transform.position, 1);
		}
	}

	private void updateAmmoText()
	{
		if (ammoText != null) {
			ammoText.text = "Ammo: " + ammo.ToString () + "/" + originalAmmo.ToString ();
		} else {
			SetAmmoText ();
			//ammoText = GameObject.FindGameObjectWithTag("AmmoText").GetComponent<Text>();
		}
	}

	private void SetAmmoText() {
		ammoText = GameObject.FindGameObjectWithTag ("AmmoText").GetComponent<Text>();
		if (ammoText == null) {
			Debug.Log ("Error retrieving ammo text in UI using its tag!");
			ammoText = GameObject.Find ("AmmoText").GetComponent<Text> ();
			if (ammoText != null) {
				Debug.Log ("Ammo text was successfully retrieved using GameObject.Find method!");
			}
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
		if (reloadAudio) {
			AudioSource.PlayClipAtPoint (reloadAudio, transform.position, 1);
		}
	}

	IEnumerator WaitForReload()
	{
        isReloading = true;
		yield return new WaitForSeconds(reloadTime);
        isReloading = false;
		ammo = originalAmmo;
	}
}
