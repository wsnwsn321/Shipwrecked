using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviors : GenericBehaviors
{
    List<Transform> guns;
    List<Transform> turningElements;

    private bool isGatling;
    private FieldOfView fov;
    private float maxRotationSpeed;
    private bool ableToRotate;

    private void Start()
    {
        guns = new List<Transform>();
        turningElements = new List<Transform>();
        isGatling = false;
        fov = GetComponentInChildren<FieldOfView>();
        maxRotationSpeed = 10f;
        ableToRotate = false;
        GetTurningElements();
        GetGuns();
    }

    // Assumes that the format of the turret is:
    // Turret --> Base --> (BaseElement, Pylon) --> PylonElement
    private void GetTurningElements()
    {
        Transform turretBase = transform.GetChild(0);

        // Get the BaseElement
        turningElements.Add(turretBase.GetChild(0));

        // Get the Pylon
        turningElements.Add(turretBase.GetChild(1));
    }

    private void GetGuns()
    {
        Transform turret = transform.GetChild(0).GetChild(1).GetChild(1);
        foreach (Transform gun in turret)
        {
            guns.Add(gun);
        }

        if (guns.Count == 1)
        {
            isGatling = guns[0].name.Equals("RotatingGuns");
        }
    }

    private void RotateTurret()
    {
        foreach (Transform turningElement in turningElements)
        {
            turningElement.Rotate(0, maxRotationSpeed * Time.deltaTime, 0);
        }
    }

    private void TurnTowardsTarget(Transform target)
    {
        foreach (Transform turningElement in turningElements)
        {
            var targetPosition = Quaternion.LookRotation(turningElement.position - target.position);
            turningElement.rotation = Quaternion.RotateTowards(turningElement.rotation, targetPosition, maxRotationSpeed * Time.deltaTime);
        }
    }

    private bool IsPointingAt(Transform target)
    {
        // TODO
        return false;
    }

    public override void Attack()
    {
        throw new NotImplementedException();
    }

    public override void Behave()
    {
        if (!ableToRotate)
        {
            bool canRotate = true;
            foreach (Transform turningElement in turningElements)
            {
                canRotate = canRotate && turningElement.gameObject.activeSelf;
            }
            ableToRotate = canRotate;
        } else
        {
            if (!IsAttacking())
            {
                if (IsIdle())
                {
                    Idle();
                }
                else
                {
                    behaviors.Add(Behaviors.Idle);
                }
            }

            if (fov.visibleTargets.Count > 0)
            {
                if (IsAttacking())
                {
                    TurnTowardsTarget(fov.nearestTarget);
                    if (IsPointingAt(fov.nearestTarget))
                    {
                        Attack();
                    }
                }
                else
                {
                    behaviors.Remove(Behaviors.Idle);
                    behaviors.Add(Behaviors.Attack);
                }
            }
            else
            {
                if (IsAttacking())
                {
                    behaviors.Remove(Behaviors.Attack);
                }
            }
        }
    }

    public override void Idle()
    {
        RotateTurret();
    }
}
