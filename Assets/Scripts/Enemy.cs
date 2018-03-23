using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class Enemy : Photon.MonoBehaviour {
	//extends MonsterSpawnManager class to update monster count
	public MonsterSpawnManager spawnManager;
    private Animator enemy_ani;

	// We want Photon to sync this value, so we will serialize it
	[SerializeField]
	public float health = 50f;
    
	void Start()
    {
        enemy_ani = GetComponent<Animator>();
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
            print("called");
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
