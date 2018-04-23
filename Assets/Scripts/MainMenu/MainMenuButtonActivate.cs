using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtonActivate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject controls;
    public GameObject mainmenu;
    private Button button;
    private AudioSource audioSource;

    public static GameObject[] buttonArr = new GameObject[5];

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

        if (InputManager.MenuSelect())
        {
            TaskOnClick();
        }
        if (controls.activeSelf == false && !SceneManager.GetActiveScene().name.Equals("RegularCredits"))
        {
            EventSystem.current.SetSelectedGameObject(buttonArr[MainMenuButtonManager.selectionIndex]);
        }
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
            audioSource.Play();
            StartCoroutine("waitForSound");
            controls.SetActive(true);
            mainmenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(GameObject.Find("MainBack"));
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "MainBack")
        {
            audioSource.Play();
            StartCoroutine("waitForSound");
            SceneManager.LoadScene("MainMenuSelect");
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
        if (EventSystem.current.currentSelectedGameObject.tag == "GameLevel")
        {
            SceneManager.LoadScene("IntroScene");

        }
        else
        {
            SceneManager.LoadScene(EventSystem.current.currentSelectedGameObject.tag.ToString());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (controls.activeSelf == false && !SceneManager.GetActiveScene().name.Equals("RegularCredits"))
        {
            for (int i = 0; i < buttonArr.Length; i++)
            {
                if (buttonArr[i].name.Equals(gameObject.name))
                {
                    MainMenuButtonManager.selectionIndex = i;
                    break;
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}