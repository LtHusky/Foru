using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCamera : MonoBehaviour
{
    public Transform transformToFollow;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transformToFollow.transform.position.y, transform.position.z);
    }
}
