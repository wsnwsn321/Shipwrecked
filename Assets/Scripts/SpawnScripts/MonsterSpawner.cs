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

        if (monsters[index].monster)
        {
            GameObject monster;
			if (PhotonNetwork.connected && PhotonNetwork.isMasterClient)
            {
                monster = PhotonNetwork.Instantiate(monsters[index].monster.name, spawnPoint, transform.rotation, 0);
            } else
            {
                monster = Instantiate(monsters[index].monster, spawnPoint, transform.rotation);
            }

            monster.transform.SetParent(monsterContainer.transform);
        }
    }
}
