using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ImpulseLevel
{
    one, two, three, four
    //, five
};

public enum LevelRampsGenerator
{
    levelOne, levelTwo, levelThree, levelFour
};

public class PlayerSkiController : MonoBehaviour
{
    public float speedAdjustmentAceleration = 0.4f;
    public float rotationXPoint = 0.05f;
    public float rotationZPoint = 0.1f;

    // Actual impulse level
    // public ImpulseLevel impulseLevel = ImpulseLevel.two;

    [HideInInspector]
    public ImpulseLevel impulseLevel;


    // Generate the number of ramps
    // public LevelRampsGenerator gameDifficulty = LevelRampsGenerator.levelOne;

    [HideInInspector]
    public LevelRampsGenerator gameDifficulty;

    //[HideInInspector]
    public bool startGame = false;

    //[HideInInspector]
    public bool resetPositionUser = false;

    public static PlayerSkiController sharedInstance;

    void Awake()
    {
        sharedInstance = this;
    }

    void Start()
    {
        Debug.Log(GameStaticVariables.UserSkillLevel);
        impulseLevel = (ImpulseLevel)(GameStaticVariables.UserSkillLevel - 1);
        gameDifficulty = (LevelRampsGenerator)(GameStaticVariables.UserSkillLevel - 1);
    }
}
