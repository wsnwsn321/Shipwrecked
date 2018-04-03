using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepFix : MonoBehaviour {

	public Animator animator;
	AudioSource audio;

	void Start(){
		animator = GetComponent<Animator>();
		audio = GetComponent<AudioSource> ();
	}

	public void FootR(){
		audio.volume = Random.Range (0.20f, 0.32f);
		audio.pitch = Random.Range (0.7f, 0.9f);
		audio.Play();
	}

	public void FootL(){
		audio.volume = Random.Range (0.20f, 0.32f);
		audio.pitch = Random.Range (0.7f, 0.9f);
		audio.Play();
	}

	public void FootStep(){
		audio.volume = Random.Range (0.20f, 0.32f);
		audio.pitch = Random.Range (0.7f, 0.9f);
		audio.Play();
	}

	void Update(){
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Sprint") && audio.isPlaying == false) {
			audio.volume = Random.Range (0.20f, 0.32f);
			audio.pitch = Random.Range (0.7f, 0.9f);
			audio.Play();
			//AudioSource.PlayClipAtPoint (footstepAudio, transform.position, 1);
		}
	}


}
