using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour {

    public string sceneOnEscape;
	public string sceneOnStart;
	public GameObject characterSelect;

	public GameObject bg;
	public GameObject waitingRoom;
	public WaitingRoomManager wrm;


	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.Escape)) {
            ExitMultiplayer ();
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
