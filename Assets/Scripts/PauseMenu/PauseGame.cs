using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

    public GameObject pauseMenu;
    public GameObject controlsMenu;
    private GameplayManager gm;

    // Use this for initialization
    void Start () {
        gm = GetComponent<GameplayManager>();
        pauseMenu.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		if (InputManager.Pause())
        {
            if (pauseMenu.activeInHierarchy)
            {
                gm.Resume();
                pauseMenu.SetActive(false);
            }
            else
            {
                pauseMenu.SetActive(true);
                gm.Pause();
            }

            if (controlsMenu.activeInHierarchy)
            {
                controlsMenu.SetActive(false);
            }
        }
    }
}
