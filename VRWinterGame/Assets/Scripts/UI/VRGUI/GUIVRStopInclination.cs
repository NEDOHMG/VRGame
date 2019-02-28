using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIVRStopInclination : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enter");
            FollowVRCamera.sharedInstance.inclination = false;
        }
    }
}
