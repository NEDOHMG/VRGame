using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneCurrentStage { CalibrationScene, GameScene }


public class ScenePlayerManager : MonoBehaviour
{
    public static ScenePlayerManager sharedInstance;

    /// <summary>
    /// playerCalibrated will be use in future to change to the game scene
    /// returnToCalibrationStage will return the user to the calibration stage if the bottom in the GUI of the game is pressed
    /// </summary>

    [HideInInspector]
    public bool playerCalibrated = false, returnToCalibrationStage = false;

    SceneCurrentStage currentScene;

    public string[] GameVRScenes;

    void Awake()
    {
        sharedInstance = this;
    }

    //void Update()
    //{
    //    LoadDifferentScene();
    //}

    public void CalibrationScene()
    {
        // here we go how to choose the scene
    }

    public void SkiGameScene()
    {

    }

    //public void LoadDifferentScene()
    //{
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        Debug.Log("The scene was load");
    //        SceneManager.LoadScene(GameVRScenes[1]);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        SceneManager.LoadScene(GameVRScenes[0]);
    //    }
    //}

    void CurrentSceneGameManager(SceneCurrentStage _currentScene)
    {
        if (_currentScene == SceneCurrentStage.CalibrationScene)
        {
            SceneManager.LoadScene(GameVRScenes[0]);
        }
        else if (_currentScene == SceneCurrentStage.CalibrationScene)
        {
            SceneManager.LoadScene(GameVRScenes[1]);
        }
    }
}
