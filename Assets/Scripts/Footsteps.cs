using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour {

	CoreControl cc;
	public AudioClip footstepAudio;
	AudioSource audioSource;
	bool isPlaying;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CoreControl> ();
		audioSource.clip = footstepAudio;
	}
	
	// Update is called once per frame
	void Update () {
		if (cc.isGrounded == true && (cc.forwardMovement != 0  || cc.horizontalMovement != 0)) {
			//AudioSource.PlayClipAtPoint (footstepAudio, transform.position, 1);
			//audioSource.Play();
		}
	}
}
