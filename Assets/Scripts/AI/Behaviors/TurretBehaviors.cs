using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviors : GenericBehaviors
{
    List<Transform> guns;
    List<Transform> turningElements;

    private float shootingDelay = 0.5f;

    private bool isGatling;
    private FieldOfView fov;
    private float maxRotationSpeed;
    private float idleRotationSpeed;
    private bool ableToRotate;
    private float currentShootingTime;

    // Gatling
    private bool isRotating;
    private float gunRotationSpeed;

    private void Start()
    {
        guns = new List<Transform>();
        turningElements = new List<Transform>();
        isGatling = false;
        fov = GetComponentInChildren<FieldOfView>();
        maxRotationSpeed = 50f;
        idleRotationSpeed = 30f;
        ableToRotate = false;
        currentShootingTime = shootingDelay;

        isRotating = false;

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
            turningElement.Rotate(0, idleRotationSpeed * Time.deltaTime, 0);
        }
    }

    private void TurnTowardsTarget(Transform target)
    {
        Quaternion targetDirection;
        Quaternion facingDirection;
        foreach (Transform turningElement in turningElements)
        {
            // Get the vector from the turning element to the target.
            Vector3 targetPosition = (target.position - turningElement.position);

            // Ignore the height difference.
            targetPosition.y = 0;

            targetDirection = Quaternion.LookRotation(targetPosition.normalized);
            facingDirection = turningElement.rotation * Quaternion.Euler(0, fov.angleOffset, 0);
            turningElement.rotation = Quaternion.Euler(0, -fov.angleOffset, 0) * Quaternion.RotateTowards(facingDirection, targetDirection, maxRotationSpeed * Time.deltaTime);
        }
    }

    private bool IsPointingAt(Transform target)
    {
        Vector3 facingDirection = (turningElements[0].rotation * Quaternion.Euler(0, fov.angleOffset, 0)).eulerAngles;
        Vector3 targetPosition = target.position - transform.position;
        targetPosition.y = 0;
        Vector3 targetDirection = Quaternion.LookRotation(targetPosition.normalized).eulerAngles;
        return Mathf.Abs(facingDirection.y - targetDirection.y) < 5;
    }

    private IEnumerator KickBack(Transform gun)
    {
        int kickbackIterations = 10;
        float maxKickbackDistance = 0.2f;
        float kickbackIncrement = maxKickbackDistance / kickbackIterations;
        float kickbackDistance = 0;

        while (kickbackDistance < maxKickbackDistance)
        {
            gun.position -= gun.right * kickbackIncrement;
            yield return new WaitForFixedUpdate();
            kickbackDistance += kickbackIncrement;
        }

        while (kickbackDistance > 0)
        {
            gun.position += gun.right * kickbackIncrement;
            yield return new WaitForFixedUpdate();
            kickbackDistance -= kickbackIncrement;
        }

        yield break;
    }

    private IEnumerator Shoot()
    {
        foreach (Transform gun in guns)
        {
            StartCoroutine(KickBack(gun));
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator RotateGuns()
    {
        int speedIterations = 200;
        float maxGunRotationSpeed = 1000f;
        float speedIncrement = maxGunRotationSpeed / speedIterations;

        while (isRotating)
        {
            if (gunRotationSpeed < maxGunRotationSpeed)
            {
                gunRotationSpeed += speedIncrement;
            }

            guns[0].Rotate(gunRotationSpeed * Time.deltaTime, 0, 0);

            yield return new WaitForFixedUpdate();
        }
        
        while (!isRotating && gunRotationSpeed > 0)
        {
            gunRotationSpeed -= speedIncrement;

            guns[0].Rotate(gunRotationSpeed * Time.deltaTime, 0, 0);

            yield return new WaitForFixedUpdate();
        }
    }

    public override void Attack()
    {
        if (currentShootingTime >= shootingDelay)
        {
            currentShootingTime -= shootingDelay;

            if (isGatling)
            {
                // Put code for shooting gatling here.
            }
            else
            {
                StartCoroutine(Shoot());
            }
        }
    }

    public override void Behave()
    {
        currentShootingTime += Time.deltaTime;

        if (currentShootingTime > shootingDelay)
        {
            currentShootingTime = shootingDelay;
        }

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
                if (isGatling && !isRotating)
                {
                    isRotating = true;
                    StartCoroutine(RotateGuns());
                }

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
                isRotating = false;

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
