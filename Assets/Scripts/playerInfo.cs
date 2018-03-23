using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class playerInfo : MonoBehaviour {
	[HideInInspector]
	public string playerName;

	static string playerClassPrefKey = "pc";


	private string docName = "Doctor Tyler Yusko (Doc)";
	private string sargeName = "Sergeant Peter Fremin(Sarge)";
	private string greaseName = "Mechanic Mike Raider(Grease)";
	private string captainName = "Captain Hugo Stiglitz(Captain)";

	public void SetCharacterPref() {
		if(playerName.Equals(docName)) {
			PlayerPrefs.SetString(playerClassPrefKey, "d");
		} else if(playerName.Equals(sargeName)) {
			PlayerPrefs.SetString(playerClassPrefKey, "s");
		} else if(playerName.Equals(greaseName)) {
			PlayerPrefs.SetString(playerClassPrefKey, "g");
		} else {
			PlayerPrefs.SetString(playerClassPrefKey, "c");
		}
	}

	public string GetClassName() {
		string pc = PlayerPrefs.GetString (playerClassPrefKey, "s");
		string className = "Sarge";

		if (pc.Equals ("d")) {
			className = "Doc";
		} else if (pc.Equals ("g")) {
			className = "Grease";
		} else if (pc.Equals ("c")) {
			className = "Captain";
		}

		return className;
	}

}
