using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPartsSpawnManager : MonoBehaviour {

    public List<GameObject> parts;

    List<Transform> spawnPoints;

	// Use this for initialization
	void Start () {
        spawnPoints = new List<Transform>();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("ShipPartsSpawner"))
            {
                spawnPoints.Add(child);
            }
        }

        if (spawnPoints.Count > 0)
        {
            SpawnShipParts();
        }
	}

    void SpawnShipParts()
    {
        for (int i = 0; i < parts.Count; i++)
        {
            int random = Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[random];

            GameObject part = null;
            if (PhotonNetwork.connected)
            {
                PhotonNetwork.Instantiate(parts[i].name, spawnPoint.position, spawnPoint.rotation, 0);
            }
            else
            {
                Instantiate(parts[i], spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
}
