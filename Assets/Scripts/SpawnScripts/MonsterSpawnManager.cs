using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : Photon.MonoBehaviour {

    List<MonsterSpawner> SpawnPoints;
    List<float> spawnChances;

    public enum SpawnState
    {
        Spawning, Waiting, Counting, Finished
    }

    [System.Serializable]
    public class MonsterChance
    {
        public GameObject monster;

        // Determines the odds of picking this monster compared to the others.
        // If the odds of picking this monster are 3 and the odds are 15 in all,
        // then the probability of picking this monster is 3 / 15.
        public int spawnOdds = 1;
    }

    public float timeBetweenSpawns = 5f;
    [Range(1, 10)]
    public float spawnRate = 1;
    [Range(0, 200)]
    public int maxNumberMonsters = 10;
    public List<MonsterChance> monsters;

    [HideInInspector]
    public int originalMaxMonsters;

    [HideInInspector]
    public float spawnCountdown;
    [HideInInspector]
    public SpawnState state;
    [HideInInspector]
    public int numMonsters;

    GameObject monsterContainer;

    void Start () {
        if (PhotonNetwork.connected)
        {
            maxNumberMonsters = PhotonNetwork.playerList.Length * 6;
        }
        else
        {
            maxNumberMonsters = 8;
        }

        originalMaxMonsters = maxNumberMonsters;

        state = SpawnState.Counting;
        spawnCountdown = 0f;
        SpawnPoints = new List<MonsterSpawner>();
        spawnChances = new List<float>();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("MonsterSpawner"))
            {
                SpawnPoints.Add(child.GetComponent<MonsterSpawner>());
            }
        }

        float totalSpawnOdds = 0f;
        foreach (MonsterChance monster in monsters)
        {
            totalSpawnOdds += monster.spawnOdds;
        }

        if (totalSpawnOdds > 0)
        {
            int previousSpawnOdds = 0;
            foreach (MonsterChance monster in monsters)
            {
                if (monster.spawnOdds == 0)
                {
                    spawnChances.Add(-1f);
                } else
                {
                    int spawnOdds = monster.spawnOdds + previousSpawnOdds;
                    spawnChances.Add(spawnOdds / totalSpawnOdds);
                    previousSpawnOdds += monster.spawnOdds;
                }
            }
        }

        if (SpawnPoints.Count < 1)
        {
            Debug.LogError("There are no spawn points!");
        } else
        {
            monsterContainer = new GameObject();
            monsterContainer.name = "Monsters";
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (state == SpawnState.Waiting)
        {
            if (numMonsters < maxNumberMonsters)
            {
                state = SpawnState.Counting;
            }
        }

		if (spawnCountdown >= timeBetweenSpawns)
        {
            if (state == SpawnState.Counting)
            {
                StartCoroutine(SpawnMonsters());
            }
            if (state == SpawnState.Finished)
            {
                spawnCountdown -= timeBetweenSpawns;
                state = SpawnState.Counting;
            }
        } else
        {
            spawnCountdown += Time.deltaTime;
        }
	}

    IEnumerator SpawnMonsters()
    {
        state = SpawnState.Spawning;

        foreach (MonsterSpawner spawnPoint in SpawnPoints)
        {
            // If the spawn point has been deactivate, don't spawn anything at it.
            if (!spawnPoint.gameObject.activeSelf)
            {
                continue;
            }

            if (numMonsters < maxNumberMonsters)
            {
                spawnPoint.spawnMonster(monsters, spawnChances, monsterContainer);
                numMonsters++;
                yield return new WaitForSeconds(1f / spawnRate);
            } else
            {
                state = SpawnState.Waiting;
                yield break;
            }
        }

        state = SpawnState.Finished;

        yield break;
    }
}
