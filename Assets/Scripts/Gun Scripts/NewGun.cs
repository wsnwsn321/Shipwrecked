using UnityEngine;

public class NewGun : PlayerManager {

	public float damage = 10f;
	public float range = 100f;
	public float fireRate = 15f;
	public GameObject impactEffect;
	private Camera newCamSpot;
	public GameObject charLocation;
	private GameObject camera;
	public LayerMask hitMask;
	public GameObject bloodSplatter;
    // Core Control Modifiers
    private CoreControl core;
    private float baseDamage;

	private Vector3 charOffset = new Vector3 (-3.5f, -2f, 0f);

	private float nextTimeToFire = 0f;
	public GameObject crosshairPrefab;

	private PlayerManager playerManager;



	void Start(){
		camera = this.GetComponentInParent<Control> ().CamRef;
		crosshairPrefab = Instantiate (crosshairPrefab);
		newCamSpot = this.GetComponentInParent<Control> ().main_c;

        core = GetComponentInParent<CoreControl>();
        TeammateTypes characterType = GetComponentInParent<EntityType>().teammateType;
        switch (characterType)
        {
            case TeammateTypes.Captain:
                baseDamage = 3f;
                fireRate = 20f;
                break;
            case TeammateTypes.Doctor:
                baseDamage = 5f;
                fireRate = 12f;
                break;
            case TeammateTypes.Engineer:
                baseDamage = 30f;
                fireRate = 1f;
                break;
            case TeammateTypes.Sergeant:
                baseDamage = 5f;
                fireRate = 15f;
                break;
        }

        core.fireRate = fireRate;
	}

	void Update () {


			//add rid of GetButtonDown to make it fire on click
			//add GetButton to fire automatically on click and hold

			//this if checks if the player has remaining ammo
		if (!gameObject.GetComponentInParent<CoreControl> ().IsReloading ()&& !gameObject.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Die")&& !gameObject.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("AB2")) {
			if (gameObject.GetComponent<AmmoRemaining> ().ammo > 0 && gameObject.GetComponent<AmmoRemaining>().playerType.Equals("Sergeant")&&!core.autoRifle) {
				if (Input.GetButtonDown ("Fire1") && Time.time >= nextTimeToFire) {
					nextTimeToFire = Time.time + 2f / fireRate;
					Shoot ();
				}
			}
			else if (gameObject.GetComponent<AmmoRemaining> ().ammo > 0 && gameObject.GetComponent<AmmoRemaining>().playerType.Equals("Doctor")) {
				if (Input.GetButtonDown ("Fire1") && Time.time >= nextTimeToFire) {
					nextTimeToFire = Time.time + 2f / fireRate;
					Shoot ();
				}
			}
			else if (gameObject.GetComponent<AmmoRemaining> ().ammo > 0 && gameObject.GetComponent<AmmoRemaining>().playerType.Equals("Mechanic")) {
				if (Input.GetButtonDown ("Fire1") && Time.time >= nextTimeToFire) {
					nextTimeToFire = Time.time + 2f / fireRate;
					Shoot ();
				}
			}
			else if (gameObject.GetComponent<AmmoRemaining> ().ammo > 0 && gameObject.GetComponent<AmmoRemaining>().playerType.Equals("Captain")||core.autoRifle) {
				if (Input.GetButton ("Fire1") && Time.time >= nextTimeToFire) {
					nextTimeToFire = Time.time + 2f / fireRate;
					Shoot ();
				}
			}
		}
	}
		

