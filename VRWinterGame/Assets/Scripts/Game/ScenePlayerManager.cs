using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePlayerManager : MonoBehaviour
{
    [HideInInspector]
    public bool playerCalibrated = false, returnToCalibrationStage = false; // playerCalibrated will be use in future to change the scene

    

    public string[] GameVRScenes;

    void Update()
    {
        LoadDifferentScene();
    }

    public void LoadDifferentScene()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("The scene was load");
            SceneManager.LoadScene(GameVRScenes[1]);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(GameVRScenes[0]);
        }
    }
}
