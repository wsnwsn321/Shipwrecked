using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

    public GameObject pauseMenu;
    private GameplayManager gm;

    // Use this for initialization
    void Start () {
        gm = GetComponent<GameplayManager>();
        pauseMenu.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.P))
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
        }
    }
}
