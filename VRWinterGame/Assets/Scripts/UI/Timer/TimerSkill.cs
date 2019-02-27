using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerSkill : MonoBehaviour
{
    public Text textGameMenuTimer;
    public Text textGameOverTimer;

    public static TimerSkill sharedInstance;

    [HideInInspector]
    public bool stopTheGameTimer = true, showTheFinalTime = false;

    [HideInInspector]
    public float startTime = 0.0f;

    [HideInInspector]
    public string minutes = "", seconds = "", showMinutes = "", showSeconds = "";

    void Awake()
    {
        sharedInstance = this;
    }

    void Update()
    {
        if (stopTheGameTimer == false)
        {
            ShowTimer();
        }

        if (showTheFinalTime)
        {
            FinalTime();
        }
    }

    // On click this will run 
    public void StartTimer()
    {
        startTime = Time.time;
        stopTheGameTimer = false;
    }

    void ShowTimer()
    {
        float t = Time.time - startTime;
        minutes = ((int)t / 60).ToString();
        seconds = (t % 60).ToString("f4");
        textGameMenuTimer.text = minutes + " : " + seconds;

        if (showTheFinalTime == false)
        {
            showMinutes = minutes;
            showSeconds = seconds;
        }
    }

    void FinalTime()
    {
        textGameOverTimer.text = showMinutes + " : " + showSeconds;
    }


    // If we received a signal of the trigger stop the timer
    public void UserFinished()
    {
        stopTheGameTimer = true;
        showTheFinalTime = true;
    }

    public void ResetTimerVariables()
    {
        stopTheGameTimer = true;
        showTheFinalTime = false;
        startTime = 0.0f;
        minutes = "";
        seconds = "";
        showMinutes = "";
        showSeconds = "";
        textGameMenuTimer.text = "00:00";
    }

}
