using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nuitrack;

public class PlayerHands : MonoBehaviour
{
    string message = "";

    public JointType[] typeJoint;
    GameObject[] CreatedJoint;
    public GameObject LeftHand; // 1
    public GameObject RightHand; // 0

    static public PlayerHands sharedInstance;

    void Awake()
    {
        sharedInstance = this;
    }

    void Start()
    {

        CreatedJoint = new GameObject[typeJoint.Length];

        for (int q = 0; q < typeJoint.Length; q++)
        {
            if (q == 0)
            {
                CreatedJoint[q] = Instantiate(LeftHand);
                CreatedJoint[q].transform.SetParent(transform);

            }
            else if (q == 1)
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
                if (q == 0 || q == 1)
                {
                    nuitrack.Joint joint = skeleton.GetJoint(typeJoint[q]);

                    // For the position 
                    UnityEngine.Vector3 newPosition = 0.001f * joint.ToVector3(); // This is a 3D unity Vector 
                    CreatedJoint[q].transform.localPosition = newPosition;

                    // For the rotation
                    // Quaternion jointOrient = Quaternion.Inverse((CalibrationInfo.SensorOrientation) * joint.ToQuaternion());
                    // CreatedJoint[q].transform.localRotation = jointOrient;
                }
            }

            // isTracked = true;

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
}
