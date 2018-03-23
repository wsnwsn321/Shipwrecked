using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterAnimationTrigger : MonoBehaviour {
	 Animator anim;
	 AudioSource audio;
	public AudioClip[] clips;
	public string characterInfo;
	public string characterWeaponInfo;
	public string characterName;
	private bool start = true;
	//public AudioClip[] textTyping; 



	void Start() {
		anim = GetComponent<Animator>();
		audio = GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider  col){
		
			int randomClip = Random.Range (0, clips.Length);
	
			
			audio.clip = clips [randomClip];
			audio.Play ();

			anim.SetBool ("trig", true);
			GameObject.Find ("CharacterInfoText").GetComponent<TypeOutScript> ().FinalText = characterInfo;
			GameObject.Find ("CharacterInfoText").GetComponent<TypeOutScript> ().On = true;
	
			GameObject.Find ("WeaponInfoText").GetComponent<TypeOutScript> ().FinalText = characterWeaponInfo;
			GameObject.Find ("WeaponInfoText").GetComponent<TypeOutScript> ().On = true;

			GameObject.Find ("CharacterNameText").GetComponent<TypeOutScript> ().FinalText = characterName;
			GameObject.Find ("CharacterNameText").GetComponent<TypeOutScript> ().On = true;




	}

	void OnTriggerExit(Collider  col){

		GameObject.Find ("CharacterInfoText").GetComponent<TypeOutScript> ().reset = true;
		GameObject.Find ("WeaponInfoText").GetComponent<TypeOutScript> ().reset = true;
		GameObject.Find ("CharacterNameText").GetComponent<TypeOutScript> ().reset = true;

		anim.SetBool ("trig", false);
	}
}
