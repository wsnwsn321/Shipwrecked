using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buff_script : MonoBehaviour {

    // Use this for initialization
    public GameObject buffCylinder;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(this.transform.position, buffCylinder.transform.position) < 8f)
        {
            buffCylinder.SetActive(true);
        }
        else
        {
            buffCylinder.SetActive(false);
        }
	}
}
