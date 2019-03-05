﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneCurrentStage { CalibrationScene, GameScene }

public class ScenePlayerManager : MonoBehaviour
{
    public static ScenePlayerManager sharedInstance;

    public SceneCurrentStage currentScene;

    public string[] GameVRScenes;

    void Awake()
    {
        sharedInstance = this;
    }

    void Start()
    {
        currentScene = SceneCurrentStage.CalibrationScene;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("The scene was load");
            SkiGameScene();
        }
    }

    public void SquatScene()
    {
        CurrentSceneGameManager(SceneCurrentStage.CalibrationScene);
    }

    public void SkiGameScene()
    {
        CurrentSceneGameManager(SceneCurrentStage.GameScene);
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
        else if (_currentScene == SceneCurrentStage.GameScene)
        {
            SceneManager.LoadScene(GameVRScenes[1]);
        }
    }
}
