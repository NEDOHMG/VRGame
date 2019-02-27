using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewInGame : MonoBehaviour
{

    public Image normalImage;
    public Image downImage;
    public Image UpImage;

    public static ViewInGame sharedInstance;

    void Awake()
    {
        sharedInstance = this;
    }

    void Start()
    {
        normalImage.enabled = true;
        downImage.enabled = false;
        UpImage.enabled = false;
    }

    public void StateNormal()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inTheGame)
        {
            normalImage.enabled = true;
            downImage.enabled = false;
            UpImage.enabled = false;
        }
    }

    public void StateDown()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inTheGame)
        {
            normalImage.enabled = false;
            downImage.enabled = true;
            UpImage.enabled = false;
        }
    }

    public void StateHigh()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inTheGame)
        {
            normalImage.enabled = false;
            downImage.enabled = false;
            UpImage.enabled = true;
        }
    }

    public void ResetStateSignal()
    {
        normalImage.enabled = true;
        downImage.enabled = false;
        UpImage.enabled = false;
    }

}
