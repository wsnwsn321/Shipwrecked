using UnityEngine;

public enum EntityTypes
{
    Monster, Teammate, Turret
}

public enum MonsterTypes
{
    Brains, Critter, Tough, Spaz
}

public enum TeammateTypes
{
    Captain, Doctor, Engineer, Sergeant
}

public class EntityType : MonoBehaviour
{
    public EntityTypes type = EntityTypes.Monster;

    public MonsterTypes monsterType = MonsterTypes.Brains;

    public TeammateTypes teammateType = TeammateTypes.Captain;
}