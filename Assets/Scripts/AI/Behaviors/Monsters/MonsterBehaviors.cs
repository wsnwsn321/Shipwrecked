using Pathfinding;
using System;
using UnityEngine;
using System.Collections.Generic;

public class MonsterBehaviors : GenericBehaviors
{
    // Reference scripts attached to the AI.
    IAstarAI ai;
    FieldOfView fov;
    Seeker seeker;
    BehaviorManager bm;
    Enemy enemy;

    // The previous destination the AI was going towards.
    Vector3 previousDestination;

    // True if the AI is pursuing a target.
    bool isPursuing;
    bool overridePath;

    float wanderTime;
    float maxWanderTime = 5f;

    List<Transform> visibleCharacters;
    List<Transform> visibleTurrets;

    MonsterBehaviors specificBehavior;

    void Start()
    {
        ai = GetComponent<IAstarAI>();
        fov = GetComponent<FieldOfView>();
        seeker = GetComponent<Seeker>();
        bm = GetComponent<BehaviorManager>();
        enemy = GetComponent<Enemy>();
        previousDestination = Vector3.zero;
        isPursuing = false;
        overridePath = false;
        wanderTime = 0;
    }

    #region Actions

    public override void Attack()
    {
        throw new NotImplementedException();
    }

    public override void Behave()
    {
        if (enemy.Attackers.Count > 0)
        {
            Transform nearestAttacker = enemy.Attackers[0];
            float nearestDistance = Vector3.Distance(fov.transform.position, enemy.Attackers[0].position);
            for (int i = 1; i < enemy.Attackers.Count; i++)
            {
                float distance = Vector3.Distance(fov.transform.position, enemy.Attackers[i].position);
                if (distance < nearestDistance)
                {
                    nearestAttacker = enemy.Attackers[i];
                    nearestDistance = distance;
                }
            }

            Pursue.OverrideTarget(nearestAttacker.position, ai, seeker, previousDestination, isPursuing);

            behaviors.Clear();
            if (!isPursuing)
            {
                wanderTime = 0;
                isPursuing = true;
                behaviors.Add(Behaviors.PursueAttacker);
            }
        }
        else if (fov.visibleTargets.Count > 0)
        {
            /** This splits the visible targets up into characters and turrets. */
            //visibleCharacters.Clear();
            //visibleTurrets.Clear();
            //for (int i = 0; i < fov.visibleTargets.Count; i++)
            //{
            //    if (fov.visibleTargets[i].gameObject.layer == LayerMask.NameToLayer("Character"))
            //    {
            //        visibleCharacters.Add(fov.visibleTargets[i]);
            //    }
            //    else if (fov.visibleTargets[i].gameObject.layer == LayerMask.NameToLayer("TurretParent"))
            //    {
            //        visibleTurrets.Add(fov.visibleTargets[i]);
            //    }
            //}
            
            if (IsPursuingShip() || IsPursuingNearest())
            {
                Pursue.Nearest(fov, ai, seeker, previousDestination, isPursuing, overridePath: true);
            } else
            {
                Pursue.Nearest(fov, ai, seeker, previousDestination, isPursuing);
            }

            behaviors.Clear();
            if (!isPursuing)
            {
                wanderTime = 0;
                isPursuing = true;
                behaviors.Add(Behaviors.PursueNearest);
            }
        }
        else
        {
            if (BehaviorManager.spaceship && !IsPursuing())
            {
                Pursue.Target(BehaviorManager.spaceship.position, ai, seeker, previousDestination, isPursuing);

                behaviors.Clear();
                if (!isPursuing)
                {
                    wanderTime = 0;
                    isPursuing = true;
                    behaviors.Add(Behaviors.PursueShip);
                }
            }
            else if (wanderTime > maxWanderTime)
            {
                wanderTime -= maxWanderTime;
                overridePath = true;
            }
            else
            {
                behaviors.Clear();
                if (isPursuing)
                {
                    isPursuing = false;
                }
            }
        }

        // Update the previous destination to the newly determined destination.
        previousDestination = ai.destination;
    }

    public override void Idle()
    {
        throw new NotImplementedException();
    }

    #endregion

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("MonsterSpawner"))
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider, ignore: true);
        }
    }
}
