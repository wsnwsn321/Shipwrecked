using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameManager : MonoBehaviour {

	public InputField nicknameField;
	public Button confirmButton;

	private Vector3 buttonScale;
	private string nickname;

	// Store the PlayerPref Key to avoid typos
	static string playerNamePrefKey = "PlayerName";

	// Use this for initialization
	void Start () {
		nicknameField.onValueChanged.AddListener (delegate {ModifyConfirm(); });
		confirmButton.enabled = false;
		buttonScale = confirmButton.transform.localScale;
		confirmButton.transform.localScale = Vector3.zero;

		string defaultName = "";
		if (nicknameField!=null)
		{
			if (PlayerPrefs.HasKey(playerNamePrefKey))
			{
				defaultName = PlayerPrefs.GetString(playerNamePrefKey);
				nicknameField.text = defaultName;
			}
		}
	}


	public void SaveNickname() {
		// Do something with the nickname
		nickname = nicknameField.text;
		PlayerPrefs.SetString (playerNamePrefKey, nickname);
	}

	void ModifyConfirm() {
		if (nicknameField.text.Length > 3) {
			confirmButton.enabled = true;
			confirmButton.transform.localScale = buttonScale;
		} else {
			confirmButton.enabled = false;
			confirmButton.transform.localScale = Vector3.zero;
		}
	}



}
