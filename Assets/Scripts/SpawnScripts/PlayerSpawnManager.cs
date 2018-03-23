using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : Photon.MonoBehaviour {

	[Tooltip("Spawn points available in the spawn zone.")]
	public GameObject[] spawnPoints;

	static string playerClassPrefKey = "pc";

	/// <summary>
	/// / Player prefabs
	/// </summary>
	[Tooltip("The Sarge prefab to spawn.")]
	public GameObject sargePrefab;

	[Tooltip("The Doc prefab to spawn.")]
	public GameObject docPrefab;

	[Tooltip("The Grease prefab to spawn.")]
	public GameObject greasePrefab;

	[Tooltip("The Captain prefab to spawn.")]
	public GameObject captainPrefab;

	[HideInInspector]
	public int spawnIndex = -1;

	[HideInInspector]
	public string prefabName;

//	private static PlayerSpawnManager ps_Instance;
//	public static PlayerSpawnManager Instance {
//		get {
//			if (ps_Instance == null) {
//				ps_Instance = new PlayerSpawnManager ();
//			}
//			return ps_Instance;
//		}
//	}

//	public void Awake() {
//		ps_Instance = new PlayerSpawnManager ();
//		ps_Instance.spawnPoints = spawnPoints;
//		ps_Instance.sargePrefab = sargePrefab;
//	}
//
	public void Start() {
		SpawnPlayer ();
	}


	private void SpawnPlayer() {
		// Figure out which class the player chose. Defaults to Sarge
		string playerClass = PlayerPrefs.GetString(playerClassPrefKey, "s");

		// Figure out which spawn point to use for the player
		spawnIndex = PlayerPrefs.GetInt("pin", 0);

		/// Get the prefab name
	
		if (playerClass.Equals ("d")) {
			// Case for Doc
			prefabName = docPrefab.name;
		} else if (playerClass.Equals ("g")) {
			// Case for Grease
			prefabName = greasePrefab.name;
		} else if (playerClass.Equals ("c")) {
			// Case for Captain
			prefabName = captainPrefab.name;
		} else {
			// Default case; goes to Sarge
			prefabName = sargePrefab.name;
		}

		//ColorPerPlayer cpp = this.GetComponent<ColorPerPlayer> ();
		//Color playerColor = cpp.MyColor;


		//for (int i = 0; i < cpp.Colors.Length; i++) {
		//	if (playerColor.Equals (cpp.Colors [i])) {
		//		spawnIndex = i;
		//		break;
		//	}
		//}

		/// IMPORTANT
		/// We should make it so that, at this point, a PlayerManager of some sort now knows what the 
		/// player prefab is. In respawn, we will use the prefab associated with the PlayerManager
		/// 
		/// 
		Debug.Log("You spawned as: " + prefabName);
		Debug.Log ("SpawnIndex: " + spawnIndex);
		// Spawn the player using their prefab name
		if (spawnIndex > -1) {
			if (PhotonNetwork.connected) {
				// Multiplayer instantiate
				GameObject player = PhotonNetwork.Instantiate (prefabName, spawnPoints [spawnIndex].transform.position, spawnPoints [spawnIndex].transform.rotation, 0);
			} else {
				// Singleplayer instantiate
				GameObject player = Instantiate(Resources.Load (prefabName, typeof(GameObject)) as GameObject, 
					spawnPoints [0].transform.position, spawnPoints [0].transform.rotation);
			}

		} else {
			// MAJOR ERROR WHOA
			Debug.Log("We are not spawning the player... sorry. You have a spawn index of: " + spawnIndex);
		}

	}

	public void RespawnPlayer() {
		// Spawn the player using their prefab name
		if (spawnIndex > -1) {
			PhotonNetwork.Instantiate (prefabName, spawnPoints [spawnIndex].transform.position, spawnPoints [spawnIndex].transform.rotation, 0);
		} else {
			// MAJOR ERROR WHOA
		}

	}

}
