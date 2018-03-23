using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public static class Wander {

    // Wander Randomly
    public static void Randomly(IAstarAI ai, Seeker seeker, float radius = 10)
    {
        SetPath(PickRandomPointInCircle(radius, ai.position), ai, seeker);
    }

    // Wander Ahead
    public static void Ahead(IAstarAI ai, Seeker seeker, float angle = 30, float distance = 10, float radius = 5)
    {
        SetPath(PickRandomPointInArc(angle, distance, radius, seeker), ai, seeker);
    }

    public static void OverrideRandomly(IAstarAI ai, Seeker seeker, float radius = 10)
    {
        OverrideSetPath(PickRandomPointInCircle(radius, ai.position), ai, seeker);
    }

    public static void OverrideAhead(IAstarAI ai, Seeker seeker, float angle = 30, float distance = 10, float radius = 5)
    {
        OverrideSetPath(PickRandomPointInArc(angle, distance, radius, seeker), ai, seeker);
    }

    // Set the path to wander.
    private static void SetPath(Vector3 destination, IAstarAI ai, Seeker seeker)
    {
        // If the AI currently cannot search, then allow it to search so it can wander.
        if (!ai.canSearch) { ai.canSearch = true; }

        // Find a path if a path is not being calculated and the path is finished or doesn't exist.
        if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath))
        {
            seeker.CancelCurrentPathRequest();
            ai.destination = destination;
            ai.SearchPath();
        }
    }

    private static void OverrideSetPath(Vector3 destination, IAstarAI ai, Seeker seeker)
    {
        // If the AI currently cannot search, then allow it to search so it can wander.
        if (!ai.canSearch) { ai.canSearch = true; }

        // Find a path if a path is not being calculated.
        if (!ai.pathPending)
        {
            seeker.CancelCurrentPathRequest();
            ai.destination = destination;
            ai.SearchPath();
        }
    }

    // Pick a random point within a circle centered at the offset.
    private static Vector3 PickRandomPointInCircle(float radius, Vector3 offset)
    {
        var point = Random.insideUnitSphere * radius;

        point.y = 0;
        point += offset;
        return point;
    }

    // Pick a random point within an arc centered at the offset.
    private static Vector3 PickRandomPointInArc(float angle, float distance, float radius, Seeker seeker)
    {
        Vector3 targetDirection = Quaternion.AngleAxis(Random.Range(-angle / 2, angle / 2), Vector3.up) * seeker.transform.forward;
        Vector3 targetPosition = targetDirection * distance + seeker.transform.position;
        return PickRandomPointInCircle(radius, targetPosition);
    }
}
