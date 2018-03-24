using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreControl : MonoBehaviour {

    [HideInInspector]
    public float nextTimeToFire = 0f;

    public float fireRate = 15f;

    public AudioClip shootingAudio;

    float forwardMovement, horizontalMovement, timeScale;
    float forwardSpeed, horizontalSpeed;
    bool aiming, turnLeft, turnRight, sprint, isGrounded, turn,dead;
	public float distance;
	Animator ani;

    private Animator animator;
    private Rigidbody rb;

    void Start () {
        forwardMovement = 0;
        horizontalMovement = 0;
        turnLeft = false;
        turnRight = false;
        sprint = false;
        isGrounded = true;
        turn = false;
        dead = false;
        forwardSpeed = 1f;
        horizontalSpeed = 1.5f;
        timeScale = 0.3f * Time.deltaTime;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
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
        return !CurrentStateNameIs(0, "Sprint");
    }

    public bool CanShoot()
    {
        bool canShoot = !CurrentStateNameIs(0, "Sprint") && !CurrentStateNameIs(0, "Roll") && !CurrentStateNameIs(0, "PickupObject");
        return canShoot && Time.time >= nextTimeToFire;
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
        return CurrentStateTagIs(1, "RELOAD");
    }

    public bool IsShooting()
    {
        return CurrentStateTagIs(1, "FIRE");
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
        rb.AddForce(new Vector3(0, 9f, 0), ForceMode.Impulse);
        isGrounded = false;
    }

    public void Move()
    {
        Camera main_c = GetComponent<Control>().main_c;
        if (main_c && !dead)
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
        if (animator&&!dead)
        {
            animator.SetTrigger("Pickup");
        }
    }

    public void Reload()
    {
        if (animator && !dead)
        {
            animator.SetTrigger("Reload");
        }
		gameObject.GetComponentInChildren<AmmoRemaining> ().reload ();
    }

    public void Roll()
    {
        if (animator && !dead)
        {
            animator.SetTrigger("Roll");
        }
    }

    public void Shoot()
    {
        if (!dead)
        {
            //GetComponent<AudioSource>().PlayOneShot(Shoot);// play audio
            nextTimeToFire = Time.time + (2f / fireRate);
            if (shootingAudio)
            {
                AudioSource.PlayClipAtPoint(shootingAudio, transform.position, 1);
            }

            if (animator)
            {
                animator.SetTrigger("Shoot");
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
        if (!dead)
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
                AudioSource.PlayClipAtPoint(shootingAudio, transform.position, 1);

            }

            Control control = GetComponent<Control>();
            control.main_c.fieldOfView = 15;
            control.CamRef.transform.localPosition = new Vector3(0.6f, -1.1f, -1.2f);
            control.main_c.transform.localEulerAngles = new Vector3(0.04f, 9.667f, 0.2f);
        }
        
    }

    public void StopAiming()
    {
        if (!dead)
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
            animator.ResetTrigger("Shoot");
        }
        
    }

    public void Walk()
    {
        sprint = false;
        forwardSpeed = 1.5f;
    }

    public void DieOnGround()
    {
        if (animator)
        {
            dead = true;
        }
    }

    public void Revived()
    {
        if (animator)
        {
            dead = false;
            animator.SetTrigger("Revived");
        }
    }
	public void ReviveAllies()
	{
		if (animator)
		{
			animator.SetTrigger("Help");
		}

	}

    #endregion Actions

    public void UpdateAnimationStates()
    {
        if (animator)
        {
            animator.SetFloat("Speed", forwardMovement);
            animator.SetFloat("Y", horizontalMovement);
            animator.SetBool("Aiming", aiming);
            animator.SetBool("Sprint", sprint);
            animator.SetBool("Turn", turn);
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
			ani = other.gameObject.GetComponent<Animator> ();
			distance = Vector3.Distance(transform.position, other.gameObject.transform.position);
		}

    }
}
