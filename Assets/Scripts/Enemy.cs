using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PhotonView))]
public class Enemy : Photon.PunBehaviour {
	//extends MonsterSpawnManager class to update monster count
	public MonsterSpawnManager spawnManager;
    private MonsterTypes monsterType;
	private Animator crab_ani;
	private brain_control bc;
	// We want Photon to sync this value, so we will serialize it
	[SerializeField]
	public float health;

    // Variables to keep track of recent attackers.
    [HideInInspector]
	public bool isDead;
    List<Transform> attackers;
    List<Transform> characterAttackers;
    List<float> times;
    Transform mostRecentCharacterAttacker;
    float secondsToKeepTrack = 2f;
	public AudioClip deadCrabAudio;
	public AudioClip deadBrainAudio;


    public List<Transform> Attackers
    {
        get { return attackers; }
    }

	void Start()
    {
		health = 50f;
        EntityType type = GetComponent<EntityType>();
        if (type)
        {
            monsterType = type.monsterType;
        }
		if (this.tag == "CrabAlien") {
			crab_ani = GetComponent<Animator> ();
		} else if (this.tag == "SpiderBrain") {
			bc = GetComponentInChildren<brain_control> ();
		}

        isDead = false;
        attackers = new List<Transform>();
        characterAttackers = new List<Transform>();
        times = new List<float>();
        InvokeRepeating("CheckAttackers", 0, 0.1f);
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
			Debug.Log ("Sending current health of enemy to others: " + health + " health");
			stream.SendNext (health);
		}
		else
		{
			health = (float)stream.ReceiveNext();
			Debug.Log ("Receiving current health of enemy from others: " + health + " health");
		}

		if (health <= 0f && !isDead) {
			Die ();
		}
	}

    public void TakeDamage(float amount){
		if (!PhotonNetwork.connected) {
			ReduceHealth (amount);
		} else {
			photonView.RPC ("ReduceHealth", PhotonTargets.All, amount);

		}
	}

	[PunRPC]
	private void ReduceHealth(float amount) {
		Debug.Log ("Enemy is taking " + amount + " damage!");
		health -= amount;
		if (health <= 0f && !isDead)
		{
			Die();
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
            AddCharacterAttacker(attacker);
            mostRecentCharacterAttacker = attacker;
        }
    }

    public void AddCharacterAttacker(Transform attacker)
    {
        if (attacker.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            if (!characterAttackers.Contains(attacker))
            {
                characterAttackers.Add(attacker);
            }
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

	void Die() {
		if (this.tag == "CrabAlien") {
			crab_ani.SetTrigger("die");
			AudioSource.PlayClipAtPoint (deadCrabAudio, transform.position, 2);
		} else if (this.tag == "SpiderBrain") {
			bc.dead = true;
			AudioSource.PlayClipAtPoint (deadBrainAudio, transform.position, 2);
		}
        isDead = true;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Dead");
        }

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
                expToAllocate = 6;
                break;
            case MonsterTypes.Critter:
                expToAllocate = 4;
                break;
            case MonsterTypes.Spaz:
                expToAllocate = 2;
                break;
            case MonsterTypes.Tough:
                expToAllocate = 4;
                break;
            default:
                expToAllocate = 10;
                break;
        }

        for (int i = 0; i < characterAttackers.Count; i++)
        {
            // Allocate exp.
            characterAttackers[i].GetComponent<Experience>().IncreaseBy(expToAllocate);
        }

        // Allocate more to most recent attacker.
        if (mostRecentCharacterAttacker)
        {
            mostRecentCharacterAttacker.GetComponent<Experience>().IncreaseBy(expToAllocate / 2);
        }
    }

	//updates the amount of monsters remaining
	void UpdateMonsterAmount(){
		spawnManager.numMonsters--;
	}

}
