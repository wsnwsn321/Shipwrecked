using UnityEngine;
using UnityEngine.UI;

public class AmmoRemaining : MonoBehaviour {

	public int ammo;
	public int ammoPerShot = 1;
	public EntityType type;

	private Text ammoText;


	// Use this for initialization
	void LateStart () {
		ammoText = GameObject.FindGameObjectWithTag("AmmoText").GetComponent<Text>();
	
		type = GetComponentInParent<EntityType> ();
		switch (type.teammateType) {
		case TeammateTypes.Sergeant:
			ammo = 15;
			break;
		case TeammateTypes.Doctor:
			ammo = 20;
			break;
		case TeammateTypes.Engineer:
			ammo = 1;
			break;
		case TeammateTypes.Captain:
			ammo = 20;
			break;
		}

		print (type.teammateType);

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
