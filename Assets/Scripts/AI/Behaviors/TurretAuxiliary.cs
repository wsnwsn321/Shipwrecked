using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAuxiliary : MonoBehaviour {
    public List<GameObject> effects;
	public List<AudioClip> sounds;


	public void Update(){
		if(gameObject.GetComponent<TurretBehaviors>().isShooting){
		//	GameObject newEffect = GameObject.Instantiate (effects[0], Quaternion.identity);
		//	Destroy (newEffect, 1f);
			AudioSource.PlayClipAtPoint (sounds[0], transform.position, 0.4f);
		}
	}

}
