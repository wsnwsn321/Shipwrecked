using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Increase : MonoBehaviour {
    public GameObject healedEffect;
    private GameObject h;
	private CoreControl cc;
	private Animator an;
	private PlayerHealth allieHP;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Sarge"|| other.gameObject.tag == "Mechanic"|| other.gameObject.tag == "Captain")
        {
			cc = other.gameObject.GetComponent<CoreControl> ();
			an = other.gameObject.GetComponent<Animator> ();
			allieHP = other.gameObject.GetComponent<PlayerHealth> ();
			Destroy(gameObject);
			h = Instantiate(healedEffect, other.transform.position, Quaternion.identity);
			allieHP.health += 40;
			allieHP.updateHealthBar ();
			allieHP.updateHealthText ();
			Destroy(h, 2f);
			if (cc.dead||an.GetCurrentAnimatorStateInfo(0).IsName("Die")) {
				an.SetTrigger ("Revived");

			}
            
        }

    }
}
