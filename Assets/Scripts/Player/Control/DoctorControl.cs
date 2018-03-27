using System;
using System.Collections;
using System.Collections.Generic;
using PlayerAbilities;
using UnityEngine;

public class DoctorControl : Photon.MonoBehaviour, IClassControl {

    public int maxPills = 1;
    public GameObject healParticle;
    public GameObject pill;
	public GameObject healBuff;
	private GameObject healEffect;
    [HideInInspector]
    public float healthPerSec = 1f;
    [HideInInspector]
    public float researchBuff = 1f;

	public LayerMask layerMask;

    private GameObject healing;
    private List<GameObject> pills;
    private bool heal;
	private Animator animator;

    void Start()
    {
        heal = false;
        pills = new List<GameObject>();
		animator = GetComponent<CoreControl>().GetAnimator();
    }

    void ThrowPill()
    {
        if (pills.Count < maxPills)
        {
            
            if (animator&&!animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                animator.SetTrigger("Ability1");
            }
			GameObject currentPill = PhotonNetwork.connected ? PhotonNetwork.Instantiate(pill.name, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity, 0) :Instantiate(pill, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
            Physics.IgnoreCollision(this.GetComponent<BoxCollider>(), currentPill.GetComponent<CapsuleCollider>());
            currentPill.GetComponent<Rigidbody>().velocity = GetComponent<Control>().main_c.transform.forward * 10;
            pills.Add(currentPill);

            StartCoroutine(waitPillDie());
        }
    }

	void HealingCircle(){
		if (animator&&!animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
		{
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("AB2")) {
				animator.SetTrigger("Ability2");
				healBuff.SetActive (true);
				StartCoroutine(HealBuff());
			}
			Collider[] players = Physics.OverlapSphere (transform.position, 15f,layerMask, QueryTriggerInteraction.Collide);
			Debug.Log (players.Length);
			for(int i=0;i<players.Length;i++){
				Debug.Log (players[i].name);
				healEffect = players [i].transform.GetChild (5).gameObject;
				healEffect.SetActive (true);
				//healing = PhotonNetwork.connected? PhotonNetwork.Instantiate(healEffect.name, players[i].transform.position, Quaternion.identity,0) :Instantiate(healEffect,  players[i].transform.position, Quaternion.identity);
				//healing.transform.position = players [i].transform.position;	
				StartCoroutine(EndBuff(healEffect));

			}

		}

	}



	IEnumerator EndBuff(GameObject hE)
	{
		yield return new WaitForSeconds(5f);
		hE.SetActive (false);
		//StopHealing ();
		healBuff.SetActive (false);

	}

	void StopHealing()
	{
		if (PhotonNetwork.connected) {
			PhotonNetwork.Destroy (healing);
		} else {
			Destroy (healing);
		}
		healing = null;
	}

    IEnumerator waitPillDie()
    {
        yield return new WaitForSeconds(5f);
		if (PhotonNetwork.connected) {
			PhotonNetwork.Destroy (pills [0]);
		} else {
			Destroy (pills [0]);
		}
        pills.RemoveAt(0);
    }
		


	IEnumerator HealBuff( )
	{
		yield return new WaitForSeconds(5f);
		healBuff.SetActive (false);
		animator.SetTrigger("Ab2Finished");

	}
    #region Inherited Methods

    public void Activate(SpecialAbility ability)
    {
        if (ability == SpecialAbility.ThrowPill)
        {
            ThrowPill();
        }
		if (ability == SpecialAbility.HealingCircle)
		{
			HealingCircle();
		}
    }

    public bool CanAim()
    {
        return true;
    }

    public bool CanIdle()
    {
        return true;
    }

    public bool CanJump()
    {
        return true;
    }

    public bool CanMove()
    {
        return true;
    }

    public bool CanPickUpObject()
    {
        return true;
    }

    public bool CanReload()
    {
        return true;
    }

    public bool CanRoll()
    {
        return true;
    }

    public bool CanShoot()
    {
        return true;
    }

    public bool CanSprint()
    {
        return true;
    }

	public bool CanUseAbility1()
	{
		return !heal;
	}

	public bool CanUseAbility2()
	{
		return !heal;
	}

    public void FixedUpdateActions(float deltaTime)
    {
        // TODO
    }

    public void StopAction()
    {
        // TODO
    }

    public void UpdateActions(float deltaTime)
    {
        // TODO
    }

    public void UpdateAnimationStates(Animator animator)
    {
        if (animator)
        {
            //animator.SetBool("Healing", heal);
        }
    }

    #endregion
}
