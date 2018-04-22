using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtonManager : MonoBehaviour
{
    public GameObject controls;
    public GameObject mainmenu;
    private Button button;
    private AudioSource audioSource;

    private GameObject[] buttonArr = new GameObject[5];
    private int selectionIndex;

    private string[] controllerNames;

    // Use this for initialization
    void Start()
    {
        audioSource = GameObject.Find("Key Press Audio").GetComponent<AudioSource>();

        if (!mainmenu)
        {
            mainmenu = GameObject.Find("MenuCanvas");
        }

        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        buttonArr[0] = GameObject.Find("Single Player");
        buttonArr[1] = GameObject.Find("Multiplayer");
        buttonArr[2] = GameObject.Find("Controls");
        buttonArr[3] = GameObject.Find("Credits");
        buttonArr[4] = GameObject.Find("Quit");

        selectionIndex = 0;
        canInput = true;
        inputDelay = 0.2f;
    }

    bool canInput;
    float inputDelay;
    private void Update()
    {
        if (!canInput)
        {
            return;
        }

        if (!mainmenu || !mainmenu.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
            if (InputManager.MenuSelect())
            {
                TaskOnClick();
            }

            return;
        }

        if (InputManager.MenuSelect())
        {
            TaskOnClick();
        }
        else if (InputManager.MenuNavigateUp())
        {
            selectionIndex--;

            StartCoroutine(DelayInput());
        }
        else if (InputManager.MenuNavigateDown())
        {
            selectionIndex++;
            StartCoroutine(DelayInput());
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

    private IEnumerator DelayInput()
    {
        canInput = false;
        yield return new WaitForSeconds(inputDelay);
        canInput = true;
    }

    public void TaskOnClick()
    {
        if (EventSystem.current.currentSelectedGameObject.tag == "Controls")
        {
            controls.SetActive(true);
            mainmenu.SetActive(false);
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "MainBack")
        {
            mainmenu.SetActive(true);
            controls.SetActive(false);
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