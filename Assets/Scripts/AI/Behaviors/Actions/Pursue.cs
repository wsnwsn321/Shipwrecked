using UnityEngine;
using Pathfinding;

public class Pursue {

    // Set the path to pursue a target.
    public static void Target(Vector3 targetPosition, IAstarAI ai, Seeker seeker, Vector3 previousDestination, bool isPursuing)
    {
        // Don't find a path if a path is currently being calculated.
        if (!ai.pathPending)
        {
            // Only find a path if the AI is currently not pursuing or if it is pursuing and hasn't reached its target. 
            if (!isPursuing || (isPursuing && !ai.reachedEndOfPath))
            {
                ai.destination = targetPosition;

                // Only update the path if the destination changes.
                if (!(ai.destination == previousDestination))
                {
                    ai.SearchPath();
                }
            }
            else
            {
                // Else the AI is pursuing and has reached its target.

                // Stop searching if the target isn't moving, otherwise continue pursuit.
                if (targetPosition == previousDestination)
                {
                    ai.canSearch = false;
                }
                else
                {
                    ai.canSearch = true;
                    ai.destination = targetPosition;
                }
            }
        }
    }

    // Set the path to pursue a target.
    public static void OverrideTarget(Vector3 targetPosition, IAstarAI ai, Seeker seeker, Vector3 previousDestination, bool isPursuing)
    {
        ai.canSearch = true;
        ai.destination = targetPosition;

        // Only update the path if the destination changes.
        if (!(ai.destination == previousDestination))
        {
            ai.SearchPath();
        }
    }

    // Sets the path to pursue the nearest target.
    public static void Nearest(FieldOfView fov, IAstarAI ai, Seeker seeker, Vector3 previousDestination, bool isPursuing, bool overridePath = false)
    {
        if (overridePath)
        {
            OverrideTarget(fov.nearestTarget.position, ai, seeker, previousDestination, isPursuing);
        } else
        {
            Target(fov.nearestTarget.position, ai, seeker, previousDestination, isPursuing);
        }
    }
}
