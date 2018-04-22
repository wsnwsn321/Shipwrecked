using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtonManager : MonoBehaviour
{

    private GameObject[] buttonArr = new GameObject[5];
    public static int selectionIndex;

    bool canInput;
    float inputDelay;

    // Use this for initialization
    void Start()
    {
        buttonArr[0] = GameObject.Find("Single Player");
        buttonArr[1] = GameObject.Find("Multiplayer");
        buttonArr[2] = GameObject.Find("Controls");
        buttonArr[3] = GameObject.Find("Credits");
        buttonArr[4] = GameObject.Find("Quit");

        selectionIndex = 0;
        canInput = true;
        inputDelay = 0.2f;
    }

    private void Update()
    {
        if (InputManager.MenuNavigateUp())
        {
            selectionIndex--;
            Debug.Log("Up: selectionIndex = " + selectionIndex);
            StartCoroutine(DelayInput());
        }
        else if (InputManager.MenuNavigateDown())
        {
            selectionIndex++;
            Debug.Log("Down: selectionIndex = " + selectionIndex);
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
    }

    private IEnumerator DelayInput()
    {
        canInput = false;
        yield return new WaitForSeconds(inputDelay);
        canInput = true;
    }
}