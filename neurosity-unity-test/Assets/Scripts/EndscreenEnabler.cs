using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndscreenEnabler : MonoBehaviour
{
    [SerializeField]
    private GameObject endscreen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            endscreen.SetActive(true);
        }
    }
}
