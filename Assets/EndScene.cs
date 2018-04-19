using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;



public class EndScene : MonoBehaviour {
	public string[] Story;
	public AudioClip[] clips;

	void Start () {
		StartCoroutine("PlayVideo");
	}
	
	// Update is called once per frame
	void Update () {
		

	}

	private IEnumerator PlayVideo()
	{

		yield return new WaitForSeconds(1f);
		GameObject.Find ("Main Camera").GetComponent<AudioSource>().Play();	
		yield return new WaitForSeconds(1f);
		//type out story text
		for (int i = 0; i < 4; i++) {

			int randomClip = Random.Range (0, clips.Length);
			//set text to next section of story
			GameObject.Find ("Text").GetComponent<TypeOutScript> ().reset = true;
			GameObject.Find ("Text").GetComponent<TypeOutScript> ().FinalText = Story [i];
			if (GameObject.Find ("Text").GetComponent<TypeOutScript> ().FinalText.Length < 40) {
				GameObject.Find ("Text").GetComponent<TypeOutScript> ().TotalTypeTime = 3;
			} else {
				GameObject.Find ("Text").GetComponent<TypeOutScript> ().TotalTypeTime = 10;
			}
			GameObject.Find ("Text").GetComponent<TypeOutScript> ().On = true;
			gameObject.GetComponent<AudioSource> ().loop = true;
			gameObject.GetComponent<AudioSource>().clip = clips [0];
			//randomClip
			gameObject.GetComponent<AudioSource>().Play ();

			while (GameObject.Find ("Text").GetComponent<Text> ().text.Length != GameObject.Find ("Text").GetComponent<TypeOutScript> ().FinalText.Length) {
				yield return new WaitForSeconds (.1f);

			}
			gameObject.GetComponent<AudioSource> ().loop = false;
			yield return new WaitForSeconds(3f); // wait to let people read
		}
		//set text to nothing
		GameObject.Find ("Text").GetComponent<TypeOutScript> ().reset = true;

		GameObject.Find("Cube").GetComponent<VideoPlayer>().Prepare();
		GameObject.Find ("Main Camera").GetComponent<AudioSource>().Pause();
		GameObject.Find("Cube").GetComponent<VideoPlayer>().Play();

		yield return new WaitForSeconds(30f);
		SceneManager.LoadScene ("RegularCredits");
	}
}
