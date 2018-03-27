using System;
using System.Collections;
using System.Collections.Generic;
using PlayerAbilities;
using UnityEngine;

public class CaptainControl : MonoBehaviour, IClassControl
{
	[Range(0f, 10f)]
	public float RampageCooldown = 10f;
	[Range(0f, 10f)]
	public float RampageTime = 8f;
    [HideInInspector]
    public float attackBuff = 1f, defenseBuff = 1f;
	private Animator animator;
	public GameObject flame;
	private GameObject flaming;
	private animation shoot;
	private bool canRampage;
    void Start()
    {
        // TODO
		animator = GetComponent<Animator>();
		canRampage = true;

    }

	void Rampage(){
		if (canRampage&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("AB1")) {
			canRampage = false;
			if (animator) {
				animator.SetTrigger("Ability1");
			}
			flaming = PhotonNetwork.connected? PhotonNetwork.Instantiate(flame.name, transform.position, Quaternion.identity,0) :Instantiate(flame, transform.position, Quaternion.identity);
			flaming.transform.parent = transform;
			if (animator.GetCurrentAnimatorStateInfo (0).IsName("Shoot")) {
				animator.speed = 2f;
			} else {
				animator.speed = 1f;
			}
			StartCoroutine(HealForTime());
		}
	}

	void StopRampage()
	{
		if (PhotonNetwork.connected) {
			PhotonNetwork.Destroy (flaming);
		} else {
			Destroy (flaming);
		}
		flaming = null;
		animator.speed = 1f;
		StartCoroutine(WaitAbilityUse());
	}

	IEnumerator HealForTime()
	{
		yield return new WaitForSeconds(RampageTime);
		if (flaming)
		{
			StopRampage();
		}
	}
	IEnumerator WaitAbilityUse()
	{
		yield return new WaitForSeconds(RampageCooldown);
		canRampage = true;
	}

    #region Inherited Methods

    public void Activate(SpecialAbility ability)
    {
        if (ability == SpecialAbility.Leadership)
        {
			Rampage ();
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
		return true;
	}

	public bool CanUseAbility2()
	{
		return true;
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
            // TODO
        }
    }

    #endregion
}
