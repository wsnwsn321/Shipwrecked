using System.Collections.Generic;
using UnityEngine;

public abstract class GenericBehaviors : Photon.MonoBehaviour
{
    [HideInInspector]
    public List<Behaviors> behaviors = new List<Behaviors>();

    #region IsBehavior

    public bool IsAttacking()
    {
        return behaviors.Contains(Behaviors.Attack);
    }

    public bool IsIdle()
    {
        return behaviors.Contains(Behaviors.Idle);
    }

    public bool IsPursuing()
    {
        return IsPursuingAttacker() || IsPursuingNearest() || IsPursuingShip() || IsPursuingTarget();
    }

    public bool IsPursuingAttacker()
    {
        return behaviors.Contains(Behaviors.PursueAttacker);
    }

    public bool IsPursuingNearest()
    {
        return behaviors.Contains(Behaviors.PursueNearest);
    }

    public bool IsPursuingShip()
    {
        return behaviors.Contains(Behaviors.PursueShip);
    }

    public bool IsPursuingTarget()
    {
        return behaviors.Contains(Behaviors.PursueTarget);
    }

    public bool IsWanderingAhead()
    {
        return behaviors.Contains(Behaviors.WanderAhead);
    }

    public bool IsWanderingRandomly()
    {
        return behaviors.Contains(Behaviors.WanderRandomly);
    }

    #endregion

    #region Abstract Methods

    public abstract void Attack();

    // The logic for action goes here.
    public abstract void Behave();

    public abstract void Idle();

    #endregion
}
