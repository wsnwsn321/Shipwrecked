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
	public Text setShowSelect;

	public static Text showSelect;


	void Start() {
		showSelect = setShowSelect;
		currentNavController.InitializeController ();
	}

	// Update is called once per frame
	void Update () {
		if (InputManager.Pause()) {
            ExitMultiplayer ();
		}
		currentNavController.UpdateController ();
    }

	public void ChangeNavController() {
		foreach(GameObject window in navigateableWindows) {
			if (window.activeInHierarchy) {
				currentNavController = window.GetComponent<NavController> ();
				currentNavController.InitializeController ();
				break;
			}
		}
	}

	public void OpenCharacterSelect() {
		characterSelect.SetActive (true);
		bg.SetActive (false);
		waitingRoom.SetActive (false);
	}

	public void CloseCharacterSelect() {
		characterSelect.SetActive (false);
		bg.SetActive (true);
		waitingRoom.SetActive (true);
		wrm.UpdateWindow ();
	}


	public void ExitMultiplayer() {
		
		SceneManager.LoadScene(sceneOnEscape);
	}

}
