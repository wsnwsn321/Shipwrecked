using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreControl : Photon.PunBehaviour {

    [HideInInspector]
    public float nextTimeToFire = 0f;
    private float currentFireTime = 0f;

    [HideInInspector]
    public float damageModifier;

    public float fireRate = 15f;

    public float forwardMovement, horizontalMovement, timeScale;
    public float forwardSpeed, horizontalSpeed;
	public bool aiming, turnLeft, turnRight, sprint, isGrounded, turn, isMoving;
	public float distance;

    [HideInInspector]
    public  Animator allie_ani;
	private CoreControl allie_core;
	private PlayerHealth allie_health;
	public bool dead, hasSpecialAbility,rampage,autoRifle;
	public Animator animator;
    private Rigidbody rb;
	private PlayerHealth myhp;

    [HideInInspector]
	public AmmoRemaining ammo;

	public AudioClip footstepAudio;

    void Start () {
        damageModifier = 1f;
        forwardMovement = 0;
        horizontalMovement = 0;
        turnLeft = false;
        turnRight = false;
        sprint = false;
        isGrounded = true;
        turn = false;
        dead = false;
		autoRifle = false;
        hasSpecialAbility = false;
		rampage = false;
        forwardSpeed = 1f;
        horizontalSpeed = 1.5f;
        timeScale = 0.3f * Time.deltaTime;

		if (PlayerManager.LocalPlayerInstance == null) {
			Debug.Log ("ERROR in CoreControl! Local player instance not set!");
		}

		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		ammo = GetComponentInChildren<AmmoRemaining> ();
		myhp = GetComponent<PlayerHealth> ();
		if (PhotonNetwork.connected && this.gameObject.Equals (PlayerManager.LocalPlayerInstance)) {
			//myhp = GetComponent<PlayerHealth> ();
			//ammo = GetComponentInChildren<AmmoRemaining> ();
		} else if (!PhotonNetwork.connected) {
			myhp = GetComponent<PlayerHealth> ();
		}
    }

    public bool IsJumping()
    {
        return !isGrounded;
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public void GetMovement()
    {
        forwardMovement = Input.GetAxis("Vertical");
        horizontalMovement = Input.GetAxis("Horizontal");
    }

    public void SetLayerWeight(int layerIndex, float weight)
    {
        if (animator)
        {
            animator.SetLayerWeight(layerIndex, weight);
        }
    }

    #region States

    #region Can Action

    public bool CanAim()
    {
        return !CurrentStateTagIs(0, "aim_mode");
    }

    public bool CanJump()
    {
        return isGrounded;
    }

    public bool CanPickupObject()
    {
        return !CurrentStateNameIs(0, "Jump") && !CurrentStateNameIs(0, "Roll");
    }

    public bool CanReload()
    {
        return !CurrentStateNameIs(0, "Sprint") && !IsReloading();
    }

    public bool CanShoot()
    {
		bool canShoot = !CurrentStateNameIs(0, "Sprint") && !CurrentStateNameIs(0, "Roll") && !CurrentStateNameIs(0, "PickupObject")&&!CurrentStateNameIs(0,"AB2");
        return canShoot && !animator.GetBool("Shoot") && Time.time >= nextTimeToFire;
    }

    public bool CanStopShooting()
    {
        return Time.time >= nextTimeToFire;
    }

    #endregion Can Action

    #region Is Action

    public bool IsAiming()
    {
        return aiming;
    }

    public bool IsIdle()
    {
        return forwardMovement == 0 && horizontalMovement != 0f && !CurrentStateNameIs(0, "Shoot");
    }

    public bool IsInAimingMode()
    {
        return CurrentStateTagIs(0, "aim_mode") || CurrentStateTagIs(2, "aim_mode");
    }

    public bool IsReloading()
    {
		if (ammo) {
			return ammo.isReloading;

		} else {
			return false;
		}
    }

    public bool IsShooting()
    {
		return CurrentStateNameIs(0, "Shoot")||CurrentStateNameIs(0, "RampageShoot")||CurrentStateNameIs(0, "AutoShoot");
    }

    #endregion Is Action

    public bool CurrentStateNameIs(int layerIndex, string name)
    {
        if (animator)
        {
            return animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(name);
        }

        return false;
    }

    public bool CurrentStateTagIs(int layerIndex, string tag)
    {
        if (animator)
        {
            return animator.GetCurrentAnimatorStateInfo(layerIndex).IsTag(tag);
        }

        return false;
    }

    #endregion States

    #region Actions

    public void GetHit()
    {
        if (animator)
        {
            animator.SetTrigger("Hit");
        }
    }

    public void Jump()
    {
        if (animator)
        {
            if ((animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") || animator.GetCurrentAnimatorStateInfo(0).IsName("AIM")) && forwardMovement == 0 && horizontalMovement == 0&& !dead)
            {
                animator.SetTrigger("Jump");
            }
        }
		if (!dead) {
			rb.AddForce(new Vector3(0, 9f, 0), ForceMode.Impulse);
		}
        
        isGrounded = false;
    }

    public void Move()
    {
        Camera main_c = GetComponent<Control>().main_c;
		if (main_c && !dead&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("Reviving"))
        {
            Vector3 movement = new Vector3(horizontalMovement * Time.deltaTime * horizontalSpeed, 0.0f, forwardMovement * Time.deltaTime * forwardSpeed);

            movement = transform.TransformDirection(movement);
            rb.MovePosition(transform.position + movement);

            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && horizontalMovement == 0)
            {
                turn = true;
            }
            else
            {
                turn = false;
            }
        }
    }

    public void PickUpObject()
    {
		if (animator&&!dead&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("Reviving"))
        {
            animator.SetTrigger("Pickup");
        }
    }

    public void Reload()
    {
		if (animator && !dead&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("Reviving"))
        {
            animator.SetTrigger("Reload");
        }
		ammo.reload ();
    }

    public void Roll()
    {
		if (animator && !dead&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("Reviving")&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("AB2"))
        {
            animator.SetTrigger("Roll");
        }
    }

    public void Shoot()
    {
		if (!dead&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("Reviving")&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("AB2"))
        {
            //GetComponent<AudioSource>().PlayOneShot(Shoot);// play audio
            nextTimeToFire = Time.time + (2f / fireRate);
            /*if (shootingAudio)
            {
                AudioSource.PlayClipAtPoint(shootingAudio, transform.position, 1);
            }*/

			if (animator)
            {
                if (ammo.ammo != 0)
                {
					if (rampage) {
						animator.SetTrigger ("Rampageshoot");
					} else if (autoRifle) {
						animator.SetTrigger ("Auto");
					}
                    else
                    {
                        animator.SetTrigger("Shoot");
                        currentFireTime = 0f;
                    }
                }
            }
        }
        
    }

    public void Sprint()
    {
        sprint = true;
        forwardSpeed = 3.0f;
    }

    public void StartAiming()
    {
		if (!dead&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("Reviving")&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("AB2"))
        {
            aiming = true;
            if (animator)
            {
                animator.SetTrigger("AIM");
            }
            horizontalSpeed = 0.5f;
            forwardSpeed = 0.5f;
            if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 2f / fireRate;
                //	GetComponent<AudioSource>().PlayOneShot(Shoot);// play audio
                //AudioSource.PlayClipAtPoint(shootingAudio, transform.position, 1);

            }

            Control control = GetComponent<Control>();
            control.main_c.fieldOfView = 15;
            control.CamRef.transform.localPosition = new Vector3(0.6f, -1.1f, -1.2f);
            control.main_c.transform.localEulerAngles = new Vector3(0.04f, 9.667f, 0.2f);
        }
    }

    public void StopAiming()
    {
		if (!dead&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("Reviving")&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("AB2"))
        {
            aiming = false;
            if (animator)
            {
                animator.SetTrigger("UNAIM");
            }
            horizontalSpeed = 1.5f;
            forwardSpeed = 1.5f;

            Control control = GetComponent<Control>();
            control.main_c.fieldOfView = 35;
            control.CamRef.transform.localPosition = new Vector3(0.71f, -0, -0.22f);
            control.main_c.transform.localEulerAngles = new Vector3(18.41f, 9.667f, 0.2f);
        }
    }

    public void StopShooting()
    {
        if (animator)
        {
            print("Hit.");
			if (rampage) {
				animator.ResetTrigger ("Rampageshoot");
			} else {
				animator.ResetTrigger("Shoot");
			}
        }
        
    }

    public void Walk()
    {
        sprint = false;
        forwardSpeed = 1.5f;
		if (isGrounded) {
			//AudioSource.PlayClipAtPoint (footstepAudio, transform.position, 1);
		}
		//if (footstepAudio) {
		//	AudioSource.PlayClipAtPoint (footstepAudio, transform.position, 1);
		//}
    }

    public void DieOnGround()
    {
        if (animator)
        {
            dead = true;
			myhp.health = 0;
			myhp.updateHealthBar ();
			myhp.updateHealthText ();
        }
    }

	[PunRPC]
	public void CheckForRevival(Vector3 revPos) {
		if (Vector3.Distance(PlayerManager.LocalPlayerInstance.transform.position, revPos) < 3f) {
			// This means that this player is revived. Call Revived
			print("I'm revived!");
			Revived();
		}
	}

	[PunRPC]
	public void CheckForPillTarget(Vector3 pillPos) {
		// Physics can be faster instead of Vector distance!
		if (Vector3.Distance(PlayerManager.LocalPlayerInstance.transform.position, pillPos) < 3f) {
			// This means that this player is revived. Call Revived
			if (this.gameObject.GetComponent<PlayerHealth> ().health == 0) {
				// Player is dead, revive them
				Revived();
			} else {
				// Player is alive, heal them
				this.gameObject.GetComponent<PlayerHealth> ().health += 40;
				this.gameObject.GetComponent<PlayerHealth> ().updateHealthBar ();
				this.gameObject.GetComponent<PlayerHealth> ().updateHealthText ();
			}
		}
	}

	public void PillThrown(Vector3 pillPos) {
		photonView.RPC ("CheckForPillTarget", PhotonTargets.Others, pillPos);
	}


    public void Revived()
    {
        if (animator)
        {
			print ("reviving!!!!!");
            dead = false;
            animator.SetTrigger("Revived");
			this.gameObject.layer = 10;
			myhp.health = 10;
			myhp.updateHealthBar ();
			myhp.updateHealthText ();

        }
    }
	public void ReviveAllies()
	{
		if (animator)
		{
				animator.SetTrigger ("Help");
				StartCoroutine (animationDelay ());

		}

	}

	public void UnReviveAllies()
	{
		if (animator)
		{
			animator.SetTrigger ("Unrevive");
		}

	}
    public void LevelUp()
    {
        if (animator)
        {
            animator.SetTrigger("LevelUp");
        }

    }

    #endregion Actions

    private void Update()
    {
        if (CurrentStateNameIs(0, "Shoot"))
        {
            currentFireTime += Time.deltaTime;
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if (1 / fireRate < state.length)
            {
                float ratio = (1 / fireRate) / state.length;
                animator.speed = ratio;
            }
            print(currentFireTime);
            print(state.length * animator.speed);
            if (currentFireTime >= state.length * animator.speed)
            {
                StopShooting();
            }
        }
        else if (animator.speed != 1f)
        {
            animator.speed = 1f;
        }

        if (Time.time >= nextTimeToFire)
        {
            animator.SetBool("Shooting", false);
        }
    }

    public void UpdateAnimationStates()
    {
        if (animator)
        {
            animator.SetFloat("Speed", forwardMovement);
            animator.SetFloat("Y", horizontalMovement);
            animator.SetBool("Aiming", aiming);
            animator.SetBool("Sprint", sprint);
            //animator.SetBool("Turn", turn);
            animator.SetBool("Dead", dead);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }

		if (other.gameObject.tag == "Mechanic" || other.gameObject.tag == "Sarge" || other.gameObject.tag == "Doctor"||other.gameObject.tag == "Captain")
		{
			
			allie_ani = other.gameObject.GetComponent<Animator> ();
			allie_core = other.gameObject.GetComponent<CoreControl> ();
			allie_health = other.gameObject.GetComponent<PlayerHealth> ();
			distance = Vector3.Distance(transform.position, other.gameObject.transform.position);
		}

    }

	IEnumerator animationDelay( )
	{
		yield return new WaitForSeconds(5f);
		photonView.RPC("CheckForRevival", PhotonTargets.Others, PlayerManager.LocalPlayerInstance.transform.position);
		animator.SetTrigger ("FinishRevive");
		//allie_core.Revived ();
	}
}
