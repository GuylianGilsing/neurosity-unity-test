using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidingLight : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints
    public float speed = 5f; // Speed of the light
    private int currentWaypointIndex = 0; // Index of the current waypoint
    private bool shouldMoveToNextWaypoint = false; // Flag to control movement

    void Start()
    {
        // Start at the first waypoint
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
        }
    }

    void Update()
    {
        if (waypoints.Length == 0)
            return;

        if (shouldMoveToNextWaypoint)
        {
            MoveTowardsWaypoint();
        }
    }

    void MoveTowardsWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            // Reached the waypoint, stop moving and wait for player to trigger next move
            transform.position = targetWaypoint.position; // Ensure exact positioning
            shouldMoveToNextWaypoint = false; // Wait for player to reach the next waypoint

            // Destroy the guiding light if it reaches the third waypoint (index 2)
            if (currentWaypointIndex == 2)
            {
                Destroy(gameObject);
                return; // Exit the function to avoid further processing
            }
        }
        else
        {
            // Move towards the waypoint
            transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        }
    }

    public void PlayerReachedWaypoint(Transform waypoint)
    {
        // Check if the player reached the current waypoint
        if (waypoints[currentWaypointIndex] == waypoint)
        {
            // Prepare to move to the next waypoint
            shouldMoveToNextWaypoint = true;
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0; // Loop or stop if needed
            }
        }
    }
}
