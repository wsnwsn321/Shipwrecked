using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    float swing_angle;
    float elevate_angle;
    public float speed;
    public float camSlowDecay;
    public GameObject cam;

    [Range(0, 1)]
    public float snapbackTime;

    private float nearZero = 1.0f;
    private Quaternion startingRotation;
    private float snapbackRate;
    private float rotationPercentage;

    private Vector3 adjustedDestination = Vector3.zero;
    private Vector3 destination = Vector3.zero;
    private Vector3 playerPosition = Vector3.zero;
    private Vector3 camVelocity = Vector3.zero;

    public CollisionHandler collision = new CollisionHandler();
    public PositionSettings position = new PositionSettings();

    void Start()
    {
        startingRotation = cam.transform.rotation;
        snapbackRate = 1.0f / snapbackTime;

        collision.Initialize(cam.GetComponent<Camera>());
        collision.UpdateCameraClipPoints(cam.transform.position, cam.transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, cam.transform.rotation, ref collision.desiredCameraClipPoints);

    }

    void FixedUpdate() {
        MoveCamera();
        RotateCamera();

        collision.UpdateCameraClipPoints(cam.transform.position, cam.transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, cam.transform.rotation, ref collision.desiredCameraClipPoints);

        for (int i = 0; i < 5; i++)
        {
            
        }

        collision.CheckColliding(transform.position);
        position.adjustmentDistance = collision.GetAdjustedDistanceWithRayFrom(transform.position);
        
    }

    void MoveCamera()
    {
        playerPosition = transform.position + position.playerOffset;
        destination = -Vector3.forward * position.distanceFromTarget;
        destination += transform.position;

        if (collision.colliding)
        {
            adjustedDestination = -Vector3.forward * position.adjustmentDistance;
            adjustedDestination += transform.position;

            if (position.smoothFollow)
            {
                cam.transform.position = Vector3.SmoothDamp(cam.transform.position, adjustedDestination, ref camVelocity, position.smooth);
            } else
            {
                cam.transform.position = adjustedDestination;
            }
        } else
        {
            if (position.smoothFollow)
            {
                cam.transform.position = Vector3.SmoothDamp(cam.transform.position, destination, ref camVelocity, position.smooth);
            }
            else
            {
                cam.transform.position = destination;
            }
        }
    }

    void RotateCamera()
    {
        if (Input.GetMouseButton(0))
        {
            swing_angle = Input.GetAxis("Mouse X") * speed;
            elevate_angle = Input.GetAxis("Mouse Y") * speed;
        }
        else
        {
            // Ease the camera's rotation to zero.
            if (swing_angle != 0) swing_angle = swing_angle * camSlowDecay;
            if (elevate_angle != 0) elevate_angle = elevate_angle * camSlowDecay;
            if (swing_angle < nearZero && swing_angle > -nearZero) swing_angle = 0;
            if (elevate_angle < nearZero && elevate_angle > -nearZero) elevate_angle = 0;

            // When the camera stops moving, move back to the original position within the given amount of time (snapback time).
            if (swing_angle == 0 && elevate_angle == 0)
            {
                // Only snapback if the camera isn't rotated the same as it originally was.
                if (!cam.transform.rotation.Equals(startingRotation))
                {
                    if (rotationPercentage < 1.0f)
                    {
                        rotationPercentage = Time.deltaTime * snapbackRate;
                        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, startingRotation, Mathf.SmoothStep(0.0f, 1.0f, rotationPercentage));
                    }
                }
            }
            else
            {
                // Reset the rotation percentage if the camera was snapping back and
                // the player rotated the screen in the middle of the snapback.
                if (rotationPercentage > 0)
                {
                    rotationPercentage = 0;
                }
            }
        }
        cam.transform.Rotate(-elevate_angle, 0, 0);
        cam.transform.Rotate(0, swing_angle, 0);
    }

    // Code taken from the Camera Collision Handler video from Renaissance Coders.
    // The code taken is both the PositionSettings and the CollisionHandler classes.
    [System.Serializable]
    public class PositionSettings
    {
        public Vector3 playerOffset = new Vector3(0, 0, 0);
        public float lookSmooth = 100f;
        public float distanceFromTarget = 8;

        // Zoom boundaries.
        public float maxZoom = -2;
        public float minZoom = -15;

        // Smoothing
        public float zoomSmooth = 10;
        public float zoomStep = 2;
        public bool smoothFollow = true;
        public float smooth = 0.05f;

        [HideInInspector]
        public float adjustmentDistance = 8;
    }

    [System.Serializable]
    public class CollisionHandler
    {
        public LayerMask collisionLayer;

        [HideInInspector]
        public bool colliding = false;
        [HideInInspector]
        public Vector3[] adjustedCameraClipPoints;
        [HideInInspector]
        public Vector3[] desiredCameraClipPoints;
        public float cameraCushion = 3.41f;

        Camera camera;

        public void Initialize(Camera cam)
        {
            camera = cam;
            adjustedCameraClipPoints = new Vector3[5];
            desiredCameraClipPoints = new Vector3[5];
        }

        public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray)
        {
            if (!camera) return;

            // Clear the contents of the intoArray.
            intoArray = new Vector3[5];

            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView / cameraCushion) * z;
            float y = x / camera.aspect;

            // Get the 5 clip points. These are part of the camera's 
            // Top Left
            intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPosition;

            // Top Right
            intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPosition;

            // Bottom Left
            intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition;

            // Bottom Right
            intoArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPosition;

            // Camera Position
            // Note: Subtracting camera.transform.forward gives a bit more room for the camera to collide with.
            intoArray[4] = cameraPosition - camera.transform.forward;
        }

        bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
        {
            for (int i = 0; i < clipPoints.Length; i++)
            {
                Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
                float distance = Vector3.Distance(clipPoints[i], fromPosition);
                if (Physics.Raycast(ray, distance, collisionLayer))
                {
                    return true;
                }
            }

            return false;
        }

        public float GetAdjustedDistanceWithRayFrom(Vector3 from)
        {
            float distance = -1;

            for (int i = 0; i < desiredCameraClipPoints.Length; i++)
            {
                Ray ray = new Ray(from, desiredCameraClipPoints[i] - from);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (distance == -1)
                        distance = hit.distance;
                    else
                    {
                        if (hit.distance < distance)
                            distance = hit.distance;
                    }
                }
            }

            if (distance == -1)
                return 0;
            else
                return distance;
        }

        public void CheckColliding(Vector3 targetPosition)
        {
            colliding = CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition);
        }
    }
}