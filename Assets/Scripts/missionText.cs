using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class missionText : MonoBehaviour {
	public string[] MissionList;

	public int secondaryMission = 1;
	public AudioClip[] clips;
	public int partNo = 0;
	public bool hasMission = true;
	// Use this for initialization
	void Start()
	{

		StartCoroutine("MissionControl");
		StartCoroutine("MissionUpdate");

	}

	// Update is called once per frame
	void Update () {

		if (hasMission) {
			
			StartCoroutine("MissionControl");
		}

	}


	private IEnumerator MissionUpdate()
	{
		yield return new WaitForSeconds (90f);
		secondaryMission = 2;
		hasMission = true;
	}

	private IEnumerator MissionControl()
	{



	


				hasMission = false;
				string primaryMission = string.Concat ("[", partNo, "/5", "]", MissionList [0]); 
				gameObject.GetComponent<TypeOutScript> ().reset = true;
				gameObject.GetComponent<TypeOutScript> ().TotalTypeTime = 2f;
				gameObject.GetComponent<TypeOutScript> ().FinalText = "Loading Mission...";
				gameObject.GetComponent<TypeOutScript> ().On = true;


				//audio for mission alert
				gameObject.GetComponent<AudioSource> ().loop = true;
				gameObject.GetComponent<AudioSource> ().clip = clips [0];
				gameObject.GetComponent<AudioSource> ().Play ();

				yield return new WaitForSeconds (4f);
				gameObject.GetComponent<AudioSource> ().loop = false;


				//audio for mission display
				gameObject.GetComponent<AudioSource> ().loop = true;
				gameObject.GetComponent<AudioSource> ().clip = clips [1];
				gameObject.GetComponent<AudioSource> ().Play ();

				gameObject.GetComponent<TypeOutScript> ().reset = true;
		MissionList [secondaryMission] = MissionList [secondaryMission].Replace("<br>", "\n");

				gameObject.GetComponent<TypeOutScript> ().FinalText = string.Concat (primaryMission, MissionList [secondaryMission]);
				gameObject.GetComponent<TypeOutScript> ().On = true;
				yield return new WaitForSeconds (2f);
				gameObject.GetComponent<AudioSource> ().loop = false;


	}

}
