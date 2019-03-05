using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameCalibrationState
{
    menu,
    inTheGame
}

public class GameManagerCalibrationStage : MonoBehaviour
{
    // Actual GameState
    public GameState currentGameState = GameState.menu;

    public static GameManagerCalibrationStage sharedInstance;

    public Canvas CalibrationStageMenu;

    void Awake()
    {
        // Initialize the singleton and share all the GameManager fields and methods with it
        sharedInstance = this;
    }

    void Start()
    {
        currentGameState = GameState.menu;
        CalibrationStageMenu.enabled = true;
    }

    public void StartCalibration()
    {
        Debug.Log("In the game");
        ChangeGameState(GameState.inTheGame);
        Intel.sharedInstance.StartTheTrackingStage = true;
    }

    // This method will manage the states of the game
    void ChangeGameState(GameState newGameState)
    {
        if (newGameState == GameState.menu)
        {
            // The logic of the principal menu
            CalibrationStageMenu.enabled = true;
        }
        else if (newGameState == GameState.inTheGame)
        {
            // This is the current scene or level of the game
            CalibrationStageMenu.enabled = false;
        }

        // This is the new state after the change 
        currentGameState = newGameState;

    }
}
