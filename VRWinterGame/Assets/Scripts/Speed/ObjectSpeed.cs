using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpeed : MonoBehaviour
{
    const float x = -1.3f, y = -1.7f, z = 2.4f;

    public Rigidbody rb;

    TriggerSpeed triggerSpeed;
    PlayerJump playerJump;

    GameObject ramp;
    RampGenerator rampGenerator;

    //public float speedAdjustmentAceleration = 0.4f;

    Vector3 speed;

    public static ObjectSpeed sharedInstance;

    [HideInInspector]
    Vector3 originalPosition;


    void Awake()
    {
        sharedInstance = this;
    }

    // Use this for initialization
    void Start()
    {
        triggerSpeed = GetComponent<TriggerSpeed>();
        playerJump = GetComponent<PlayerJump>();

        ramp = GameObject.Find("RampSpotGenerator");
        rampGenerator = ramp.GetComponent<RampGenerator>();

        speed = new Vector3(x, y, z);

        originalPosition = transform.position;
    }

    public void ResetPositionSpeedUser()
    {
        transform.position = originalPosition;
        speed = new Vector3(x, y, z);
    }

    public void InTheGame()
    {
        if (triggerSpeed.stop)
        {
            // Debug.Log("The game is stoped");
            rb.isKinematic = true; // Stop all the movement of the user
        }

        if (triggerSpeed.pointOneTriggered == false && triggerSpeed.pointTwoTriggered == false && triggerSpeed.pointThreeTriggered == false && triggerSpeed.goal == false)
        {
            rb.velocity = speed * PlayerSkiController.sharedInstance.speedAdjustmentAceleration;
        }
        else if (triggerSpeed.pointOneTriggered == true && triggerSpeed.pointTwoTriggered == false && triggerSpeed.pointThreeTriggered == false && triggerSpeed.goal == false)
        {
            rb.velocity = speed * (PlayerSkiController.sharedInstance.speedAdjustmentAceleration + 0.1f);
        }
        else if (triggerSpeed.pointOneTriggered == true && triggerSpeed.pointTwoTriggered == true && triggerSpeed.pointThreeTriggered == false && triggerSpeed.goal == false)
        {
            if (rampGenerator.levelOne)
            {
                PlayerSkiController.sharedInstance.speedAdjustmentAceleration = 0.5f;
                // rb.AddForce(rotationXPoint * speedAceleration, 0, rotationZPoint * speedAceleration * Time.deltaTime, ForceMode.Impulse);
                rb.AddForce(0.05f * 1.3f, 0, 0.1f * 1.3f, ForceMode.Impulse);
                rb.velocity = speed * (PlayerSkiController.sharedInstance.speedAdjustmentAceleration + 0.12f);
            }
            rb.velocity = speed * (PlayerSkiController.sharedInstance.speedAdjustmentAceleration + 0.05f);
        }
        else if (triggerSpeed.pointOneTriggered == true && triggerSpeed.pointTwoTriggered == true && triggerSpeed.pointThreeTriggered == true && triggerSpeed.goal == false && triggerSpeed.speedAcelerator == true && playerJump.jump == true)
        {

            rb.velocity = speed * (PlayerSkiController.sharedInstance.speedAdjustmentAceleration + 0.12f);

        }
        else if (triggerSpeed.pointOneTriggered == true && triggerSpeed.pointTwoTriggered == true && triggerSpeed.pointThreeTriggered == true && triggerSpeed.goal == false)
        {
            rb.velocity = speed * PlayerSkiController.sharedInstance.speedAdjustmentAceleration;
        }
        else if (triggerSpeed.pointOneTriggered == true && triggerSpeed.pointTwoTriggered == true && triggerSpeed.pointThreeTriggered == true && triggerSpeed.goal == true)
        {
            // It will start to desacelerate
        }
    }

    void FixedUpdate()
    {
        if (PlayerSkiController.sharedInstance.startGame || PlayerSkiController.sharedInstance.resetPositionUser == true)
        {
            // agregar aqui el reset del reloj
            rb.isKinematic = false; // Check if we are in the game
        }
        // We are in the game
        InTheGame();
    }

}
