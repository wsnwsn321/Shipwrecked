using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBuffDoctor : MonoBehaviour {

	public GameObject healEffect;
	private GameObject healing;
	public GameObject Hola;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggeEnter(Collider collision)
	{
		print ("colliding");
		if ((collision.gameObject.tag == "Sarge" || collision.gameObject.tag == "Mechanic" || collision.gameObject.tag == "Captain"))
		{
			print ("colliding");
			healing = PhotonNetwork.connected? PhotonNetwork.Instantiate(healEffect.name, transform.position, Quaternion.identity,0) :Instantiate(healEffect, transform.position, Quaternion.identity);
			StartCoroutine(EndBuff());
		}

	}
	void OnTriggeStay(Collider collision)
	{	
		print ("colliding");
		if ((collision.gameObject.tag == "Sarge" || collision.gameObject.tag == "Mechanic" || collision.gameObject.tag == "Captain"))
		{
			print ("colliding");
			healing = PhotonNetwork.connected? PhotonNetwork.Instantiate(healEffect.name, transform.position, Quaternion.identity,0) :Instantiate(healEffect, transform.position, Quaternion.identity);
			StartCoroutine(EndBuff());
		}

	}
	void OnTriggeExit(Collider collision)
	{
		if ((collision.gameObject.tag == "Sarge" || collision.gameObject.tag == "Mechanic" || collision.gameObject.tag == "Captain"))
		{
			StopHealing ();
		}

	}


}
