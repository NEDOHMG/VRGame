using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGMStates : MonoBehaviour
{

    public SerialHandlerPGM serialHandlerPGM;
    
    int currentState = 5;
    int previousState = 5;

    // Update is called once per frame
    void Update()
    {

        currentState = Intel.sharedInstance.ExerciseState;


        if (currentState == 0 && currentState != previousState)
        {
            if (Intel.sharedInstance.resistanceMode == false)
            {
                serialHandlerPGM.Write("1");
                Debug.Log("The PGM is actuated");
            }
            else
            {
                serialHandlerPGM.Write("0");
                Debug.Log("The PGM is not actuated");
            }
        }


        if (currentState == 2 && currentState != previousState)
        {
            if (Intel.sharedInstance.resistanceMode == false)
            {
                serialHandlerPGM.Write("0");
                Debug.Log("The PGM is not actuated");
            }
            else
            {
                serialHandlerPGM.Write("1");
                Debug.Log("The PGM is actuated");
            }

        }

        previousState = currentState;


    }
}
