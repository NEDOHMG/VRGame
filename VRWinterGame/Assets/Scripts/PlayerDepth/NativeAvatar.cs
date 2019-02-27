using UnityEngine;
using UnityEngine.UI;
using nuitrack;
using System.Collections.Generic;

public class NativeAvatar : MonoBehaviour
{
    string message = "";

    public JointType[] typeJoint;
    GameObject[] CreatedJoint;
    public GameObject LeftHand; // 8
    public GameObject RightHand; // 9

    // References for the canvas 
    //public Text LeftKneeText;
    //public Text LeftHipText;
    //public Text RightKneeText;
    //public Text RightHipText;

    // Vectors for the angles
    UnityEngine.Vector3 _footLeft;
    UnityEngine.Vector3 _ankleLeft;
    UnityEngine.Vector3 _kneeLeft;
    UnityEngine.Vector3 _hipLeft;

    // Center
    UnityEngine.Vector3 _spine;

    // Right
    UnityEngine.Vector3 _footRight;
    UnityEngine.Vector3 _ankleRight;
    UnityEngine.Vector3 _kneeRight;
    UnityEngine.Vector3 _hipRight;

    // Object of angle calculation
    private AnglesCalculation _anglesCalculation;

    // Variables used to hold the value of the angles
    float _kneeLeftAngle = 0.0f;
    float _hipLeftAngle = 0.0f;
    float _kneeRightAngle = 0.0f;
    float _hipRightAngle = 0.0f;

    void Start()
    {
        // Unity Vectors 
        _footLeft = new UnityEngine.Vector3(0.0f, 0.0f, 0.0f);
        _ankleLeft = new UnityEngine.Vector3(0.0f, 0.0f, 0.0f);
        _kneeLeft = new UnityEngine.Vector3(0.0f, 0.0f, 0.0f);
        _hipLeft = new UnityEngine.Vector3(0.0f, 0.0f, 0.0f);

        // Center
        _spine = new UnityEngine.Vector3(0.0f, 0.0f, 0.0f);

        // Right
        _footRight = new UnityEngine.Vector3(0.0f, 0.0f, 0.0f);
        _ankleRight = new UnityEngine.Vector3(0.0f, 0.0f, 0.0f);
        _kneeRight = new UnityEngine.Vector3(0.0f, 0.0f, 0.0f);
        _hipRight = new UnityEngine.Vector3(0.0f, 0.0f, 0.0f);

        _anglesCalculation = gameObject.GetComponent<AnglesCalculation>(); // Initialize the AnglesCalculation class

        CreatedJoint = new GameObject[typeJoint.Length];

        for (int q = 0; q < typeJoint.Length; q++)
        {
            if (q == 8)
            {
                CreatedJoint[q] = Instantiate(LeftHand);
                CreatedJoint[q].transform.SetParent(transform);

            }
            else if (q == 9)
            {
                CreatedJoint[q] = Instantiate(RightHand);
                CreatedJoint[q].transform.SetParent(transform);
            }
        }
        message = "Skeleton created";

    }

    void Update()
    {
        // Track the first user
        if (CurrentUserTracker.CurrentUser != 0)
        {
            Skeleton skeleton = CurrentUserTracker.CurrentSkeleton;
            message = "Skeleton found";


            for (int q = 0; q < typeJoint.Length; q++)
            {
                if (q == 8 || q == 9)
                {
                    nuitrack.Joint joint = skeleton.GetJoint(typeJoint[q]);
                    // CreatedJoint[q].transform.SetParent(transform, true);

                    // For the position 
                    UnityEngine.Vector3 newPosition = 0.001f * joint.ToVector3(); // This is a 3D unity Vector 
                    CreatedJoint[q].transform.localPosition = newPosition;

                    // For the rotation
                    //Quaternion jointOrient = ((CalibrationInfo.SensorOrientation) * joint.ToQuaternion());
                    // Quaternion jointOrient = Quaternion.Inverse((CalibrationInfo.SensorOrientation) * joint.ToQuaternion());
                    //CreatedJoint[q].transform.localRotation = jointOrient;
                }
            }

            // Get the information of the angles
            ShowAngles(skeleton);

        }
        else
        {
            message = "Skeleton not found";
        }
    }

    // Display the message on the screen
    void OnGUI()
    {
        GUI.color = Color.red;
        GUI.skin.label.fontSize = 50;
        GUILayout.Label(message);
    }

    void ShowAngles(Skeleton body)
    {
        // Left
        //_footLeft = body.Joints[(int)JointType.LeftFoot].Real;
        //_ankleLeft = body.Joints[(int)JointType.LeftAnkle].Real;
        //_kneeLeft = body.Joints[(int)JointType.LeftKnee].Real;
        //_hipLeft = body.Joints[(int)JointType.LeftHip].Real;

        //// Center
        //_spine = body.Joints[(int)JointType.Waist].Real;

        ////// Right
        //_footRight = body.Joints[(int)JointType.RightFoot].Real;
        //_ankleRight = body.Joints[(int)JointType.RightAnkle].Real;
        //_kneeRight = body.Joints[(int)JointType.RightKnee].Real;
        //_hipRight = body.Joints[(int)JointType.RightHip].Real;

        // Convert the nuitrack.Vector to Unity vector 
        _footLeft = body.Joints[(int)JointType.LeftFoot].ToVector3() * 0.001f;
        _ankleLeft = body.Joints[(int)JointType.LeftAnkle].ToVector3() * 0.001f;
        _kneeLeft = body.Joints[(int)JointType.LeftKnee].ToVector3() * 0.001f;
        _hipLeft = body.Joints[(int)JointType.LeftHip].ToVector3() * 0.001f;

        _spine = body.Joints[(int)JointType.Waist].ToVector3() * 0.001f;

        _footRight = body.Joints[(int)JointType.RightFoot].ToVector3() * 0.001f;
        _ankleRight = body.Joints[(int)JointType.RightAnkle].ToVector3() * 0.001f;
        _kneeRight = body.Joints[(int)JointType.RightKnee].ToVector3() * 0.001f;
        _hipRight = body.Joints[(int)JointType.RightHip].ToVector3() * 0.001f;

        // Calculation of the angles
        //float _ankleLeftAngle = _anglesCalculation.AngleBetweenTwoVectors(_ankleLeft - _kneeLeft, _ankleLeft - _footLeft);
        _kneeLeftAngle = _anglesCalculation.AngleBetweenTwoVectors(_kneeLeft - _hipLeft, _kneeLeft - _ankleLeft);
        _hipLeftAngle = _anglesCalculation.AngleBetweenTwoVectors(_hipLeft - _spine, _hipLeft - _kneeLeft);
        _kneeRightAngle = _anglesCalculation.AngleBetweenTwoVectors(_kneeRight - _hipRight, _kneeRight - _ankleRight);
        _hipRightAngle = _anglesCalculation.AngleBetweenTwoVectors(_hipRight - _spine, _hipRight - _kneeRight);

        //LeftAnkleText.text = "The left ankle angle is: " + _ankleLeftAngle.ToString();
        //LeftKneeText.text = "The left knee angle is: " + _kneeLeftAngle.ToString();
        //LeftHipText.text = "The left hip angle is: " + _hipLeftAngle.ToString();
        //RightKneeText.text = "The right knee is: " + _kneeRightAngle.ToString();
        //RightHipText.text = "The right hip angle is: " + _hipRightAngle.ToString();

    }

}