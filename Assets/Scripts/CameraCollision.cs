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

    Vector3 offset;

    void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        dollyDist = transform.localPosition.magnitude;
        distance = transform.localPosition.z;
        maxDistance = -distance;
        minDistance = 0f;
        offset = new Vector3(0, 1, 0);
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        desiredCameraPos = transform.parent.TransformPoint(dollyDir * dollyDist);
        if (Physics.Linecast(transform.parent.parent.position + offset, desiredCameraPos, out hit, layersToCheck))
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
