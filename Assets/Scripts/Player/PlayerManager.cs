using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Photon.PunBehaviour {

	[Tooltip("The local player instance. Use this to know if the local player is represented in the scene.")]
	public static GameObject LocalPlayerInstance;

	[Tooltip("The camera object to be attached to the player.")]
	public GameObject cameraObject;


	// Player shoot contains the code for firing your weapon
	private PlayerShoot playerShoot;

	// Cam is a reference to the Camera prefab
	private GameObject cam;

	void Awake() {

		// #Critical
		// we flag as don't destroy on load so that instance survives level sync
		DontDestroyOnLoad(this.gameObject);
	}

	// Use this for initialization
	void Start () {
		//#Important
		// Do this only for the local player
		if (photonView.isMine || !PhotonNetwork.connected) {
			PlayerManager.LocalPlayerInstance = this.gameObject;
			// Instantiate the player camera
			cam = GameObject.Instantiate(cameraObject, this.gameObject.transform);
			cam.transform.position = this.gameObject.transform.position + new Vector3(0.71f, 0f, -0.22f);
			this.GetComponent<FreeLookCam> ().enabled = true;
			this.GetComponentInChildren<NewGun> ().enabled = true;
			this.gameObject.GetComponent<Control> ().main_c = cam.GetComponentInChildren<Camera>();
			this.gameObject.GetComponent<Control> ().CamRef = cam;
		}
	}

	// Update is called once per frame
	void Update () {
		// We only want to update our character! Added on 2/6/18
		if (photonView.isMine == false && PhotonNetwork.connected) {
			return;
		}
	}
}
