using UnityEngine;

public class NewRocketLauncher : PlayerManager {

	public float damage = 10f;
	public float range = 100f;
	public float fireRate = 50f;
	public GameObject impactEffect;
	private Camera newCamSpot;
	public GameObject charLocation;
	private GameObject camera;
	public LayerMask hitMask;

	private Vector3 charOffset = new Vector3 (0f, 1f, 0f);

	private float nextTimeToFire = 0f;
	public GameObject crosshairPrefab;
	private Ray ray = new Ray ();

	private PlayerManager playerManager;


	void Start(){
		camera = this.GetComponentInParent<Control> ().CamRef;
		crosshairPrefab = Instantiate (crosshairPrefab);
		newCamSpot = this.GetComponentInParent<Control> ().main_c;
	}

	void Update () {
		//add rid of GetButtonDown to make it fire on click
		if (Input.GetButtonDown ("Fire1") && Time.time >= nextTimeToFire) {
			nextTimeToFire = Time.time + 2f / fireRate;
			Shoot();
		}

		//PositionCrosshair(ray);
	}
		

	void Shoot(){
		//Debug.DrawRay(charLocation.transform.position + charOffset, cameraLocation.transform.forward, Color.red);
		Vector3 rayOrigin = this.GetComponentInParent<Control> ().main_c.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 3.3f));

		RaycastHit hit;
		if (Physics.Raycast (rayOrigin, newCamSpot.transform.forward, out hit, range, hitMask)) {
			
			//Debug.DrawRay(charLocation.transform.position + charOffset, cameraLocation.transform.forward, Color.green);

			Enemy enemy = hit.transform.GetComponent<Enemy> ();
			if (enemy != null) {
				enemy.TakeDamage (damage);
			}
			GameObject impactGO = Instantiate (impactEffect, hit.point, Quaternion.LookRotation (hit.normal));
			//impactGO.GetComponent<ParticleSystem> ().Play ();
			Destroy (impactGO, 1f);
		
		}

	}
		

	//Position the crosshair to the point that we are aiming
	void PositionCrosshair(Ray ray){
		RaycastHit laserhit;
		Physics.Raycast (charLocation.transform.position + charOffset, camera.transform.forward, out laserhit, Mathf.Infinity, hitMask);
		crosshairPrefab.transform.position = laserhit.point;
		crosshairPrefab.transform.LookAt (Camera.main.transform);

	}
		

}
