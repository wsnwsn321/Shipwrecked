using System;
using System.Collections;
using System.Collections.Generic;
using PlayerAbilities;
using UnityEngine;
using UnityEngine.UI;

public class CaptainControl : Photon.PunBehaviour, IClassControl
{
	
	[Range(0f, 10f)]
	public float StunDistance;
	public float RampageCooldown = 10f;
	public float StunCooldown;
	public float EnemyStunnedTime;
	[Range(0f, 10f)]
	public float RampageTime = 8f;
    [HideInInspector]
    public float attackBuff = 1f, defenseBuff = 1f;

	private Animator animator;
	private Animator enemy_animator;
	private brain_control bc;
	public GameObject flame;
	public GameObject flaming;
	private animation shoot;
	public bool canRampage,canKnockBack, isFlaming;
	private CoreControl corecontrol;
	public LayerMask layerMask;
    private CooldownTimerUI timer;
    public float skillTimeStamp1;
    public float skillTimeStamp2;

    void Start()
    {


        StunDistance = 3f;
		StunCooldown = 5f;
		EnemyStunnedTime = 2f;
        // TODO
		animator = GetComponent<Animator>();
		canRampage = true;
		canKnockBack = true;
		corecontrol = GetComponent<CoreControl> ();
		isFlaming = false;
    }

    void Update()
    {
		if (!PhotonNetwork.connected || photonView.isMine) {
			if (timer == null) {
				timer = new CooldownTimerUI (GameObject.FindGameObjectWithTag ("Skill1").GetComponent<Image> (), GameObject.FindGameObjectWithTag ("Skill2").GetComponent<Image> ());
				timer.CooldownStart ();
			}

			timer.CooldownUpdate (RampageCooldown, StunCooldown, skillTimeStamp1, skillTimeStamp2);
		}
    }

	[PunRPC]
	void Rampage(){
		if (canRampage&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("AB1")&&!animator.GetCurrentAnimatorStateInfo(0).IsName("Die")) {
			isFlaming = true;
			canRampage = false;
			if (animator) {
				animator.SetTrigger("Ability1");
			}
			if (PhotonNetwork.connected && photonView.isMine) {
				// The player who activates the skill instantiates the flame over the network so everyone can see it
				flaming = PhotonNetwork.Instantiate (flame.name, transform.position, Quaternion.identity, 0);
			} else {
				flaming = Instantiate (flame, transform.position, Quaternion.identity);
			}
			flaming.transform.parent = transform;
			corecontrol.rampage = true;
			StartCoroutine(RampageForTime());
		}
	}

	[PunRPC]
	void KnockBack(){
		if (canKnockBack && !animator.GetCurrentAnimatorStateInfo (0).IsName ("AB2")&&!animator.GetCurrentAnimatorStateInfo(0).IsName("Die")) {
			canKnockBack = false;
			if (animator) {
				animator.SetTrigger("Ability2");
			}
			Collider[] enemies = Physics.OverlapSphere (transform.position, StunDistance,layerMask, QueryTriggerInteraction.Collide);
			if (enemies.Length > 0) {
				for (int i = 0; i < enemies.Length; i++) {
					if (enemies [i].tag == "CrabAlien") {
						enemy_animator = enemies [i].GetComponent<Animator> ();
						enemy_animator.SetTrigger ("stunned");
						StartCoroutine (WaitForStun ());
					} else if (enemies [i].tag == "SpiderBrain") {
						bc = enemies[i].GetComponentInChildren<brain_control> ();
						bc.stunned = true;
						StartCoroutine (WaitForStunBrain ());
					}
				}

			}
			if (!PhotonNetwork.connected || photonView.isMine) {
				
				// Start cooldown animation for UI skill image
				timer.startCooldownTimerUI (2);
				skillTimeStamp2 = Time.time + StunCooldown;
				StartCoroutine (WaitAbility2Use ());
			}

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
		corecontrol.rampage = false;    
        
		if (!PhotonNetwork.connected || photonView.isMine) {
			
			// Start cooldown animation for UI skill image
			timer.startCooldownTimerUI (1);
			skillTimeStamp1 = Time.time + RampageCooldown;

			StartCoroutine (WaitAbility1Use ());
		}
	}

	IEnumerator RampageForTime()
	{
		yield return new WaitForSeconds(RampageTime);
		if (flaming)
		{
			StopRampage();
			isFlaming = false;
		}
	}
		

	IEnumerator WaitAbility1Use()
	{
		yield return new WaitForSeconds(RampageCooldown);
		canRampage = true;
	}
	IEnumerator WaitAbility2Use()
	{
		yield return new WaitForSeconds(StunCooldown);
		canKnockBack = true;
	}

	IEnumerator WaitForStun()
	{
		yield return new WaitForSeconds(EnemyStunnedTime);
		enemy_animator.SetTrigger ("getUp");
	}
	IEnumerator WaitForStunBrain()
	{
		yield return new WaitForSeconds(EnemyStunnedTime);
		bc.stunned = false;
	}

    #region Inherited Methods

    public void Activate(SpecialAbility ability)
    {
        if (ability == SpecialAbility.Leadership)
        {
			if (photonView.isMine) {
				this.photonView.RPC ("Rampage", PhotonTargets.All, null);
			} else {
				Rampage ();
			}
        }
		if (ability == SpecialAbility.KnockBack)
		{
			if (photonView.isMine) {
				this.photonView.RPC ("KnockBack", PhotonTargets.All, null);
			} else {
				KnockBack ();
			}
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
            // TODO
        }
    }

    #endregion
}
