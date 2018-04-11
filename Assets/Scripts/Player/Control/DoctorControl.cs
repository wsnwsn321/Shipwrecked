using System;
using System.Collections;
using System.Collections.Generic;
using PlayerAbilities;
using UnityEngine;
using UnityEngine.UI;

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
	private bool canBuff;
	private Animator animator;
	private PlayerHealth allieHP;

    private CooldownTimerUI timer;
    public float skillTimeStamp;
    private float healingCooldown = 5f;
	private float healBuffCooldown =10f;

    void Start()
    {
        timer = new CooldownTimerUI(GameObject.FindGameObjectWithTag("Skill1").GetComponent<Image>(), GameObject.FindGameObjectWithTag("Skill2").GetComponent<Image>());
        timer.CooldownStart();
		canBuff = true;
    }

    void Update()
    {
		if (animator == null) {
			heal = false;
			pills = new List<GameObject> ();
			animator = GetComponent<CoreControl> ().GetAnimator ();
			if (animator == null) {
				Debug.Log ("ERROR! Doctor cannot retrieve its animator");
			}
		}
        timer.CooldownUpdate(healingCooldown, skillTimeStamp);
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
			currentPill.GetComponent<Increase> ().player = PhotonNetwork.player;
			currentPill.GetComponent<Increase> ().thrower = this.gameObject;
			Physics.IgnoreCollision(this.GetComponent<BoxCollider>(), currentPill.GetComponent<CapsuleCollider>());
            currentPill.GetComponent<Rigidbody>().velocity = GetComponent<Control>().main_c.transform.forward * 10;
            pills.Add(currentPill);

            StartCoroutine(waitPillDie());
        }
    }

	void HealingCircle(){
		if (animator&&!animator.GetCurrentAnimatorStateInfo(0).IsName("Die")&&canBuff)
		{
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("AB2")) {
				canBuff = false;
				animator.SetTrigger("Ability2");
				healBuff.SetActive (true);
				StartCoroutine(HealBuff());
				StartCoroutine(WaitAbilityUse());
			}
			Collider[] players = Physics.OverlapSphere (transform.position, 15f,layerMask, QueryTriggerInteraction.Collide);
			Debug.Log (players.Length);
			for(int i=0;i<players.Length;i++){
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
        // Start cooldown animation for UI skill image
        timer.startCooldownTimerUI(1);
        skillTimeStamp = Time.time + healingCooldown;
        yield return new WaitForSeconds(5f);
		if (PhotonNetwork.connected) {
			if (pills [0] != null) {
				PhotonNetwork.Destroy (pills [0]);
			}
		} else {
			Destroy (pills [0]);
        }
        pills.RemoveAt(0);
    }

	IEnumerator WaitAbilityUse()
	{
		yield return new WaitForSeconds(healBuffCooldown);
		canBuff = true;
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

    public bool OverrideAbility2()
    {
        return false;
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
