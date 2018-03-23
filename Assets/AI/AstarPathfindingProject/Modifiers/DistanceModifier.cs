using UnityEngine;
using System.Collections.Generic;

namespace Pathfinding
{
    [AddComponentMenu("Pathfinding/Modifiers/Distance")]
    public class DistanceModifier : MonoModifier
    {
        // Determines the radius of the sphere to check for colliders.
        public float radiusToCheck = 1;

        // Determines the distance to shift the point from the direction of the nearest collider.
        public float desiredDistance = 1;

        // Determines the layer masks to check (Make this the layers you want to avoid).
        public LayerMask mask;

        // The order to run for the modifiers (the lower, the earlier it's ran).
        // Initially set to 0 since this is rather helpful in finding a rudimentary path away from objects,
        // which lends itself nicely to being modified by the other modifiers.
        public override int Order { get { return 0; } }

        public override void Apply(Path path)
        {
            // Make sure there are no path errors.
            if (path.error || path.vectorPath == null || path.vectorPath.Count == 0)
            {
                return;
            }

            // Get an empty list from the pool
            List<Vector3> newPath = Util.ListPool<Vector3>.Claim();
            List<Vector3> originalPath = path.vectorPath;

            // Initialize the previous point to something that can never be reached.
            Vector3 previousPoint = new Vector3(0, 0, float.MaxValue);

            // Go over every point on the path.
            for (int i = 0; i < originalPath.Count; i++)
            {
                // Find all nearby colliders to avoid.
                Collider[] colliders = Physics.OverlapSphere(originalPath[i], radiusToCheck, mask);
                float minDistance = float.MaxValue;
                Collider nearestCollider = null;

                // Iterate through each collider to find the nearest one.
                for (int j = 0; j < colliders.Length; j++)
                {
                    float colliderDistance = Vector3.Distance(originalPath[i], colliders[j].bounds.ClosestPoint(originalPath[i]));
                    if (colliderDistance < minDistance)
                    {
                        minDistance = colliderDistance;
                        nearestCollider = colliders[j];
                    }
                }

                // If there was a collider within the desired radius, modify the path to avoid it.
                if (nearestCollider)
                {
                    // Get the direction of the nearest collider from the current point.
                    Vector3 direction = (originalPath[i] - nearestCollider.bounds.ClosestPoint(originalPath[i])).normalized;

                    // If there was a previous point, then find the line between it and the new point.
                    if (previousPoint.z != float.MaxValue)
                    {
                        newPath.Add(Vector3.Lerp(previousPoint, originalPath[i] + direction * desiredDistance, 1));
                    }
                    else
                    {
                        // Otherwise, set the initial point to the new point.
                        newPath.Add(originalPath[i] + direction * desiredDistance);
                    }

                    // Keep track of the new point for the next iteration.
                    previousPoint = originalPath[i] + direction * desiredDistance;
                }
                else
                {
                    // If there was a previous point, then find the line between them.
                    if (previousPoint.z != float.MaxValue)
                    {
                        newPath.Add(Vector3.Lerp(previousPoint, originalPath[i], 1));
                    }
                    else
                    {
                        // Otherwise keep the same initial point.
                        newPath.Add(originalPath[i]);
                    }

                    // Keep track of the original point for the next iteration.
                    previousPoint = originalPath[i];
                }
            }

            // Assign the new path to the p.vectorPath field.
            path.vectorPath = newPath;

            // Pool the previous path. It isn't needed anymore.
            Util.ListPool<Vector3>.Release(originalPath);
        }
    }
}
