using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickANN : MonoBehaviour
{
    public static QuickANN sharedThis;

    public double[] outputs;
    public double[,] weights;
    [HideInInspector]
    public int inodes = 9, onodes = 3, h1nodes = 6, h2nodes = 6;
    
    public static bool CalculateTheSkill = false;

    void Awake()
    {
        sharedThis = this;
    }

    // Use this for initialization
    void Start()
    {
        weights = new double[3, 9];
        weights[0, 0] = 1.51534146;
        weights[0, 1] = -2.33182275;
        weights[0, 2] = 3.85516666;
        weights[0, 3] = 2.52102815;
        weights[0, 4] = 3.35533239;
        weights[0, 5] = -2.58820334;
        weights[0, 6] = -0.0176415;
        weights[0, 7] = 5.67641528;
        weights[0, 8] = 0.31952423;
        weights[1, 0] = 1.09562435;
        weights[1, 1] = -0.0202774;
        weights[1, 2] = 0.22113618;
        weights[1, 3] = 0.28598537;
        weights[1, 4] = 0.15001746;
        weights[1, 5] = -0.12090328;
        weights[1, 6] = 0.50482869;
        weights[1, 7] = 0.3349205;
        weights[1, 8] = -0.67261979;
        weights[2, 0] = 10.95721507;
        weights[2, 1] = -4.57126731;
        weights[2, 2] = -5.39863459;
        weights[2, 3] = 0.66909638;
        weights[2, 4] = 1.38444687;
        weights[2, 5] = 5.51497129;
        weights[2, 6] = 1.99774644;
        weights[2, 7] = -11.92855983;
        weights[2, 8] = 1.76752782;

        outputs = new double[3];
        outputs[0] = 0.0;
        outputs[1] = 0.0;
        outputs[2] = 0.0;

    }

    //Normalize Inputs and find Difficulty
    public int Adapt(double _leftKneeFlexionAngle,
        double _leftKneeExtensionAngle,
        double _rightKneeFlexionAngle,
        double _rightKneeExtensionAngle,
        double _leftHipFlexionAngle,
        double _leftHipExtensionAngle,
        double _rightHipFlexionAngle,
        double _rightHipExtensionAngle,
        double _extensionTime,
        double _leftKneeAverageDelta,
        double _rightKneeAverageDelta)
    {
        double LeftKneeChange = Math.Abs(_leftKneeFlexionAngle - _leftKneeExtensionAngle);
        double RightKneeChange = Math.Abs(_rightKneeFlexionAngle - _rightKneeExtensionAngle);
        double LeftHipChange = Math.Abs(_leftHipFlexionAngle - _leftHipExtensionAngle);
        double RightHipChange = Math.Abs(_rightHipFlexionAngle - _rightHipExtensionAngle);
        double ComSmoothness = 1.0 - ((0.25 * Math.Abs(LeftKneeChange - RightKneeChange) / 45.0)
            + (0.25 * Math.Abs(LeftHipChange - RightHipChange) / 45.0)
            + (0.5 * Math.Abs(_leftKneeAverageDelta))
            + (0.5 * Math.Abs(_rightKneeAverageDelta)));
        double[] NormalizedInputs = { 1.0 - (_leftKneeAverageDelta * 1000.0),
            1.0 - (_rightKneeAverageDelta * 1000.0),
            (ComSmoothness - 0.7) / 0.3, 1.0 - ((_extensionTime - 1.5) / 0.5),
            (10.0 - 7.0) / 5.0,
            Math.Abs(_leftKneeExtensionAngle - _leftKneeFlexionAngle) / 105.0,
            Math.Abs(_rightKneeExtensionAngle - _rightKneeFlexionAngle) / 105.0,
            Math.Abs(_leftHipExtensionAngle - _leftHipFlexionAngle) / 45.0,
            Math.Abs(_rightHipExtensionAngle - _rightHipFlexionAngle) / 45.0 };
        return selectDifficulty(NormalizedInputs);
    }

    //Quickly select difficulty based on pre-trained NN weights and clinical decision values using 1 forward run with no hiddens
    public int selectDifficulty(double[] inputs)
    {
        for (int j = 0; j < onodes; j++)
        {
            for (int i = 0; i < inodes; i++)
            {
                outputs[j] += inputs[i] * weights[j, i];
                outputs[j] = activation(outputs[j]);
            }
        }

        //clinical decision boundaries apply here:
        int finalresult = 0;
        if (outputs[0] >= 5 && outputs[1] >= 1.3 && outputs[2] < 7)
        {
            finalresult = 4;
        }
        else if (outputs[0] < 2 || outputs[1] < 1.1 || outputs[2] >= 16)
        {
            finalresult = 1;
        }
        else if (outputs[0] < 3.5 || outputs[1] < 1.2 || outputs[2] >= 13)
        {
            finalresult = 2;
        }
        else
        {
            finalresult = 3;
        }

        return finalresult;
    }

    // Update is called once per frame
    void Update()
    {
        if (Intel.sharedInstance.StartTheTrackingStage == true)
        {
            //GameStaticVariables.UserSkillLevel = Adapt(Intel.sharedInstance.LeftKneeFlexionAngle,
            //    Intel.sharedInstance.LeftKneeExtensionAngle,
            //    Intel.sharedInstance.RightKneeFlexionAngle,
            //    Intel.sharedInstance.RightKneeExtensionAngle,
            //    Intel.sharedInstance.LeftHipFlexionAngle,
            //    Intel.sharedInstance.LeftHipExtensionAngle,
            //    Intel.sharedInstance.RightHipFlexionAngle,
            //    Intel.sharedInstance.RightHipExtensionAngle,
            //    Intel.sharedInstance.ExtensionTime,
            //    Intel.sharedInstance.LeftKneeAverageDelta,
            //    Intel.sharedInstance.RightKneeAverageDelta);
            if(CalculateTheSkill == true)
            {
                ScenePlayerManager.sharedInstance.SkiGameScene();
                CalculateTheSkill = false;
            }
        }
    }

    //Activation Function
    private double activation(double x)
    {
        //Sigmoid
        //return 1 / (1 + System.Math.Exp(-x));
        //RelUPlain
        return x;
    }
}
