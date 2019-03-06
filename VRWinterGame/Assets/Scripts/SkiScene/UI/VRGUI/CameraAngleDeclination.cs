using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAngleDeclination : MonoBehaviour
{

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // Debug.Log("Enter");
            FollowVRCamera.sharedInstance.inclination = true;
        }
    }
}
