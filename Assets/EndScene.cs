using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class EndScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("PlayVideo");
	}
	
	// Update is called once per frame
	void Update () {
		

	}

	private IEnumerator PlayVideo()
	{
		yield return new WaitForSeconds(30f);
		SceneManager.LoadScene ("RegularCredits");
	}
}
