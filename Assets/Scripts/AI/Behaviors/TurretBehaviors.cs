using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviors : GenericBehaviors
{
    List<Transform> guns;
    List<Transform> turningElements;

    private float shootingDelay = 1/3f;
    [HideInInspector]
    public float damage = 2f;
    [HideInInspector]
    public float health = 20f;
    [HideInInspector]
    public int turretLevel;

    private bool isGatling;
    private FieldOfView fov;
    private float maxRotationSpeed;
    private float idleRotationSpeed;
    private bool ableToRotate;
    private float currentShootingTime;
    private Transform previousTarget;

    // Gatling
    private bool isRotating;
    private bool isShooting;
    private float maxGunRotationSpeed = 1000f;
    private float gunRotationSpeed;

    private void Awake()
    {
        guns = new List<Transform>();
        turningElements = new List<Transform>();
        isGatling = false;
        maxRotationSpeed = 50f;
        idleRotationSpeed = 30f;
        ableToRotate = false;
        currentShootingTime = shootingDelay;

        isRotating = false;
        isShooting = false;

        GetTurningElements();
        GetGuns();
    }

    void OnEnable()
    {
        fov = GetComponentInChildren<FieldOfView>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            Destroy(gameObject, 1f);
        }
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
            if (!gun.name.Equals("View Visualization"))
            {
                guns.Add(gun);
            }
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
    }

    private IEnumerator Shoot()
    {
        isShooting = true;

        var target = fov.nearestTarget.GetComponent<Enemy>();
        if (!target)
        {
            target = fov.nearestTarget.GetComponentInParent<Enemy>();
        }

        target.AddAttacker(transform);

        foreach (Transform gun in guns)
        {
            StartCoroutine(KickBack(gun));
            target.TakeDamage(damage);
            target.AddAttacker(transform);
            yield return new WaitForSeconds(shootingDelay / guns.Count);
        }

        isShooting = false;
    }

    private IEnumerator RotateGuns()
    {
        int speedIterations = 200;
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

    private IEnumerator ShootGatling()
    {
        isShooting = true;

        var target = fov.nearestTarget.GetComponent<Enemy>();
        if (!target)
        {
            target = fov.nearestTarget.GetComponentInParent<Enemy>();
        }

        while (isRotating)
        {
            if (gunRotationSpeed >= 0.6 * maxGunRotationSpeed)
            {
                target.TakeDamage(damage);
                target.AddAttacker(transform);
            }

            yield return new WaitForSeconds(0.1f * (maxGunRotationSpeed / gunRotationSpeed));
        }

        isShooting = false;
    }

    public override void Attack()
    {
        if (currentShootingTime >= shootingDelay)
        {
            currentShootingTime -= shootingDelay;

            if (!isShooting)
            {
                if (isGatling)
                {
                    StartCoroutine(ShootGatling());
                }
                else
                {
                    StartCoroutine(Shoot());
                }

                previousTarget = fov.nearestTarget;
            } else
            {
                if (!previousTarget.Equals(fov.nearestTarget))
                {
                    isShooting = false;
                }
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
                if (isGatling && !isRotating && guns[0].gameObject.activeSelf)
                {
                    isRotating = true;
                    StartCoroutine(RotateGuns());
                }

                if (IsAttacking())
                {
                    TurnTowardsTarget(fov.nearestTarget);
                    if (IsPointingAt(fov.nearestTarget) && guns[0].gameObject.activeSelf)
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

	// This method is responsible for synchronizing the health of the enemy
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext (health);
		}
		else
		{
			health = (float)stream.ReceiveNext();
		}
	}


}
