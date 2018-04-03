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
    void Start ()
    {
        exp = GetComponent<Experience>();
        EntityType type = GetComponent<EntityType>();

        if (type.type == EntityTypes.Teammate)
        {
            coreControl = GetComponent<CoreControl>();
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
    }

	void LateStart()
    {
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

        classControl.UpdateActions(Time.deltaTime);
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
        if (Input.GetMouseButtonUp(1) && coreControl.CanAim() && classControl.CanAim())
        {
            coreControl.StartAiming();
        }
        // Exit Aiming Mode
        else if(Input.GetMouseButtonUp(1) && coreControl.IsInAimingMode() && classControl.CanAim())
        {
            coreControl.StopAiming();
        }

        // Ensure the layer is weighted 1 when shooting and reloading.
        if ((coreControl.IsReloading() || coreControl.IsShooting()) && (classControl.CanReload() || classControl.CanShoot()))
        {
            coreControl.SetLayerWeight(1, 1f);
        }
        else
        {
            coreControl.SetLayerWeight(1, 0f);
        }

        // Shoot
		if (this.tag == "Captain") {
			if (Input.GetMouseButton (0)) {
                if (coreControl.CanShoot() && classControl.CanShoot())
                {
                    coreControl.Shoot();
                } else
                {
                    coreControl.StopShooting();
                }

                if (coreControl.ammo.ammo <= 0 && coreControl.CanReload() && classControl.CanReload())
                {
                    coreControl.Reload();
                }
            }
		} else {
			if (Input.GetMouseButtonDown (0)) {
                if (coreControl.CanShoot() && classControl.CanShoot())
                {
                    coreControl.Shoot();
                } else
                {
                    coreControl.StopShooting();
                }

                if (coreControl.ammo.ammo <= 0 && coreControl.CanReload() && classControl.CanReload())
                {
                    coreControl.Reload();
                }
            }
		}


        // Reload
        if (Input.GetKeyDown(KeyCode.R) && coreControl.CanReload() && classControl.CanReload())
        {
            coreControl.Reload();
        }

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift) && classControl.CanSprint())
        {
            coreControl.Sprint();
        }
        else
        {
            coreControl.Walk();
        }

        // Roll
        if (Input.GetKeyDown(KeyCode.C) && classControl.CanRoll())
        {
            coreControl.Roll();
        }

        // Jump
		if (Input.GetKeyDown(KeyCode.Space) && coreControl.CanJump() && classControl.CanJump()&&!ani.GetCurrentAnimatorStateInfo (0).IsName ("Reviving"))
        {
            coreControl.Jump();
        }

        // Pick Up Object
        if (Input.GetKeyDown(KeyCode.F) && coreControl.CanPickupObject() && classControl.CanPickUpObject())
        {
            coreControl.PickUpObject();
        }

        // Getting Hit.
        if (Input.GetKeyDown(KeyCode.H))
        {
            coreControl.GetHit();
        }

        // Stop current special action.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            classControl.StopAction();
        }

        classControl.FixedUpdateActions(Time.deltaTime);

        // Activate Ability 1
		if (Input.GetKeyDown(KeyCode.Q) && !coreControl.IsAiming() && classControl.CanUseAbility1())
        {
            classControl.Activate(SpecialAbility.ThrowPill);
			classControl.Activate(SpecialAbility.MakeGhostTurret);
            classControl.Activate(SpecialAbility.HealSelf);
            classControl.Activate(SpecialAbility.Leadership);
        }

		// Activate Ability 2
		if (Input.GetKeyDown(KeyCode.E) && !coreControl.IsAiming())
		{
            if (classControl.OverrideAbility2())
            {
                classControl.Activate(SpecialAbility.Build);
            }
            else if (coreControl.hasSpecialAbility && classControl.CanUseAbility2())
            {
                classControl.Activate(SpecialAbility.HealingCircle);
                classControl.Activate(SpecialAbility.KnockBack);
            }
		}

        // Moving
        if (classControl.CanMove())
        {
            coreControl.Move();
        }

        //Enter dead state, for test
        if (Input.GetKeyDown(KeyCode.K))
        {
            coreControl.DieOnGround();
        }

        //revived by allies, for test
        if (Input.GetKeyDown(KeyCode.N))
        {
            coreControl.Revived();
        }

        //cheat on experience
        if (Input.GetKeyDown(KeyCode.Z))
        {
            exp.IncreaseBy(20);
        }

        //revive allies
        if (Input.GetKeyDown(KeyCode.G))
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
	}
}
