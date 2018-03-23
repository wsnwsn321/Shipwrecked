using System;
using System.Collections;
using System.Collections.Generic;
using PlayerAbilities;
using UnityEngine;

public class DoctorControl : Photon.MonoBehaviour, IClassControl {

    public int maxPills = 1;
    public GameObject healParticle;
    public GameObject pill;

    private GameObject healing;
    private List<GameObject> pills;
    private bool heal;

    void Start()
    {
        heal = false;
        pills = new List<GameObject>();
    }

    void ThrowPill()
    {
        if (pills.Count < maxPills)
        {
            Animator animator = GetComponent<CoreControl>().GetAnimator();
            if (animator&&!animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                animator.SetTrigger("Ability1");
            }
            StartCoroutine(animationDelay());
			GameObject currentPill = PhotonNetwork.connected ? PhotonNetwork.Instantiate(pill.name, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity, 0) :Instantiate(pill, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
            Physics.IgnoreCollision(this.GetComponent<BoxCollider>(), currentPill.GetComponent<CapsuleCollider>());
            currentPill.GetComponent<Rigidbody>().velocity = GetComponent<Control>().main_c.transform.forward * 10;
            pills.Add(currentPill);

            StartCoroutine(waitPillDie());
        }
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

    IEnumerator animationDelay( )
    {
        yield return new WaitForSeconds(0.5f);
    }

    #region Inherited Methods

    public void Activate(SpecialAbility ability)
    {
        if (ability == SpecialAbility.ThrowPill)
        {
            ThrowPill();
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
