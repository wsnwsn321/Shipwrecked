using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuButtonManager : MonoBehaviour
{

    private Button button;
    private AudioSource audioSource;

    public GameplayManager gm;
    public GameObject pauseMenu;
    public GameObject controls;

    // Use this for initialization
    void Start()
    {
        audioSource = GameObject.Find("Key Press Audio").GetComponent<AudioSource>();

        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }


    public void TaskOnClick()
    {
        if (button.tag == "Controls")
        {
            gm.Pause();
            pauseMenu.SetActive(false);
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
        gm.Resume();

        if (button.name.Equals("Resume"))
        {
            yield return new WaitForSeconds(0.6f);
            pauseMenu.SetActive(false);
            controls.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene(button.tag.ToString());
        }
    }
}