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

    public static Transform spaceship;

    public float wanderRandomlyRadius;
    public float wanderAheadRadius;
    public float wanderAheadChance;
    public float wanderAheadDistance;
    public float wanderAheadAngle;

    void Awake()
    {
        if (!spaceship)
        {
            spaceship = GameObject.FindGameObjectWithTag("Spaceship").transform;
        }
        EntityType types = GetComponent<EntityType>();
        EntityTypes type = types.type;
        switch(type)
        {
            case EntityTypes.Monster:
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
                break;
            case EntityTypes.Teammate:
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
                break;
			case EntityTypes.Turret:
				behavior = gameObject.AddComponent<TurretBehaviors> ();
				behavior.enabled = false;
				GetComponent<PhotonView> ().ObservedComponents.Add (behavior);
                break;
        }
    }

    void Update()
    {
        if (behavior.enabled)
        {
            behavior.Behave();
        }
    }
}
