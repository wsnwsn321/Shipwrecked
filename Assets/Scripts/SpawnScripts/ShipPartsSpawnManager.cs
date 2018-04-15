using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShipPartsSpawnManager : MonoBehaviour
{
    public List<GameObject> parts;
    List<Transform> spawnPoints;
    List<Transform> sortedSpawnPoints;

    int spawnPointCount;

    // Use this for initialization
    void Start()
    {
        spawnPoints = new List<Transform>();
        //parts = new List<GameObject>();

        //foreach (Transform child in ShipParts.transform)
       // {
         //   parts.Add(child.gameObject);
        //}

        foreach (Transform child in transform)
        {
            if (child.CompareTag("ShipPartsSpawner"))
            {
                spawnPoints.Add(child);
            }
        }

        sortedSpawnPoints = new List<Transform>(spawnPoints);
        spawnPointCount = spawnPoints.Count;

        if (spawnPoints.Count >= parts.Count)
        {
            SpawnShipParts();
        }
    }

    void SpawnShipParts()
    {
        GameObject partsContainer = new GameObject();
        partsContainer.name = "ShipParts";

        for (int i = 0; i < parts.Count; i++)
        {
            int random = Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[random];

            GameObject part = null;

            if (PhotonNetwork.connected)
            {
                // Only have the master client spawn the parts.
                if (PhotonNetwork.isMasterClient)
                {
                    part = PhotonNetwork.Instantiate(parts[i].name, spawnPoint.position, spawnPoint.rotation, 0);
                }
            }
            else
            {
                // Single player spawning.
                part = Instantiate(parts[i], spawnPoint.position, spawnPoint.rotation);
            }

            // Protects from a non-master client spawning the parts.
            if (part != null)
            {
                RemoveNearestSpawnPoints(spawnPoint);
                part.transform.SetParent(partsContainer.transform);
            }
        }
    }

    void RemoveNearestSpawnPoints(Transform centerPoint)
    {
        int numberToRemove = spawnPointCount / parts.Count;
        sortedSpawnPoints = sortedSpawnPoints.OrderBy(spawnPoint => Vector3.SqrMagnitude(spawnPoint.position - centerPoint.position)).ToList();

        for (int i = 0; i < numberToRemove; i++)
        {
            spawnPoints.Remove(sortedSpawnPoints[0]);
            sortedSpawnPoints.RemoveAt(0);
        }
    }
}
