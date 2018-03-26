using UnityEngine;
using UnityEngine.UI;

public class AmmoRemaining : MonoBehaviour {

	public int ammo;
	public int ammoPerShot = 1;

	private Text ammoText;


	// Use this for initialization
	void LateStart () {
		ammoText = GameObject.FindGameObjectWithTag("AmmoText").GetComponent<Text>();
		if (gameObject.GetComponentInParent<EntityType> ().teammateType.Equals("Sergeant")) {
			ammo = 20;
		}
		else if (gameObject.GetComponentInParent<EntityType> ().teammateType.Equals("Engineer")) {
			ammo = 1;
		}
		else if(gameObject.GetComponentInParent<EntityType> ().teammateType.Equals("Doctor")) {
			ammo = 20;
		}
		else if(gameObject.GetComponentInParent<EntityType> ().teammateType.Equals("Captain")) {
			ammo = 15;
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
