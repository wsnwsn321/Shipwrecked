using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAbilities;

[RequireComponent(typeof(EntityType))]
public class Control : Photon.MonoBehaviour {

	[HideInInspector]
    public Camera main_c;

	[HideInInspector]
    public GameObject CamRef;

    [HideInInspector]
    public TeammateTypes characterType;

    private Vector3 camera_position, camera_rotation;
    CoreControl coreControl;
    IClassControl classControl;
    private Animator ani;
    //test only
    private Experience exp;


	private void InitControl() {
		exp = GetComponent<Experience>();
		EntityType type = GetComponent<EntityType>();

		if (type.type == EntityTypes.Teammate)
		{
			coreControl = GetComponent<CoreControl>();

			// Cringey code
			coreControl.LateStart ();

			ani = GetComponent<Animator>();
			switch(type.teammateType)
			{
			case TeammateTypes.Captain:
				classControl = GetComponent<CaptainControl>();
				characterType = TeammateTypes.Captain;
				break;
			case TeammateTypes.Doctor:
				classControl = GetComponent<DoctorControl>();
				characterType = TeammateTypes.Doctor;
				break;
			case TeammateTypes.Engineer:
				classControl = GetComponent<MechanicControl>();
				characterType = TeammateTypes.Engineer;
				break;
			case TeammateTypes.Sergeant:
				classControl = GetComponent<SergeantControl>();
				characterType = TeammateTypes.Sergeant;
				break;
			}
		}
		else
		{
			Debug.LogError("Object is not a player type.");
		}
		//    }
		//
		//	void LateStart()
		//    {
		camera_position = CamRef.transform.localPosition;
	}

    // Update is called once per frame
    private void Update()
    {
		// We only want to update our character! Added on 2/6/18
		if (photonView.isMine == false && PhotonNetwork.connected == true)
		{
			return;
		}
		if (classControl != null) {
			classControl.UpdateActions (Time.deltaTime);
		} else {
			Debug.Log ("Assigning class control in update...");
			InitControl ();
		}
    }

    // Update is called once per frame
    void FixedUpdate () {
		// We only want to update our character! Added on 2/6/18
		if (photonView.isMine == false && PhotonNetwork.connected == true)
        {
			return;
		}

        coreControl.GetMovement();
        coreControl.UpdateAnimationStates();
        
        classControl.UpdateAnimationStates(coreControl.GetAnimator());

        // Set the Layer Weights for the Idle state.
		if (coreControl.IsIdle() && classControl.CanIdle()&&!ani.GetCurrentAnimatorStateInfo(0).IsName("Die")&&!ani.GetCurrentAnimatorStateInfo (0).IsName ("Reviving")&&!ani.GetCurrentAnimatorStateInfo (0).IsName ("AB2"))
        {
            coreControl.SetLayerWeight(2, 1f);
        }
        else
        {
            coreControl.SetLayerWeight(2, 0f);
        }

        // Enter Aiming Mode
        if (InputManager.Aim() && coreControl.CanAim() && classControl.CanAim())
        {
            coreControl.StartAiming();
        }
        // Exit Aiming Mode
        else
        {
				coreControl.StopAiming();
        }

        // Ensure the layer is weighted 1 when shooting and reloading.
		if ((coreControl.IsReloading() && classControl.CanReload()) || coreControl.IsInAimingMode())
        {
            coreControl.SetLayerWeight(1, 1f);
        }
        else
        {
            coreControl.SetLayerWeight(1, 0f);
        }

        // Shoot
		if (this.tag == "Captain"||(coreControl.autoRifle)) {
			if (InputManager.ShootHeld() && coreControl.CanShoot() && classControl.CanShoot()) {
                coreControl.Shoot();
            }
		} else {
			if (InputManager.ShootHeld() && coreControl.CanShoot() && classControl.CanShoot()) {
                coreControl.Shoot();
            }
		}

        if (!InputManager.ShootHeld())
        {
            InputManager.UpdateRightTriggerInUse(false);
        }

        if (InputManager.ShootHeld() && coreControl.ammo.ammo <= 0 && !coreControl.IsShooting() && coreControl.CanReload() && classControl.CanReload())
        {
            coreControl.Reload();
        }

        // Reload
        if (InputManager.Reload() && coreControl.CanReload() && classControl.CanReload())
        {
            coreControl.Reload();
        }

        // Sprint
        if (InputManager.Sprint() && classControl.CanSprint())
        {
            coreControl.Sprint();
        }
        else
        {
            coreControl.Walk();
        }

        // Roll
        if (InputManager.Roll() && classControl.CanRoll())
        {
            coreControl.Roll();
        }

        // Jump
		if (InputManager.Jump() && coreControl.CanJump() && classControl.CanJump()&&!ani.GetCurrentAnimatorStateInfo (0).IsName ("Reviving"))
        {
            coreControl.Jump();
        }

        // Stop current special action.
        if (InputManager.StopAbility())
        {
            classControl.StopAction();
        }

        classControl.FixedUpdateActions(Time.deltaTime);

        // Activate Ability 1
		if (InputManager.Ability1() && !coreControl.IsAiming() && classControl.CanUseAbility1())
        {
            classControl.Activate(SpecialAbility.ThrowPill);
			classControl.Activate(SpecialAbility.MakeGhostTurret);
            classControl.Activate(SpecialAbility.HealSelf);
            classControl.Activate(SpecialAbility.Leadership);
        }

		// Activate Ability 2
		if (InputManager.Ability2() && !coreControl.IsAiming())
		{
            if (classControl.OverrideAbility2())
            {
                classControl.Activate(SpecialAbility.Build);
            }
            else if (coreControl.hasSpecialAbility && classControl.CanUseAbility2())
            {
                classControl.Activate(SpecialAbility.HealingCircle);
                classControl.Activate(SpecialAbility.KnockBack);
				classControl.Activate(SpecialAbility.RepairShip);
				classControl.Activate (SpecialAbility.AutoRifle);
            }
		}

        // Moving
        if (classControl.CanMove())
        {
            coreControl.Move();
        }

        //Enter dead state, for test
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    coreControl.DieOnGround();
        //}

        //revived by allies, for test
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    if (coreControl.canReviveSelf)
        //    {

        //        coreControl.canReviveSelf = false;
        //        coreControl.Revived();
        //    }
        //}

        //cheat on experience
        if (Input.GetKeyDown(KeyCode.Z))
        {
            exp.IncreaseBy(20);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            EnemyStats.Critter.Damage += 1;
        }

        //revive allies
        if (InputManager.ReviveAlly())
		{
			if (coreControl.distance < 1.2f) {
				if (!ani.GetCurrentAnimatorStateInfo (0).IsName ("Reviving")) {
					if (coreControl.allie_ani.GetCurrentAnimatorStateInfo (0).IsName ("Die")) {
						coreControl.ReviveAllies ();
					}
				} else {
					coreControl.UnReviveAllies ();
				}
			}
    	}

        // Emotes
        // Reach down.
        if (InputManager.Emote1() && coreControl.CanUseEmote() && !coreControl.dead)
        {
            coreControl.PickUpObject();
        }

        // Lean back.
        if (InputManager.Emote2() && coreControl.CanUseEmote() && !coreControl.dead)
        {
            coreControl.GetHit();
        }

        // Jump without jumping.
        if (InputManager.Emote3() && coreControl.CanUseEmote() && !coreControl.dead)
        {
            coreControl.GetOffGround();
        }

        // Raise hands.
        if (InputManager.Emote4() && coreControl.CanUseEmote() && !coreControl.dead)
        {
            coreControl.LevelUp();
        }
    }
}
