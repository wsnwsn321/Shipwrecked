using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PhotonView))]
public class Enemy : Photon.MonoBehaviour {
	//extends MonsterSpawnManager class to update monster count
	public MonsterSpawnManager spawnManager;
    private Animator enemy_ani;

	// We want Photon to sync this value, so we will serialize it
	[SerializeField]
	public float health = 50f;

    // Variables to keep track of recent attackers.
    [HideInInspector]
    List<Transform> attackers;
    List<float> times;
    float secondsToKeepTrack = 2f;

    public List<Transform> Attackers
    {
        get { return attackers; }
    }
    
	void Start()
    {
        enemy_ani = GetComponent<Animator>();
        attackers = new List<Transform>();
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
		if (health <= 0f) {
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
    }

	void Die(){
        enemy_ani.SetTrigger("die");
        Destroy (gameObject,2f);
		UpdateMonsterAmount ();
	}

	//updates the amount of monsters remaining
	void UpdateMonsterAmount(){
		spawnManager.numMonsters--;
	}

}
