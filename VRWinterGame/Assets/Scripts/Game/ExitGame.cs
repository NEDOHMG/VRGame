using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void CloseTheGame()
    {
        Debug.Log("the game was closed");
        Application.Quit();
    }
}
