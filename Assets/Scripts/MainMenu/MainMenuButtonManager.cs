using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TaskOnClick();
        }
    }

    public void TaskOnClick()
    {
        if (EventSystem.current.currentSelectedGameObject.tag == "Controls")
        {
            mainmenu.SetActive(false);
            controls.SetActive(true);
            EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("MainMenuSelect"));
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
		if (EventSystem.current.currentSelectedGameObject.tag == "GameLevel") {
			SceneManager.LoadScene ("IntroScene");

		} else {
			SceneManager.LoadScene (EventSystem.current.currentSelectedGameObject.tag.ToString ());
		}
    }
}