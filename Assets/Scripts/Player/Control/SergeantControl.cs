using System;
using System.Collections;
using System.Collections.Generic;
using PlayerAbilities;
using UnityEngine;

public class SergeantControl : MonoBehaviour, IClassControl
{
    [Range(0f, 10f)]
    public float healCooldown = 5f;
    [Range(0f, 10f)]
    public float healTime = 5f;
    [Range(1, 100)]
    public int healDivisions = 10;
    public GameObject healParticle;
    private Animator ani;
    private float currentHealTime;
    private float healDelay;
    private GameObject healing;
    private bool canHeal;
	private PlayerHealth myhp;

    void Start()
    {
        currentHealTime = 0;
        healDelay = healTime / healDivisions;
        canHeal = true;
        ani = GetComponent<Animator>();
		myhp = GetComponent<PlayerHealth> ();
    }

    void HealSelf()
    {
        if (canHeal && !healing&& !ani.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            canHeal = false;
            Animator animator = GetComponent<CoreControl>().GetAnimator();
            if (animator)
            {
                animator.SetTrigger("Use");
            }

			healing = PhotonNetwork.connected? PhotonNetwork.Instantiate(healParticle.name, transform.position, Quaternion.identity,0) :Instantiate(healParticle, transform.position, Quaternion.identity);
			StartCoroutine(HealForTime());
        }
    }

    void Heal()
    {
        print("Healed 5 health!");
    }

    bool IsHealing()
    {
        return !(healing == null);
    }

    void StopHealing()
    {
		if (PhotonNetwork.connected) {
			PhotonNetwork.Destroy (healing);
		} else {
			Destroy (healing);
		}
        healing = null;
        StartCoroutine(WaitAbilityUse());
    }

    IEnumerator HealForTime()
    {
        yield return new WaitForSeconds(healTime);
		myhp.health += 20;
		myhp.updateHealthBar ();
		myhp.updateHealthText ();
        if (healing)
        {
            StopHealing();
        }
    }

    IEnumerator WaitAbilityUse()
    {
        yield return new WaitForSeconds(healCooldown);
        canHeal = true;
    }

    #region Inherited Methods

    public void Activate(SpecialAbility ability)
    {
        if (ability == SpecialAbility.HealSelf)
        {
            HealSelf();
        }
    }

    public bool CanAim()
    {
        return true;
    }

    public bool CanIdle()
    {
        return !IsHealing();
    }

    public bool CanJump()
    {
        return !IsHealing();
    }

    public bool CanMove()
    {
        return !IsHealing();
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
		return !IsHealing();
	}

	public bool CanUseAbility2()
	{
		return true;
	}

    public void FixedUpdateActions(float deltaTime)
    {
        if (healing)
        {
            healing.transform.position = transform.position;
        }
    }

    public void StopAction()
    {
        if (healing)
        {
            StopHealing();
        }
    }

    public void UpdateActions(float deltaTime)
    {
        if (healing)
        {
            currentHealTime += deltaTime;
            if (currentHealTime >= healDelay)
            {
                currentHealTime -= healDelay;
                Heal();
            }
        }
    }

    public void UpdateAnimationStates(Animator animator)
    {
        if (animator)
        {
            animator.SetBool("Healing", IsHealing());
        }
    }

    #endregion
}
