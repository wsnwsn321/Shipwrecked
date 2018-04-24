using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NavigationManager : MonoBehaviour {

    public string sceneOnEscape;
	public string sceneOnStart;
	public GameObject characterSelect;

	public GameObject bg;
	public GameObject waitingRoom;
	public WaitingRoomManager wrm;
	public GameObject[] navigateableWindows;

	public NavController currentNavController;



	void Start() {
		//currentNavController.InitializeController ();
	}

	// Update is called once per frame
	void Update () {
		if (InputManager.Pause()) {
            ExitMultiplayer ();
		}
		//currentNavController.UpdateController ();
    }

	public void ChangeNavController() {
//		NavController prevNav = currentNavController;
//		foreach(GameObject window in navigateableWindows) {
//			if (window.activeInHierarchy) {
//				currentNavController = window.GetComponent<NavController> ();
//				currentNavController.InitializeController ();
//				break;
//			}
//		}
//		if (prevNav.Equals (currentNavController)) {
//			// This line of code hurts me to add
//			currentNavController = navigateableWindows[3].GetComponent<NavController>();
//		}
	}

	public void OpenCharacterSelect() {
		characterSelect.SetActive (true);
		bg.SetActive (false);
		waitingRoom.SetActive (false);
		//ChangeNavController ();
	}

	public void CloseCharacterSelect() {
		characterSelect.SetActive (false);
		bg.SetActive (true);
		waitingRoom.SetActive (true);
		wrm.UpdateWindow ();

		//ChangeNavController ();
	}


	public void ExitMultiplayer() {
		
		SceneManager.LoadScene(sceneOnEscape);
	}

}
