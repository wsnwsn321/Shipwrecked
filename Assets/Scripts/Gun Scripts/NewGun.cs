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

    // Core Control Modifiers
    private CoreControl core;
    private float baseDamage;

	private Vector3 charOffset = new Vector3 (-3.5f, -2f, 0f);

	private float nextTimeToFire = 0f;
	public GameObject crosshairPrefab;

	//For dynamic crosshair
	//private Ray ray = new Ray ();



	private PlayerManager playerManager;


	void Start(){
		camera = this.GetComponentInParent<Control> ().CamRef;
		crosshairPrefab = Instantiate (crosshairPrefab);
		newCamSpot = this.GetComponentInParent<Control> ().main_c;

        core = GetComponentInParent<CoreControl>();
        TeammateTypes characterType = GetComponentInParent<EntityType>().teammateType;
        print(characterType);
        switch (characterType)
        {
            case TeammateTypes.Captain:
                baseDamage = 3f;
                break;
            case TeammateTypes.Doctor:
                baseDamage = 5f;
                break;
            case TeammateTypes.Engineer:
                baseDamage = 30f;
                break;
            case TeammateTypes.Sergeant:
                baseDamage = 5f;
                break;
        }
        print("Base damage = " + baseDamage);
        print("Damage modifier = " + core.damageModifier);
	}

	void Update () {


			//add rid of GetButtonDown to make it fire on click
			//add GetButton to fire automatically on click and hold

			//this if checks if the player has remaining ammo
		if (!gameObject.GetComponentInParent<CoreControl> ().IsReloading ()&& !gameObject.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Die")) {
			if (gameObject.GetComponentInParent<AmmoRemaining> ().ammo > 0 && gameObject.GetComponentInParent<AmmoRemaining>().playerType.Equals("Sergeant")) {
				if (Input.GetButtonDown ("Fire1") && Time.time >= nextTimeToFire) {
					nextTimeToFire = Time.time + 2f / fireRate;
					Shoot ();
				}
			}
			else if (gameObject.GetComponentInParent<AmmoRemaining> ().ammo > 0 && gameObject.GetComponentInParent<AmmoRemaining>().playerType.Equals("Doctor")) {
				if (Input.GetButtonDown ("Fire1") && Time.time >= nextTimeToFire) {
					nextTimeToFire = Time.time + 2f / fireRate;
					Shoot ();
				}
			}
			else if (gameObject.GetComponentInParent<AmmoRemaining> ().ammo > 0 && gameObject.GetComponentInParent<AmmoRemaining>().playerType.Equals("Mechanic")) {
				if (Input.GetButtonDown ("Fire1") && Time.time >= nextTimeToFire) {
					nextTimeToFire = Time.time + 2f / fireRate;
					Shoot ();
				}
			}
			else if (gameObject.GetComponentInParent<AmmoRemaining> ().ammo > 0 && gameObject.GetComponentInParent<AmmoRemaining>().playerType.Equals("Captain")) {
				if (Input.GetButton ("Fire1") && Time.time >= nextTimeToFire) {
					nextTimeToFire = Time.time + 2f / fireRate;
					Shoot ();
				}
			}
		}
	}
		

	void Shoot(){

		//Call shot fired in AmmoRemaining script
		gameObject.GetComponent<AmmoRemaining>().shotFired();

		//gets camera for center of screen
		Vector3 rayOrigin = this.GetComponentInParent<Control> ().main_c.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 3.3f));

		//Debug.DrawRay(rayOrigin, newCamSpot.transform.forward, Color.red);
		RaycastHit hit;
		//if (Physics.Raycast (charLocation.transform.position + charOffset, camera.transform.forward, out hit, range, hitMask)) {
		if(Physics.Raycast(rayOrigin, newCamSpot.transform.forward, out hit, range, hitMask)){	
			//Debug.DrawRay(charLocation.transform.position + charOffset, cameraLocation.transform.forward, Color.green);
			Debug.Log(hit.transform.name); //This will display what is hit by the raycast
			Enemy enemy = hit.transform.GetComponent<Enemy> ();
			if (!enemy) {
				enemy = hit.transform.GetComponentInParent<Enemy> ();
			}
			if (enemy != null) {
				enemy.TakeDamage (baseDamage * core.damageModifier);
                enemy.AddAttacker(transform.parent);
			}
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
