using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbynameManager : MonoBehaviour {

	public InputField lobbynameField;
	public Button confirmButton;

	private Vector3 buttonScale;
	private string lobbyname;
	// Use this for initialization
	void Start () {
		lobbynameField.onValueChanged.AddListener (delegate {ModifyConfirm(); });

		confirmButton.enabled = false;
		buttonScale = confirmButton.transform.localScale;
		confirmButton.transform.localScale = Vector3.zero;
	}

	public string GetLobbyname() {
		return lobbyname;
	}


	void ModifyConfirm() {
		if (lobbynameField.text.Length > 3) {
			confirmButton.enabled = true;
			confirmButton.transform.localScale = buttonScale;
			lobbyname = lobbynameField.text;
		} else {
			confirmButton.enabled = false;
			confirmButton.transform.localScale = Vector3.zero;
		}
	}

}
