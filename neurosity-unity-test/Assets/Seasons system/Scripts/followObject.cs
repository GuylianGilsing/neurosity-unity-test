using UnityEngine;

public class followObject : MonoBehaviour
{
    public GameObject objectToFollow;

    void Update()
    {
        transform.position = objectToFollow.transform.position;
    }
}
