using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {

    [Range(0, 10)]
    public float spawnRadius;

    public void spawnMonster(List<MonsterSpawnManager.MonsterChance> monsters, List<float> spawnChances, GameObject monsterContainer)
    {
        float chance = Random.value;

        int index = 0;
        while (chance > spawnChances[index])
        {
            index++;
        }

        Vector3 spawnPoint = Random.insideUnitSphere * spawnRadius;
        spawnPoint.y = 0;
        spawnPoint += transform.position;

		if (monsters [index].monster) {
			GameObject monster;
			if (PhotonNetwork.connected) {
				if (PhotonNetwork.isMasterClient) {
					monster = PhotonNetwork.Instantiate (monsters [index].monster.name, spawnPoint, transform.rotation, 0);
				} else {
					// This line is added to fix compiler errors "Use of unassigned monster". 
					monster = null;
				}
			} else {
				// Singleplayer case
				monster = Instantiate (monsters [index].monster, spawnPoint, transform.rotation);
			}
			// This if-statement protects from the case where a non-master client player tries to spawn a monster
			if (monster != null) {
				monster.transform.SetParent (monsterContainer.transform);
				monster.GetComponent<Enemy> ().spawnManager = gameObject.GetComponentInParent<MonsterSpawnManager> ();
			}
		}
    }
}
