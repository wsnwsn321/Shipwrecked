using System;
using System.Collections;
using System.Collections.Generic;
using PlayerAbilities;
using UnityEngine;
using UnityEngine.UI;

public class SergeantControl : Photon.MonoBehaviour, IClassControl
{
    [Range(0f, 10f)]
    public float healCooldown = 10f;
	public float autoCooldown = 10f;
	public float autoBuffTime = 4f;
    [Range(0f, 10f)]
    public float healTime = 5f;
    [Range(1, 100)]
    public int healDivisions = 10;
	public float healAmount;
    public GameObject healParticle;
    private Animator ani;
    private float currentHealTime;
    private float healDelay;
    private GameObject healing;
    private bool canHeal,canAuto;
	private PlayerHealth myhp;
    private CooldownTimerUI timer;
    public float skillTimeStamp1;
    public float skillTimeStamp2;
    private CoreControl cc;
    void Start()
    {

		healAmount = 20;
        currentHealTime = 0;
        healDelay = healTime / healDivisions;
        canHeal = true;
		canAuto = true;

    }

    void Update()
	{
		if (ani == null) {
			ani = GetComponent<Animator> ();
			myhp = GetComponent<PlayerHealth> ();
			cc = GetComponent<CoreControl> ();
			if (ani && myhp && cc) {
				Debug.Log ("Sarge retrieved components successfully!");
			}
		}
		if (!PhotonNetwork.connected || photonView.isMine) {
			if (timer == null) {
				timer = new CooldownTimerUI (GameObject.FindGameObjectWithTag ("Skill1").GetComponent<Image> (), GameObject.FindGameObjectWithTag ("Skill2").GetComponent<Image> ());
				timer.CooldownStart ();
			}

			timer.CooldownUpdate (healCooldown, autoCooldown, skillTimeStamp1, skillTimeStamp2);
		}
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

	void AutoRifle(){
		if (canAuto && !cc.autoRifle) {
			cc.autoRifle = true;
            // Start cooldown animation for UI skill image
         
        }
		StartCoroutine(AutoRifleTime());
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
            // Start cooldown animation for UI skill image
            timer.startCooldownTimerUI(1);
            skillTimeStamp1 = Time.time + healCooldown;
        
        }
        healing = null;
        StartCoroutine(WaitAbilityUse());
    }

    IEnumerator HealForTime()
    {
        yield return new WaitForSeconds(healTime);
		myhp.RecoverHealth(healAmount);

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
	IEnumerator AutoRifleTime()
	{
		yield return new WaitForSeconds(autoBuffTime);
		canAuto = true;
		cc.autoRifle = false;
		StartCoroutine(WaitAbility2Use());
		timer.startCooldownTimerUI(2);
		skillTimeStamp2 = Time.time + autoCooldown;

	}

	IEnumerator WaitAbility2Use()
	{
		yield return new WaitForSeconds(autoCooldown);
		canAuto = true;
		cc.autoRifle = false;

	}

    #region Inherited Methods

    public void Activate(SpecialAbility ability)
    {
        if (ability == SpecialAbility.HealSelf)
        {
            HealSelf();
        }

		if (ability == SpecialAbility.AutoRifle) {
			AutoRifle ();
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

    public bool OverrideAbility2()
    {
        return false;
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
