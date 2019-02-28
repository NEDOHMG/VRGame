using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLevel : MonoBehaviour
{

    public static ResetLevel sharedInstance;

    void Awake()
    {
        sharedInstance = this;
    }

    public void ResetThePlayersMethods()
    {
        // Ramp generator
        RampGenerator.sharedInstance.levelOne = false;
        // Player jump
        PlayerJump.sharedInstance.ResetJumpVariables();
        // Rotation point
        RotationPoint.sharedInstance.rotationSpeedAceleration = 1.3f;
        // Camera triggers
        CameraTriggerRotations.sharedInstance.ResetCameraTriggerRotations();
        // Trigger speed
        TriggerSpeed.sharedInstance.ResetTriggerSpeedVariables();
        // Reset to the GUI indicator
        ViewInGame.sharedInstance.ResetStateSignal();

        StartCoroutine(ExecuteAfterTime(1));
        StartCoroutine(WaitForPlayer(3));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        ObjectSpeed.sharedInstance.ResetPositionSpeedUser();
        // CameraController.sharedInstance.ResetCamera();
        TimerSkill.sharedInstance.ResetTimerVariables();
    }

    IEnumerator WaitForPlayer(float time)
    {
        yield return new WaitForSeconds(time);
        TriggerSpeed.sharedInstance.stop = false;
        PlayerSkiController.sharedInstance.startGame = true;
        ObjectSpeed.sharedInstance.rb.isKinematic = false;
        TimerSkill.sharedInstance.StartTimer();
    }

}
