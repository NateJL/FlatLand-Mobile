using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool showGizmos;
    public List<Transform> waypoints;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            waypoints.Add(transform.GetChild(i));
        }
    }

    /// <summary>
    /// Gets the next waypoint in line given the current one.
    /// </summary>
    public Transform GetNextWaypoint(Transform currentTarget)
    {
        if (currentTarget == null)
            return waypoints[0];

        for(int i = 0; i < waypoints.Count-1; i++)
        {
            if(currentTarget.Equals(waypoints[i]))
            {
                return waypoints[i + 1];
            }
        }
        if (currentTarget.Equals(waypoints[waypoints.Count - 1]))
        {
            return waypoints[0];
        }
        else                        // should probably fix this
            return waypoints[0];
    }

    private void OnDrawGizmos()
    {
        if(showGizmos)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(waypoints[0].position + (Vector3.up * 0.3f), 0.2f);
            for (int i = 1; i < waypoints.Count; i++)
            {
                Gizmos.DrawSphere(waypoints[i].position + (Vector3.up * 0.3f), 0.2f);
                Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
            }
            Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
        }
    }
}
