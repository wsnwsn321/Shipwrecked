using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectEnd : Photon.MonoBehaviour {

	public string nextScene;
	public NavigationManager nm;

	// Update is called once per frame
	void Update () {

		string name;
		if (Input.GetKeyDown (KeyCode.A)) {
			name = GameObject.Find ("CharacterNameText").GetComponent<TypeOutScript> ().FinalText;
			GameObject.Find ("Player").GetComponent<playerInfo> ().playerName = name;
			GameObject.Find ("Player").GetComponent<playerInfo> ().SetCharacterPref ();
			if (!PhotonNetwork.connected) {
				BeginSingleplayerGame ();
			} else {
				nm.CloseCharacterSelect ();
			}
		}
	}

//	public IEnumerator Countdown()
//	{
//		while (countdown >0)
//		{
//			yield return new WaitForSeconds(1.0f);
//			countdown --;
//			countdownText.text = countdown.ToString ();
//		}
//	}

	void BeginSingleplayerGame() {
		SceneManager.LoadScene (nextScene);
	}
}
