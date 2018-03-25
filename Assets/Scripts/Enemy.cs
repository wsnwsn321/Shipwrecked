using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PhotonView))]
public class Enemy : Photon.MonoBehaviour {
	//extends MonsterSpawnManager class to update monster count
	public MonsterSpawnManager spawnManager;
    private MonsterTypes monsterType;
    private Animator enemy_ani;
    private bool isDead;

	// We want Photon to sync this value, so we will serialize it
	[SerializeField]
	public float health = 50f;

    // Variables to keep track of recent attackers.
    [HideInInspector]
    List<Transform> attackers;
    List<Transform> characterAttackers;
    List<float> times;
    Transform mostRecentCharacterAttacker;
    float secondsToKeepTrack = 2f;

    public List<Transform> Attackers
    {
        get { return attackers; }
    }
    
	void Start()
    {
        monsterType = GetComponent<EntityType>().monsterType;
        enemy_ani = GetComponent<Animator>();
        isDead = false;
        attackers = new List<Transform>();
        characterAttackers = new List<Transform>();
        times = new List<float>();
        InvokeRepeating("CheckAttackers", 0, 0.5f);
    }

    void CheckAttackers()
    {
        for (int i = 0; i < attackers.Count; i++)
        {
            if (Time.time > times[i] + secondsToKeepTrack)
            {
                attackers.RemoveAt(i);
                times.RemoveAt(i);
            }
        }
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

    public void TakeDamage(float amount){
		health -= amount;
		if (health <= 0f && !isDead) {
			Die ();
		}
	}

    public void AddAttacker(Transform attacker)
    {
        if (!attackers.Contains(attacker))
        {
            attackers.Add(attacker);
            times.Add(Time.time);
        }
        else
        {
            times[attackers.IndexOf(attacker)] = Time.time;
        }

        if (attacker.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            if (!characterAttackers.Contains(attacker))
            {
                characterAttackers.Add(attacker);
            }

            mostRecentCharacterAttacker = attacker;
        }
    }

    public List<Transform> GetCharacterAttackers()
    {
        return characterAttackers;
    }

    public Transform GetMostRecentCharacterAttacker()
    {
        return mostRecentCharacterAttacker;
    }

	void Die(){
        enemy_ani.SetTrigger("die");
        isDead = true;
        Destroy (gameObject,2f);
		UpdateMonsterAmount ();
        AllocateExp();
	}

    void AllocateExp()
    {
        int expToAllocate = 0;
        switch(monsterType)
        {
            case MonsterTypes.Brains:
                expToAllocate = 3;
                break;
            case MonsterTypes.Critter:
                expToAllocate = 2;
                break;
            case MonsterTypes.Spaz:
                expToAllocate = 2;
                break;
            case MonsterTypes.Tough:
                expToAllocate = 4;
                break;
        }

        for (int i = 0; i < characterAttackers.Count; i++)
        {
            // Allocate exp.
            characterAttackers[i].GetComponent<Experience>().IncreaseBy(expToAllocate);
        }

        // Allocate more to most recent attacker.
        mostRecentCharacterAttacker.GetComponent<Experience>().IncreaseBy(expToAllocate / 2);
    }

	//updates the amount of monsters remaining
	void UpdateMonsterAmount(){
		spawnManager.numMonsters--;
	}

}
