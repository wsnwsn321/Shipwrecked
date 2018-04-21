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

    private GameObject[] buttonArr = new GameObject[5];
    private int selectionIndex;

    // Use this for initialization
    void Start()
    {
        mainmenu = GameObject.Find("MenuCanvas");
        audioSource = GameObject.Find("Key Press Audio").GetComponent<AudioSource>();

        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        buttonArr[0] = GameObject.Find("Single Player");
        buttonArr[1] = GameObject.Find("Multiplayer");
        buttonArr[2] = GameObject.Find("Controls");
        buttonArr[3] = GameObject.Find("Credits");
        buttonArr[4] = GameObject.Find("Quit");
        selectionIndex = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TaskOnClick();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            selectionIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            selectionIndex++;
        }
        if (selectionIndex < 0)
        {
            selectionIndex = buttonArr.Length - 1;
        }
        else if (selectionIndex >= buttonArr.Length)
        {
            selectionIndex = 0;
        }
        EventSystem.current.SetSelectedGameObject(buttonArr[selectionIndex]);
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