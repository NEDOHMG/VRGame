﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowVRCamera : MonoBehaviour
{

    public static FollowVRCamera sharedInstance;

    public Transform target;

    public float xOffset = -3.2f, yOffset = -3.2f, zOffset = 10.0f;

    Vector3 totalOffSet;

    [HideInInspector]
    public bool inclination = false;

    Vector3 modifyPosition;

    void Awake()
    {
        sharedInstance = this;
    }

    void Start()
    {
        totalOffSet = new Vector3(xOffset, yOffset, zOffset);
        modifyPosition = totalOffSet;
    }

    void LateUpdate()
    {
        transform.position = target.transform.position + modifyPosition;

        if (inclination && modifyPosition.y <= 3.6)
        {
            modifyPosition.y += 0.005f;
            // Debug.Log("x: " + transform.position.x + " y: " + transform.position.y + " z: " + transform.position.z);
        }

    }

    public void ResetPosition()
    {
        modifyPosition = totalOffSet;
    }

}

