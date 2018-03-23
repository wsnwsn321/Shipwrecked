using Pathfinding;
using System;
using UnityEngine;

public class MonsterBehaviors : GenericBehaviors
{
    // Reference scripts attached to the AI.
    IAstarAI ai;
    FieldOfView fov;
    Seeker seeker;
    BehaviorManager bm;

    // The previous destination the AI was going towards.
    Vector3 previousDestination;

    // True if the AI is pursuing a target.
    bool isPursuing;
    bool overridePath;

    float wanderTime;
    float maxWanderTime = 5f;

    MonsterBehaviors specificBehavior;

    void Start()
    {
        ai = GetComponent<IAstarAI>();
        fov = GetComponent<FieldOfView>();
        seeker = GetComponent<Seeker>();
        bm = GetComponent<BehaviorManager>();
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
        // If there are targets to pursue, then pursue.
        if (fov.visibleTargets.Count > 0)
        {
            Pursue.Nearest(fov, ai, seeker, previousDestination, isPursuing);
            if (!isPursuing)
            {
                wanderTime = 0;
                isPursuing = true;
            }
        }
        else
        {
            if (wanderTime > maxWanderTime)
            {
                wanderTime -= maxWanderTime;
                overridePath = true;
            } else
            {
                wanderTime += Time.deltaTime;

                float wanderChance = UnityEngine.Random.Range(0, 1);

                if (wanderChance <= bm.wanderAheadChance)
                {
                    if (overridePath)
                    {
                        Wander.OverrideAhead(ai, seeker, bm.wanderAheadAngle, bm.wanderAheadDistance, bm.wanderAheadRadius);
                        overridePath = false;
                    } else
                    {
                        Wander.Ahead(ai, seeker, bm.wanderAheadAngle, bm.wanderAheadDistance, bm.wanderAheadRadius);
                    }
                } else
                {
                    if (overridePath)
                    {
                        Wander.OverrideRandomly(ai, seeker, bm.wanderRandomlyRadius);
                        overridePath = false;
                    } else
                    {
                        Wander.Randomly(ai, seeker, bm.wanderRandomlyRadius);
                    }
                }
                
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
}
