using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum Behaviors
{
    Attack,
    Idle,
    PursueNearest,
    PursueShip,
    PursueTarget,
    WanderAhead,
    WanderRandomly
}

[RequireComponent(typeof(EntityType))]
public class BehaviorManager : MonoBehaviour {

    GenericBehaviors behavior;

    public Transform spaceship;

    public float wanderRandomlyRadius;
    public float wanderAheadRadius;
    public float wanderAheadChance;
    public float wanderAheadDistance;
    public float wanderAheadAngle;

    void Awake()
    {
        EntityType types = GetComponent<EntityType>();
        EntityTypes type = types.type;
        if (type == EntityTypes.Monster)
        {
            gameObject.AddComponent<MonsterBehaviors>();

            switch (types.monsterType)
            {
                case MonsterTypes.Brains:
                    behavior = gameObject.AddComponent<BrainBehaviors>();
                    break;
                case MonsterTypes.Critter:
                    behavior = gameObject.AddComponent<CritterBehaviors>();
                    break;
                case MonsterTypes.Spaz:
                    behavior = gameObject.AddComponent<SpazBehaviors>();
                    break;
                case MonsterTypes.Tough:
                    behavior = gameObject.AddComponent<ToughBehaviors>();
                    break;
            }
        }
        else if (type == EntityTypes.Teammate)
        {
            gameObject.AddComponent<TeammateBehaviors>();

            switch (types.teammateType)
            {
                case TeammateTypes.Captain:
                    behavior = gameObject.AddComponent<CaptainBehaviors>();
                    break;
                case TeammateTypes.Doctor:
                    behavior = gameObject.AddComponent<DoctorBehaviors>();
                    break;
                case TeammateTypes.Engineer:
                    behavior = gameObject.AddComponent<EngineerBehaviors>();
                    break;
                case TeammateTypes.Sergeant:
                    behavior = gameObject.AddComponent<SergeantBehaviors>();
                    break;
            }
        }
        else if (type == EntityTypes.Turret)
        {
            behavior = gameObject.AddComponent<TurretBehaviors>();
        }
    }

    void Update()
    {
        behavior.Behave();
    }
}
