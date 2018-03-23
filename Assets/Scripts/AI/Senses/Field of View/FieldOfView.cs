using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

    // PUBLIC INSPECTOR VARIABLES
    [Header("Sight Settings")]
    public Vector3 offset = Vector3.zero;
    [Range(0, 360)]
    public float angleOffset = 0;
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [Header("Display Settings")]
    public bool displayFOV = false;
    public Material baseMaterial;
    public Color fovColor = new Color(1, 1, 1, 0.5f);
    public Color targetFoundColor = new Color(1, 0, 0, 0.5f);

    [Header("Display Resolution")]
    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDistanceThreshold;

    // ACCESSIBLE VARIABLES
    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();
    [HideInInspector]
    public Transform nearestTarget;

    // NON-PUBLIC VARIABLES
    MeshFilter viewMeshFilter;
    Mesh viewMesh;
    GameObject viewVisualization;
    bool detectsTarget = false;
    Vector3 viewPosition;

    // DEFINE STRUCTS
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

    // START OF FUNCTIONS
    void Start()
    {
        ViewInitialization();
        viewMeshFilter = viewVisualization.GetComponent<MeshFilter>();
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    void ViewInitialization()
    {
        // Clone a material to get the base values desired and then modify it accordingly.
        Material viewMaterial = Instantiate(baseMaterial);
        viewMaterial.name = "FOV Material";
        viewMaterial.SetColor("_Color", fovColor);

        // Initialize the visualization object for the FOV.
        viewVisualization = new GameObject("View Visualization");
        viewVisualization.layer = LayerMask.NameToLayer("View");
        MeshFilter filter = viewVisualization.AddComponent<MeshFilter>();

        // Set the renderer and its values.
        MeshRenderer renderer = viewVisualization.AddComponent<MeshRenderer>();
        renderer.receiveShadows = false;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.material = viewMaterial;

        viewVisualization.transform.position = offset;

        // Set the parent of the view to the object with field of view.
        viewVisualization.transform.SetParent(transform, false);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        nearestTarget = null;
        float nearestTargetDistance = 0;
        Vector3 forwardWithOffset = Quaternion.Euler(0, angleOffset, 0) * transform.forward;

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position + offset, viewRadius, targetMask);

        foreach (Collider target in targetsInViewRadius)
        {
            bool isWithinExtents = false;
            Vector3 extents = target.transform.GetComponent<Collider>().bounds.extents;
            for (int i = 0; i < 6; i++)
            {
                Vector3 outerbound = Vector3.zero;
                switch (i % 3) {
                    case 0:
                        outerbound = new Vector3(extents.x, 0, 0);
                        break;
                    case 1:
                        outerbound = new Vector3(0, extents.y, 0);
                        break;
                    case 2:
                        outerbound = new Vector3(0, 0, extents.z);
                        break;
                    default:
                        Debug.LogError("Something went terribly wrong in the FieldOfView Script.");
                        break;
                }

                if (i > 2)
                {
                    outerbound = -outerbound;
                }

                Vector3 directionToTargetFromExtent = (target.transform.position + outerbound - transform.position).normalized;
                bool isWithinExtent = Vector3.Angle(forwardWithOffset, directionToTargetFromExtent) < viewAngle / 2;
                isWithinExtents = isWithinExtents || isWithinExtent;

                if (isWithinExtents)
                {
                    break;
                }
            }
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            if (isWithinExtents)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (!Physics.Raycast(transform.position + offset, directionToTarget, distanceToTarget, obstacleMask))
                {
                    visibleTargets.Add(target.transform);

                    if (!nearestTarget || distanceToTarget < nearestTargetDistance)
                    {
                        nearestTarget = target.transform;
                        nearestTargetDistance = distanceToTarget;
                    }
                }
            }
        }
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    void LateUpdate()
    {
        if (displayFOV)
        {
            DrawFieldOfView();
        }
        else
        {
            if (viewMesh.vertices.Length > 0)
            {
                viewMesh.Clear();
            }
        }

        if (detectsTarget)
        {
            if (visibleTargets.Count == 0)
            {
                viewVisualization.GetComponent<MeshRenderer>().material.color = fovColor;
                detectsTarget = false;
            }
        }
        else
        {
            if (visibleTargets.Count > 0)
            {
                viewVisualization.GetComponent<MeshRenderer>().material.color = targetFoundColor;
                detectsTarget = true;
            }
        }
    }

    void DrawFieldOfView()
    {
        // Number of Rays per degree.
        int rayCount = Mathf.RoundToInt(viewAngle * meshResolution);

        // The size of the angle from one ray to the next.
        float rayAngleSize = viewAngle / rayCount;

        List<Vector3> viewPoints = new List<Vector3>();

        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + angleOffset + rayAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }

                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];

        // A triangle has 3 points and the center point and previous point are repeated, so -2.
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 direction = DirectionFromAngle(globalAngle, true);
        RaycastHit hit;

        // If the raycast hits a point, then readjust its viewing distance. Otherwise display the full length.
        if (Physics.Raycast(transform.position + offset, direction, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point - offset, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + direction * viewRadius, viewRadius, globalAngle);
        }
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDistanceThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }
}
