using UnityEngine;
using UnityEngine.UI;

public class AmmoRemaining : MonoBehaviour {

	public int ammo;
	public int ammoPerShot = 1;
	private int originalAmmo;
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
			ammo = 15;
			playerType = "Sergeant";
			break;
		case "DoctorGun":
			ammo = 12;
			playerType = "Doctor";
			break;
		case "MechanicGun":
			ammo = 1;
			playerType = "Mechanic";
			break;
		case "CaptainGun":
			ammo = 20;
			playerType = "Captain";
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
		ammo = originalAmmo;
	}
}
