using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Increase : Photon.MonoBehaviour {
    public GameObject healedEffect;
	[HideInInspector]
	public GameObject thrower;
    private GameObject h;
	private CoreControl cc;
	private Animator an;
	private PlayerHealth allieHP;
	private bool activated = false;

    void OnCollisionEnter (Collision other)
	{
		if (PlayerManager.LocalPlayerInstance.Equals (thrower)) {
			if (!activated && other.gameObject != thrower && other.gameObject.layer == LayerMask.NameToLayer ("Character")) {
				activated = true;
				if (!PhotonNetwork.connected) {
					cc = other.gameObject.GetComponent<CoreControl> ();
					an = other.gameObject.GetComponent<Animator> ();
					allieHP = other.gameObject.GetComponent<PlayerHealth> ();
					h = Instantiate (healedEffect, other.transform.position, Quaternion.identity);
					allieHP.RecoverOrRevive (40);
					Destroy (h, 2f);
					if (cc.dead || an.GetCurrentAnimatorStateInfo (0).IsName ("Die")) {
						an.SetTrigger ("Revived");
					}
					Destroy (gameObject);
				} else if (photonView.isMine) {
					other.gameObject.GetComponent<CoreControl> ().WillBePilled(this.gameObject.transform.position);
					h = PhotonNetwork.Instantiate (healedEffect.name, other.transform.position, Quaternion.identity, 0);
					this.gameObject.GetComponent<CapsuleCollider> ().enabled = false;
					StartCoroutine (WaitDestroyHealEffect ());
				}

			}
		}
	}


	IEnumerator WaitDestroyHealEffect()
	{
		gameObject.transform.localScale.Set (0f, 0f, 0f);
		yield return new WaitForSeconds(1f);
		PhotonNetwork.Destroy (h);
		PhotonNetwork.Destroy (gameObject);
	}

}