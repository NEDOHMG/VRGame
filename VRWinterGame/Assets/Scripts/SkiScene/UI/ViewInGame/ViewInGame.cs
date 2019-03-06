using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewInGame : MonoBehaviour
{

    public Image normalImage;
    public Image downImage;
    public Image UpImage;

    public Image normalImageVR;
    public Image downImageVR;
    public Image UpImageVR;

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

        normalImageVR.enabled = true;
        downImageVR.enabled = false;
        UpImageVR.enabled = false;

    }

    public void StateNormal()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inTheGame)
        {
            normalImage.enabled = true;
            downImage.enabled = false;
            UpImage.enabled = false;

            normalImageVR.enabled = true;
            downImageVR.enabled = false;
            UpImageVR.enabled = false;

        }
    }

    public void StateDown()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inTheGame)
        {
            normalImage.enabled = false;
            downImage.enabled = true;
            UpImage.enabled = false;

            normalImageVR.enabled = false;
            downImageVR.enabled = true;
            UpImageVR.enabled = false;
        }
    }

    public void StateHigh()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inTheGame)
        {
            normalImage.enabled = false;
            downImage.enabled = false;
            UpImage.enabled = true;

            normalImageVR.enabled = false;
            downImageVR.enabled = false;
            UpImageVR.enabled = true;
        }
    }

    public void ResetStateSignal()
    {
        normalImage.enabled = true;
        downImage.enabled = false;
        UpImage.enabled = false;

        normalImageVR.enabled = true;
        downImageVR.enabled = false;
        UpImageVR.enabled = false;
    }

}
