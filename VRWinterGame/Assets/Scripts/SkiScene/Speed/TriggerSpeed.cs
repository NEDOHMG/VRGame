using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpeed : MonoBehaviour
{

    public static TriggerSpeed sharedInstance;

    [HideInInspector]
    public bool pointOneTriggered = false, pointTwoTriggered = false, pointThreeTriggered = false, goal = false, stop = false, speedAcelerator = false;

    void Awake()
    {
        sharedInstance = this;
    }

    public void OnTriggerEnter(Collider collider)
    {

        // This is the normal state of the game before enter in any trigger
        if (collider.gameObject.CompareTag("PointOne") && pointOneTriggered == false && pointTwoTriggered == false && pointThreeTriggered == false && goal == false)
        {
            //Debug.Log("beginning");
            pointOneTriggered = true;
        }
        else if (collider.gameObject.CompareTag("PointTwo") && pointOneTriggered == true && pointTwoTriggered == false && pointThreeTriggered == false && goal == false)
        {
            //Debug.Log("two");
            pointTwoTriggered = true;
        }
        else if (collider.gameObject.CompareTag("PointThree") && pointOneTriggered == true && pointTwoTriggered == true && pointThreeTriggered == false && goal == false)
        {
            //Debug.Log("three");
            pointThreeTriggered = true;
        }
        else if (collider.gameObject.CompareTag("Goal") && pointOneTriggered == true && pointTwoTriggered == true && pointThreeTriggered == true && goal == false)
        {
            //Debug.Log("goal");
            speedAcelerator = false;
            goal = true;
        }
    }

    public void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.CompareTag("RotatePointFour"))
        {
            speedAcelerator = true;
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Goal"))
        {
            // Debug.Log("Stop");
            stop = true; // Stop all the movement of the user
            GameManager.sharedInstance.GameOver();
        }
    }

    public void ResetTriggerSpeedVariables()
    {
        pointOneTriggered = false;
        pointTwoTriggered = false;
        pointThreeTriggered = false;
        goal = false;
        // stop = false; <-- dont use this
        speedAcelerator = false;
    }

}
