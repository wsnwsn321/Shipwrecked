using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepFix : MonoBehaviour {

	public AudioClip footstepAudio;
	CoreControl cc;
	private float time;
	private float newTime;
	public Animator animator;
	private float footTime = .470f;


	void Start(){
		time = Time.deltaTime;
		newTime = Time.deltaTime;
		animator = GetComponent<Animator>();
	}

	/*void Update(){
		time = newTime;
		newTime = Time.deltaTime;
	}*/

	public void FootR(){
		//if (cc.isGrounded == true && (cc.forwardMovement != 0  || cc.horizontalMovement != 0)) {
			AudioSource.PlayClipAtPoint (footstepAudio, transform.position, 1);
			//audioSource.Play();
		//}
	}

	public void FootL(){
		//if (cc.isGrounded == true && (cc.forwardMovement != 0  || cc.horizontalMovement != 0)) {
			AudioSource.PlayClipAtPoint (footstepAudio, transform.position, 1);
		//}
	}

	public void FootStep(){
		//if (cc.isGrounded == true && (cc.forwardMovement != 0  || cc.horizontalMovement != 0)) {
			AudioSource.PlayClipAtPoint (footstepAudio, transform.position, 1);
		//}
	}

	void Update(){
		newTime = Time.deltaTime;
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Sprint") && (newTime > (time + footTime))) {
			AudioSource.PlayClipAtPoint (footstepAudio, transform.position, 1);
			time = Time.deltaTime;
		}
			
	}

	/*void Sprint(){
		//if(cc.animator.GetCurrentAnimatorStateInfo(0).IsName("Sprint")){
		if (time != newTime) {
			AudioSource.PlayClipAtPoint (footstepAudio, transform.position, 1);
		}
		//}


	}*/

}
