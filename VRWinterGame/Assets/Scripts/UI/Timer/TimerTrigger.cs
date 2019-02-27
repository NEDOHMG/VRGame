using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        GameObject.Find("TimerManager").SendMessage("UserFinished");
    }
}
