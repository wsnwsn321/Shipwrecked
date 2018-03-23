using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuKeyPress : MonoBehaviour {

    private AudioSource audio;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.anyKey && !Input.GetMouseButton(0))
        {
            audio.Play();
        }
    }
}
