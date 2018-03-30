using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAbilities;

namespace PlayerAbilities
{
    public enum SpecialAbility
    {
        // Name Abilities in the following order:
        // Mechanic, Sergeant, Captain, Doctor
        // Note: the order doesn't actually matter, but it's good for consistency and quick understanding.

        // Ability 1
        MakeGhostTurret,
        HealSelf,
        Leadership,
        ThrowPill,
        
        // Ability 2
		Build,
        HealingCircle,
		KnockBack
    }
}

public interface IClassControl {

    void Activate(SpecialAbility ability);

    bool CanAim();

    bool CanIdle();

    bool CanJump();

    bool CanMove();

    bool CanPickUpObject();

    bool CanReload();

    bool CanRoll();

    bool CanShoot();

    bool CanSprint();

	bool CanUseAbility1();

	bool CanUseAbility2();

    bool OverrideAbility2();

    void FixedUpdateActions(float deltaTime);

    void StopAction();

    void UpdateActions(float deltaTime);

    void UpdateAnimationStates(Animator animator);
}
