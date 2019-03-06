using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePGMController : MonoBehaviour
{
    public GamePGMHandler serialHandlerPGM;

    public void ActuateThePGM()
    {
        serialHandlerPGM.Write("1");
        Debug.Log("The PGM is actuated");
    }

    public void RelaxThePGM()
    {
        serialHandlerPGM.Write("0");
        Debug.Log("The PGM is not actuated");
    }
    // Update is called once per frame
}
