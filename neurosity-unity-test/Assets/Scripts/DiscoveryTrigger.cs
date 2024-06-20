using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoveryTrigger : MonoBehaviour
{
    public GuidingLight guidingLight; // Reference to the GuidingLight script

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            guidingLight.PlayerReachedWaypoint(transform);
        }
    }
}
