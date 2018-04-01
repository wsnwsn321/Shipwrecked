using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncTurret : MonoBehaviour {

    private bool isFullyActive = false;

    // This method is responsible for synchronizing the health of the enemy
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            if (!isFullyActive)
            {
                
                List<Transform> descendants = GetDescendants(transform);
                for (int i = 0; i < descendants.Count; i++)
                {
                    stream.SendNext(descendants[i].gameObject.activeSelf);
                }
                stream.SendNext(gameObject.GetComponent<TurretBehaviors>().enabled);
            }
        }
        else
        {
            if (!isFullyActive)
            {
                bool isActive = true;
                List<Transform> descendants = GetDescendants(transform);
                for (int i = 0; i < descendants.Count; i++)
                {
                    descendants[i].gameObject.SetActive((bool)stream.ReceiveNext());
                    print(descendants[i].name);
                    print(descendants[i].gameObject.activeSelf);
                    isActive = isActive && descendants[i].gameObject.activeSelf;
                }
                gameObject.GetComponent<TurretBehaviors>().enabled = (bool)stream.ReceiveNext();
                isFullyActive = isActive;
            }
        }
    }

    List<Transform> GetDescendants(Transform t)
    {
        List<Transform> descendants = new List<Transform>();
        foreach (Transform child in t)
        {
            descendants.Add(child);
            descendants.AddRange(GetDescendants(child));
        }
        return descendants;
    }
}
