using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPoint : MonoBehaviour
{

    Rigidbody rb;

    [HideInInspector]
    public float rotationSpeedAceleration = 1.3f;
    // public float speedAceleration = 1.3f;

    public static RotationPoint sharedInstance;

    bool rotateTrigger = false;
    bool rotSpeedBoost = false;

    void Awake()
    {
        sharedInstance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.CompareTag("RotatePointOne"))
        {
            // Debug.Log("Rotate One");
            rotateTrigger = true;
        }
        else if (collider.gameObject.CompareTag("RotatePointTwo"))
        {
            //Debug.Log("Rotate Two");
            rotateTrigger = true;
        }
        else if (collider.gameObject.CompareTag("RotatePointThree"))
        {
            // Debug.Log("Rotate Three");
            rotateTrigger = true;
        }
        else if (collider.gameObject.CompareTag("RotatePointFour"))
        {
            //Debug.Log("Rotate Four");
            rotateTrigger = true;
            rotSpeedBoost = true;
            rotationSpeedAceleration = rotationSpeedAceleration + 0.02f;
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("RotatePointOne"))
        {
            // Debug.Log("Rotate One Exit");
            rotateTrigger = false;

        }
        else if (collider.gameObject.CompareTag("RotatePointTwo"))
        {
            // Debug.Log("Rotate Two Exit");
            rotateTrigger = false;
        }
        else if (collider.gameObject.CompareTag("RotatePointThree"))
        {
            // Debug.Log("Rotate Three Exit");
            rotateTrigger = false;
        }
        else if (collider.gameObject.CompareTag("RotatePointFour"))
        {
            // Debug.Log("Rotate Four Exit");
            rotateTrigger = false;
            rotSpeedBoost = false;
        }
    }

    void FixedUpdate()
    {
        if (rotateTrigger)
        {
            rb.AddForce(PlayerSkiController.sharedInstance.rotationXPoint, 0, PlayerSkiController.sharedInstance.rotationZPoint, ForceMode.Impulse);
        }
        else if (rotateTrigger == true && rotSpeedBoost == true)
        {
            rb.AddForce(PlayerSkiController.sharedInstance.rotationXPoint * rotationSpeedAceleration, 0, PlayerSkiController.sharedInstance.rotationZPoint * rotationSpeedAceleration * Time.deltaTime, ForceMode.Impulse);
        }
    }
}
