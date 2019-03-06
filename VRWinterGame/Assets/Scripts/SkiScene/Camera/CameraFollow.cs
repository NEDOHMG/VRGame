using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float xOffset = 0.4f, yOffset = 0.35f, zOffset = 0.0f;

    Vector3 totalOffSet;

    void Start()
    {
        totalOffSet = new Vector3(xOffset, yOffset, zOffset);
    }

    void LateUpdate()
    {
        transform.position = target.transform.position + totalOffSet;
    }
}
