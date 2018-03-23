using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtonManager : MonoBehaviour
{
    public GameObject controls;
    private GameObject mainmenu;
    private Button button;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        mainmenu = GameObject.Find("MenuCanvas");
        audioSource = GameObject.Find("Key Press Audio").GetComponent<AudioSource>();

        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
        if (button.tag == "Controls")
        {
            mainmenu.SetActive(false);
            controls.SetActive(true);
        }
        else
        {
            audioSource.Play();
            StartCoroutine("waitForSound");
        }
    }

    IEnumerator waitForSound()
    {
        yield return new WaitForSeconds(1.0f); 
		if (button.tag == "GameLevel") {
			SceneManager.LoadScene ("IntroScene");

		} else {
			SceneManager.LoadScene (button.tag.ToString ());
		}
    }
}