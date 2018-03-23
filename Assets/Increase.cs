using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Increase : MonoBehaviour {
    public GameObject healedEffect;
    private GameObject h;
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
            Destroy(gameObject);
            h = Instantiate(healedEffect, other.transform.position, Quaternion.identity);
            Destroy(h, 2f);
        }

    }
}
