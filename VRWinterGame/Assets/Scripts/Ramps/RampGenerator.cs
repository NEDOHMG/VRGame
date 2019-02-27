using System.Collections.Generic;
using UnityEngine;


public class RampGenerator : MonoBehaviour
{
    public List<RampBlock> allTheRamps = new List<RampBlock>(); // List that contain all the levels
    public List<RampBlock> currentRamps = new List<RampBlock>(); // List of the blocks that are currently displaying

    List<int> randomValues;

    [HideInInspector]
    public bool levelOne = false;

    int level;

    public static RampGenerator sharedInstance;

    void Awake()
    {
        sharedInstance = this;
    }

    public void SpawnRamps()
    {
        level = LevelDifficultSetup(PlayerSkiController.sharedInstance.gameDifficulty);

        if (level == 1)
        {
            levelOne = true;
        }
        GetNonRepeatRandom();
        GenerateRamp(level * 2);
    }

    int LevelDifficultSetup(LevelRampsGenerator _levelRamps)
    {
        int _level = 0;

        if (_levelRamps == LevelRampsGenerator.levelOne)
        {
            _level = 1;
        }
        else if (_levelRamps == LevelRampsGenerator.levelTwo)
        {
            _level = 2;
        }
        else if (_levelRamps == LevelRampsGenerator.levelThree)
        {
            _level = 3;
        }
        else if (_levelRamps == LevelRampsGenerator.levelFour)
        {
            _level = 4;
        }

        return _level;
    }

    // Randomize the position of the prefabs
    void GetNonRepeatRandom()
    {
        int lastRandomNumber = 10;
        randomValues = new List<int>();
        int randPairOrImpair = Random.Range(0, 2);
        
        for (int i = 0; i < allTheRamps.Count;)
        {

            int rand = Random.Range(0, allTheRamps.Count);

            if (rand != lastRandomNumber && !randomValues.Contains(rand))
            {
                // Here we will add for level one just if is pair or impair to give more "space" between the prefabs 
                if (level == 1 && randPairOrImpair == 0 && rand % 2 == 0)
                {

                    randomValues.Add(rand);
                    // Debug.Log(rand);

                }
                else if (level == 1 && randPairOrImpair == 1 && rand % 2 != 0)
                {
                    randomValues.Add(rand);
                    // Debug.Log(rand);
                }
                else if(level != 1)
                {
                    randomValues.Add(rand); // do it normally
                }

                i++;

            }

            lastRandomNumber = rand;

        }
    }

    // Generate the prefabs
    void GenerateRamp(int position)
    { 

        for (int i = 0; i < position; i++)
        {
            RampBlock ramp = Instantiate(allTheRamps[randomValues[i]]);
            currentRamps.Add(ramp);
        }
    }

    void RemoveOldRamps()
    {
        // Save the old block in a new object 
        RampBlock ramp = currentRamps[0];
        // Remove the block from the list
        currentRamps.Remove(ramp);
        // Destroy the block
        Destroy(ramp.gameObject);
    }

    public void RemoveAllTheRamps()
    {
        while (currentRamps.Count > 0)
        {
            RemoveOldRamps();
        }
        randomValues = new List<int>();
    }

}
