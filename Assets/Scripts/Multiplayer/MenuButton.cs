using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{

	private Button button;
	private GameObject nm;
	//private AudioSource audioSource;

	// Use this for initialization
	void Start()
	{
		//audioSource = GameObject.Find("Key Press Audio").GetComponent<AudioSource>();
		nm = GameObject.Find("NetworkManager");
		if (nm == null) {
			Debug.Log ("ERROR: NetworkManager was not found!");
			return;
		}
		button = GetComponent<Button>();
		button.onClick.AddListener(TaskOnClick);
	}


	public void TaskOnClick()
	{
		//audioSource.Play();
		StartCoroutine("waitForSound");
	}

	IEnumerator waitForSound()
	{
		yield return new WaitForSeconds(1.0f);
		if (button.tag.ToString ().Equals ("JoinGame")) {
			// Join an existing lobby
		} else if (button.tag.ToString ().Equals ("CreateLobby")) {
			// Host a new lobby
		} else {
			// Exit to main menu
			SceneManager.LoadScene("MainMenu");
		}
			
	}
}