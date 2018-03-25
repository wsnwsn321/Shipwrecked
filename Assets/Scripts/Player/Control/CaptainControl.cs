using System;
using System.Collections;
using System.Collections.Generic;
using PlayerAbilities;
using UnityEngine;

public class CaptainControl : MonoBehaviour, IClassControl
{
    [HideInInspector]
    public float attackBuff = 1f, defenseBuff = 1f;

    void Start()
    {
        // TODO
    }

    #region Inherited Methods

    public void Activate(SpecialAbility ability)
    {
        if (ability == SpecialAbility.Leadership)
        {
            print("Captain Ability 1 Activated!");
            // TODO
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
            // TODO
        }
    }

    #endregion
}