	void Shoot(){
			//Call shot fired in AmmoRemaining script
			gameObject.GetComponent<AmmoRemaining> ().shotFired ();

			//gets camera for center of screen
			Vector3 rayOrigin = this.GetComponentInParent<Control> ().main_c.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 3.3f));

		//Debug.DrawRay(rayOrigin, newCamSpot.transform.forward, Color.red);
		RaycastHit hit;
		//if (Physics.Raycast (charLocation.transform.position + charOffset, camera.transform.forward, out hit, range, hitMask)) {
		if (gameObject.GetComponent<AmmoRemaining> ().playerType.Equals ("Captain") && !gameObject.GetComponent<AmmoRemaining> ().isFlaming) {
			float xOffset = Random.Range (-0.07f, 0.07f);
			float yOffset = Random.Range (-0.07f, 0.07f);
			float zOffset = Random.Range (-0.07f, 0.07f);
			rayOrigin = this.GetComponentInParent<Control> ().main_c.ViewportToWorldPoint (new Vector3 (0.5f + xOffset, 0.5f + yOffset, 3.3f + zOffset));
			if (Physics.Raycast (rayOrigin, newCamSpot.transform.forward, out hit, range, hitMask)) {	
				//Debug.DrawRay(charLocation.transform.position + charOffset, cameraLocation.transform.forward, Color.green);
				Debug.Log (hit.transform.name); //This will display what is hit by the raycast
				Enemy enemy = hit.transform.GetComponent<Enemy> ();
				if (!enemy) {
					enemy = hit.transform.GetComponentInParent<Enemy> ();
				}
				if (enemy != null) {
					enemy.TakeDamage (baseDamage * core.damageModifier);
					enemy.AddAttacker (transform.parent);
				}

			}
		} else if (gameObject.GetComponent<AmmoRemaining> ().playerType.Equals ("Sergeant") || gameObject.GetComponent<AmmoRemaining> ().playerType.Equals ("Doctor")) {
			float xOffset = Random.Range (-0.05f, 0.05f);
			float yOffset = Random.Range (-0.05f, 0.05f);
			float zOffset = Random.Range (-0.05f, 0.05f);
			rayOrigin = this.GetComponentInParent<Control> ().main_c.ViewportToWorldPoint (new Vector3 (0.5f + xOffset, 0.5f + yOffset, 3.3f + zOffset));
			if (Physics.Raycast (rayOrigin, newCamSpot.transform.forward, out hit, range, hitMask)) {	
				//Debug.DrawRay(charLocation.transform.position + charOffset, cameraLocation.transform.forward, Color.green);
				Debug.Log (hit.transform.name); //This will display what is hit by the raycast
				Enemy enemy = hit.transform.GetComponent<Enemy> ();
				if (!enemy) {
					enemy = hit.transform.GetComponentInParent<Enemy> ();
				}
				if (enemy != null) {
					enemy.TakeDamage (baseDamage * core.damageModifier);
					enemy.AddAttacker (transform.parent);
				}

			}
		} else if (gameObject.GetComponentInParent<CoreControl>().aiming) {
			if (Physics.Raycast (rayOrigin, newCamSpot.transform.forward, out hit, range, hitMask)) {	
				//Debug.DrawRay(charLocation.transform.position + charOffset, cameraLocation.transform.forward, Color.green);
				Debug.Log (hit.transform.name); //This will display what is hit by the raycast
				Enemy enemy = hit.transform.GetComponent<Enemy> ();
				if (!enemy) {
					enemy = hit.transform.GetComponentInParent<Enemy> ();
				}
				if (enemy != null) {
					enemy.TakeDamage (baseDamage * core.damageModifier);
					enemy.AddAttacker (transform.parent);
				}
					
			}
		} else {
			if (Physics.Raycast (rayOrigin, newCamSpot.transform.forward, out hit, range, hitMask)) {	
				//Debug.DrawRay(charLocation.transform.position + charOffset, cameraLocation.transform.forward, Color.green);
				Debug.Log (hit.transform.name); //This will display what is hit by the raycast
				Enemy enemy = hit.transform.GetComponent<Enemy> ();
				if (!enemy) {
					enemy = hit.transform.GetComponentInParent<Enemy> ();
				}
				if (enemy != null) {
					enemy.TakeDamage (baseDamage * core.damageModifier);
					enemy.AddAttacker (transform.parent);
				}


			}
		}
		if (hit.transform.name.Equals ("AI_Crab_Alien(Clone)") || hit.transform.name.Equals ("Spider_Brain")) {
			GameObject bloodImpact = GameObject.Instantiate (bloodSplatter, hit.point, Quaternion.identity);
			Destroy (bloodImpact, 1f);
		} else {
			GameObject impactGO = Instantiate (impactEffect, hit.point, Quaternion.LookRotation (hit.normal));
			//impactGO.GetComponent<ParticleSystem> ().Play ();
			Destroy (impactGO, 1f);
		}

	}
		

	//Position the dynamic crosshair to the point that we are aiming
	void PositionCrosshair(Ray ray){
		RaycastHit laserhit;
		Physics.Raycast (charLocation.transform.position + charOffset, camera.transform.forward, out laserhit, Mathf.Infinity, hitMask);
		crosshairPrefab.transform.position = laserhit.point;
		crosshairPrefab.transform.LookAt (Camera.main.transform);

	}
		

}
