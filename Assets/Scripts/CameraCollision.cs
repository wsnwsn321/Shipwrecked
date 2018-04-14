using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour {

    public LayerMask layersToCheck;

    float minDistance;
    float maxDistance;

    Vector3 originalPosition;

    Vector3 dollyDir;
    float dollyDist;
    float distance;

    Vector3 desiredCameraPos;

    void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        dollyDist = transform.localPosition.magnitude;
        distance = transform.localPosition.z;
        maxDistance = -distance;
        minDistance = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        desiredCameraPos = transform.parent.parent.TransformPoint(dollyDir * dollyDist);
        if (Physics.Linecast(transform.parent.parent.position, desiredCameraPos, out hit, layersToCheck))
        {
            distance = Mathf.Clamp(hit.distance, minDistance, maxDistance) - 1;
        }
        else
        {
            distance = maxDistance;
        }

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -distance);
    }
}
