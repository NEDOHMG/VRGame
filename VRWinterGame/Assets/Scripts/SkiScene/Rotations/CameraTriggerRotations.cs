using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTriggerRotations : MonoBehaviour
{
    [HideInInspector]
    public bool cameraRotationX = false, cameraRotationY = false, cameraRotationZ = false, cameraFieldView = false;

    public static CameraTriggerRotations sharedInstance;

    void Awake()
    {
        sharedInstance = this;
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("RotatePointOne"))
        {
            cameraRotationX = true;
            cameraRotationZ = true;

        }
        else if (collider.gameObject.CompareTag("RotatePointFour"))
        {
            cameraRotationY = true;
            cameraFieldView = true;
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("RotatePointTwo"))
        {
            cameraRotationZ = false;
        }

        if (collider.gameObject.CompareTag("RotatePointFour"))
        {
            cameraRotationY = false;
        }
    }

    public void ResetCameraTriggerRotations()
    {
        cameraRotationX = false;
        cameraRotationY = false;
        cameraRotationZ = false;
        cameraFieldView = false;
    }
}
