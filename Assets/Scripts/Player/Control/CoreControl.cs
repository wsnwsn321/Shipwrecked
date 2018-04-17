using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreControl : Photon.PunBehaviour {

    [HideInInspector]
    public float nextTimeToFire = 0f;
    private float currentFireTime;

    [HideInInspector]
    public float damageModifier;

    [HideInInspector]
    public float fireRate;

    public float forwardMovement, horizontalMovement, timeScale;
    public float forwardSpeed, horizontalSpeed;
	public bool aiming, sprint, isGrounded, isMoving;
	public float distance;
	public bool canReviveSelf;
    [HideInInspector]
    public  Animator allie_ani;
	private CoreControl allie_core;
	private PlayerHealth allie_health;
	public bool dead, hasSpecialAbility,rampage,autoRifle;
	public Animator animator;
    private Rigidbody rb;
	private PlayerHealth myhp;
    private ReloadBar reloadBar;
    private float reloadCooldown;

    private float reloadDelay;
    private float currentReloadDelay;
    private bool canReload;

    private float emoteDelay;
    private float currentEmoteDelay;
    private bool canUseEmote;

    [HideInInspector]
	public AmmoRemaining ammo;
    [HideInInspector]
    NewGun gun;

    [HideInInspector]
    FreeLookCam cam;
    float originalTurnSpeed;

	public AudioClip footstepAudio;

    void Start () {
        damageModifier = 1f;
        forwardMovement = 0;
        horizontalMovement = 0;
        sprint = false;
        isGrounded = true;
        dead = false;
		autoRifle = false;
        hasSpecialAbility = false;
		canReviveSelf = false;
		rampage = false;
        forwardSpeed = 1f;
        horizontalSpeed = 1.5f;
        timeScale = 0.3f * Time.deltaTime;

		if (PlayerManager.LocalPlayerInstance == null && (!PhotonNetwork.connected || photonView.isMine)) {
			Debug.Log ("ERROR in CoreControl! Local player instance not set!");
		}

		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		ammo = GetComponentInChildren<AmmoRemaining> ();
        gun = GetComponentInChildren<NewGun>();
		myhp = GetComponent<PlayerHealth> ();
        cam = GetComponent<FreeLookCam>();

        originalTurnSpeed = cam.m_TurnSpeed;
        reloadBar = GetComponentInChildren<ReloadBar>();
        reloadBar.setInactive();

        reloadDelay = 0.5f;
        currentReloadDelay = 0.5f;
        canReload = true;
        reloadCooldown = 1.5f;

        emoteDelay = 0.5f;
        currentEmoteDelay = 0.5f;
        canUseEmote = true;

        rollDelay = 0.5f;
        currentRollDelay = 0.5f;
        canRoll = true;

        fireRate = gun.fireRate;
        currentFireTime = 2 / fireRate;
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
        forwardMovement = InputManager.MovementForward();
        horizontalMovement = InputManager.MovementLateral();
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
        return true;
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
        return !CurrentStateNameIs(0, "Sprint") && !IsReloading() && canReload;
    }

    public bool CanShoot()
    {
		bool canShoot = !CurrentStateNameIs(0, "Sprint") && !CurrentStateNameIs(0, "Roll") && !CurrentStateNameIs(0, "PickupObject")&&!CurrentStateNameIs(0,"AB2");
        return canShoot && !animator.GetBool("Shoot") && Time.time >= nextTimeToFire;
    }

    public bool CanStopShooting()
    {
		return !IsShooting();
    }

    public bool CanUseEmote()
    {
        return canUseEmote;
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
            if (canUseEmote)
            {
                StartCoroutine(DelayEmotes());
            }
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
        }
    }

    public void PickUpObject()
    {
		if (animator&&!dead&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("Reviving"))
        {
            animator.SetTrigger("Pickup");
            StartCoroutine(DelayEmotes());
        }
    }

    public void Reload()
    {
		if (animator && !dead&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("Reviving"))
        {
            animator.SetTrigger("Reload");
        }
        reloadBar.setActive();
        reloadBar.startReloadBar(reloadCooldown, Time.time + reloadCooldown);
		ammo.reload ();
    }

    IEnumerator DelayReloading()
    {
        canReload = false;
        while (currentReloadDelay < reloadDelay)
        {
            yield return new WaitForFixedUpdate();
            currentReloadDelay += Time.fixedDeltaTime;
        }
        canReload = true;
    }

    public void Roll()
    {
		if (animator && !dead&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("Reviving")&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("AB2"))
        {
            if (canRoll)
            {
                StartCoroutine(DelayRolling());
                animator.SetTrigger("Roll");
            }
        }
    }

    float rollDelay;
    float currentRollDelay;
    bool canRoll;
    IEnumerator DelayRolling()
    {
        canRoll = false;
        currentRollDelay = 0;
        while (currentRollDelay < rollDelay)
        {
            yield return new WaitForFixedUpdate();
            currentRollDelay += Time.fixedDeltaTime;
        }
        canRoll = true;
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
                if (ammo.ammo != 0 && currentFireTime >= 2 / fireRate)
                {
					if (((CompareTag ("Captain") || autoRifle) && InputManager.ShootHeld()) || InputManager.ShootDown()) {
						if (rampage) {
							animator.SetTrigger ("Rampageshoot");
						} else if (autoRifle) {
							animator.SetTrigger ("Auto");
						}
						else
						{
							animator.SetTrigger("Shoot");
							animator.SetBool ("Shooting", true);
						}

                        currentReloadDelay = 0f;
                        if (canReload)
                        {
                            StartCoroutine(DelayReloading());
                        }
                        
                        StartCoroutine(DelayShooting());

                        gun.ShootGun();
					}
                }
            }
        }
    }

    IEnumerator DelayShooting()
    {
        currentFireTime = 0f;
        while (currentFireTime < 2 / fireRate)
        {
            yield return new WaitForFixedUpdate();
            currentFireTime += Time.fixedDeltaTime;
        }
    }

    public void Sprint()
    {
        sprint = true;
        forwardSpeed = 3.0f;
    }

    public void StartAiming()
    {
		if (!aiming&&!dead&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("Reviving")&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("AB2"))
        {
            aiming = true;
            if (animator)
            {
                animator.SetTrigger("AIM");
            }
            horizontalSpeed = 0.5f;
            forwardSpeed = 0.5f;
            cam.m_TurnSpeed = 1.75f;

            Control control = GetComponent<Control>();
            control.main_c.fieldOfView = 15;
            control.CamRef.transform.localPosition = new Vector3(0.71f, 0, -1.2f);
        }
    }

    public void StopAiming()
    {
		if (aiming&&!dead&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("Reviving")&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("AB2"))
        {
            aiming = false;
            if (animator)
            {
                animator.SetTrigger("UNAIM");
            }
            horizontalSpeed = 1.5f;
            forwardSpeed = 1.5f;
            cam.m_TurnSpeed = originalTurnSpeed;

            Control control = GetComponent<Control>();
            control.main_c.fieldOfView = 35;
            control.CamRef.transform.localPosition = new Vector3(0.71f, 0, -0.22f);
        }
    }

    public void StopShooting()
    {
        if (animator)
        {
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
		if (animator && (!PhotonNetwork.connected || PlayerManager.LocalPlayerInstance.Equals(this.gameObject)))
        {
			if (myhp.GetHealth () > 0) {
				myhp.TakeDamage (myhp.GetHealth ());
			}
            dead = true;
			photonView.RPC ("FallDead", PhotonTargets.All, null);
        }
    }

    public void GetOffGround()
    {
        if (animator && (!PhotonNetwork.connected || PlayerManager.LocalPlayerInstance.Equals(this.gameObject)))
        {
            animator.SetTrigger("Jump");
            StartCoroutine(DelayEmotes());
        }
    }

	[PunRPC]
	public void CheckForRevival(Vector3 revPos) {
		if (Vector3.Distance(PlayerManager.LocalPlayerInstance.transform.position, revPos) < 5f) {
			// This means that this player is revived. Call Revived
			print("I'm revived!");
			//canReviveSelf = true;
			Revived();
		}
	}

	[PunRPC]
	public void CheckForPillTarget(Vector3 pillPos) {
		// Physics can be faster instead of Vector distance!
		if (Vector3.Distance(PlayerManager.LocalPlayerInstance.transform.position, pillPos) < 3f) {
			// This means that this player is revived. Call Revived
			if (myhp.GetHealth() == 0) {
				// Player is dead, revive them
				Revived();
			} else {
				// Player is alive, heal them
				myhp.RecoverHealth(40);
			}
		}
	}


    public void Revived()
    {
        if (animator)
        {
			print ("reviving!!!!!");
            dead = false;
			photonView.RPC ("ReviveSelf", PhotonTargets.All, null);
        }
    }

	[PunRPC]
	void ReviveSelf() {
		this.gameObject.layer = 10;
		animator.SetTrigger("Revived");
		myhp.RecoverHealth (10);
	}

	[PunRPC]
	void FallDead() {
		this.gameObject.layer = 16;
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
            if (canUseEmote)
            {
                StartCoroutine(DelayEmotes());
            }
        }

    }

    #endregion Actions

	private float shootingAnimationLength = 0f;
    private void Update()
    {
		if (PhotonNetwork.connected && !photonView.isMine) {
			return;
		}

		if (IsShooting())
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
		allie_core.WillBeRevived ();
		animator.SetTrigger ("FinishRevive");
	}


	public void WillBeRevived() {
		photonView.RPC("CheckForRevival", PhotonTargets.Others, PlayerManager.LocalPlayerInstance.transform.position);
	}

	public void WillBePilled(Vector3 pillPos) {
		photonView.RPC("CheckForPillTarget", PhotonTargets.Others, pillPos);
	}

    IEnumerator DelayEmotes()
    {
        canUseEmote = false;
        currentEmoteDelay = 0;
        while (currentEmoteDelay < emoteDelay)
        {
            yield return new WaitForFixedUpdate();
            currentEmoteDelay += Time.fixedDeltaTime;
        }
        canUseEmote = true;
    }
}
