using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;
using UnityEditor;
using System.IO;
using UnityEngine;

public class Intel : MonoBehaviour {


    #region Variables

    //public nuitrack.JointType[] typeJoint;

    // References for the canvas 
    //public Text LeftAnkleText;
    public Text LeftKneeText;
    public Text LeftHipText;
    //public Text RightAnkleText;
    public Text RightKneeText;
    public Text RightHipText;

    // Vectors for the angles
    Vector3 _footLeft;
    Vector3 _ankleLeft;
    Vector3 _kneeLeft;
    Vector3 _hipLeft;

    // Center
    Vector3 _spine;

    // Right
    Vector3 _footRight;
    Vector3 _ankleRight;
    Vector3 _kneeRight;
    Vector3 _hipRight;

    // Object of angle calculation
    private AnglesCalculation _anglesCalculation;

    // Variables used to hold the value of the angles
    float _kneeLeftAngle = 0.0f;
    float _hipLeftAngle = 0.0f;
    float _kneeRightAngle = 0.0f;
    float _hipRightAngle = 0.0f;

    // Variables used to calculte the flexion and extension angles of knees and hip
    public double LeftKneeFlexionAngle;
    public double LeftKneeExtensionAngle;
    public double RightKneeFlexionAngle;
    public double RightKneeExtensionAngle;
    public double LeftHipFlexionAngle;
    public double RightHipFlexionAngle;
    public double LeftHipExtensionAngle;
    public double RightHipExtensionAngle;
    public double FlexionTime;
    public double ExtensionTime;

    //Variable used to determine the number of seconds we are tracking joint deltas
    public int TrackingWindow = 2;

    //Boolean set to true if the subject has stopped moving (average  of spine base y-delta list is less than threshold value)
    public bool Calibrated = false;
    public bool Stopped = false;
    public double noisefilter = 0.002;

    //Subject state. 0 = stopped, extended. 1 = flexing. 2 = stopped, flexed. 3 = extending.
    public int ExerciseState = 0;
    public static Color StatusLightColor = Color.red;
    public int colorActuator = 1001;

    //fps variable stores frames per second, used for size of tracking and delta lists
    private double deltaTime = 0.0;
    public double fps = 0.0;
    public double MotionTimer = 0.0;

    //Threshold for determining stopped motion. Needs to be calibrated.
    public double threshold = 0.0;
    public double emergencythreshold = 0.3;
    public int emergencycounter = 0;

    // Variables used to save the Y values of the SpineBase
    Vector3 _spinBase;
    private List<double> _spineList;
    private List<double> _differencesSpinY;
    private double _differencesSpinYMaxAverage;
    private double _differencesSpinYMinAverage;
    private bool _spinTracking = false;
    private bool _spinTrackingMax = true;
    private bool _spinTrackingMin = false;

    // Variables to detect the time of execute the exercise
    private float _ySpinTimerStart = 0f;
    private float _ySpinTimerDownStop = 0f;
    private float _ySpinTimerHighStop = 0f;

    // Stop engine
    private Vector3 _jointHandL;
    private Vector3 _jointHead;

    //Variables for calculating threshold during start
    public int calTrackingWindow = 3;
    public List<double> _calspineList;
    public List<double> _deltacalspineY;
    private double _caldeltaAVG = 0.0;

    //Variables used to calculate knee lateral inputs
    public int LateralsWindowSize = 0;
    public List<double> LeftKneeLateralPositions;
    public List<double> LeftKneeLateralDeltas;
    public double LeftKneeAverageDelta = 0.0;
    public List<double> RightKneeLateralPositions;
    public List<double> RightKneeLateralDeltas;
    public double RightKneeAverageDelta = 0.0;

    //Variables for experimental motion stop detection
    public int StopWindowSize = 0;
    public List<double> SpineBaseVerticalPositions;
    public List<double> SpineBaseVerticalDeltas;
    public double SpineBaseAverageDelta = 0.0;

    //Variables for output to log file
    public static string preamble = "";
    public static string filename = "";
    public static string logfile = "";

    private static Texture2D _staticRectTexture;
    private static GUIStyle _staticRectStyle;

    public double SpineBaseCurrentAverage = 0.0;


    // Singleton
    public static Intel sharedInstance;

    // This is an experimental variable that will be remove in the future
    public bool resistanceMode = false;
    public double maxDeltaLeft = 0.0;
    public double maxDeltaRight = 0.0;

    #endregion

    // Note that this function is only meant to be called from OnGUI() functions.
    public static void GUIDrawRect(Rect position, Color color)
    {
        if (_staticRectTexture == null)
        {
            _staticRectTexture = new Texture2D(1, 1);
        }

        if (_staticRectStyle == null)
        {
            _staticRectStyle = new GUIStyle();
        }

        _staticRectTexture.SetPixel(0, 0, color);
        _staticRectTexture.Apply();

        _staticRectStyle.normal.background = _staticRectTexture;

        GUI.Box(position, GUIContent.none, _staticRectStyle);
    }

    void Awake()
    {
        // Initialize the vectors 
        // Left
        _footLeft = new Vector3(0.0f, 0.0f, 0.0f);
        _ankleLeft = new Vector3(0.0f, 0.0f, 0.0f);
        _kneeLeft = new Vector3(0.0f, 0.0f, 0.0f);
        _hipLeft = new Vector3(0.0f, 0.0f, 0.0f);

        // Center
        _spine = new Vector3(0.0f, 0.0f, 0.0f);

        // Right
        _footRight = new Vector3(0.0f, 0.0f, 0.0f);
        _ankleRight = new Vector3(0.0f, 0.0f, 0.0f);
        _kneeRight = new Vector3(0.0f, 0.0f, 0.0f);
        _hipRight = new Vector3(0.0f, 0.0f, 0.0f);

        // Emergency stop 
        _jointHandL = new Vector3(0.0f, 0.0f, 0.0f);
        _jointHead = new Vector3(0.0f, 0.0f, 0.0f);

        // Initialize the singleton and share all the GameManager fields and methods with it
        sharedInstance = this;

    }

    // Use this for initialization
    void Start()
    {

        _anglesCalculation = GetComponent<AnglesCalculation>(); // Initialize the AnglesCalculation class

        //initialize the knee lateral lists
        LeftKneeLateralPositions = new List<double>();
        RightKneeLateralPositions = new List<double>();
        LeftKneeLateralDeltas = new List<double>();
        RightKneeLateralDeltas = new List<double>();

        // Initialize the list for the Spin
        _spineList = new List<double>();
        _differencesSpinY = new List<double>();
        _calspineList = new List<double>();

        //initialize log file name based on current date and time
        DateTime dt = DateTime.Now;
        preamble = "Assets/Resources/";
        filename = "log_" + dt.ToString("yyyy-MM-dd_HH-mm-ss");
        logfile = preamble + filename + ".txt";

        threshold = 0.0;
        noisefilter = 0.002;
        TrackingWindow = 2;

    }

    //write an entry to the log file
    //static void WriteLog(string entry)
    //{
    //    if (!File.Exists(logfile))
    //    {
    //        StreamWriter testing = File.CreateText(logfile);
    //        testing.Close();
    //    }
    //    //Write some text to the test.txt file
    //    StreamWriter writer = File.AppendText(logfile);
    //    writer.WriteLine(entry);
    //    writer.Close();

    //    //Re-import the file to update the reference in the editor
    //    AssetDatabase.ImportAsset(logfile);
    //    TextAsset asset = Resources.Load(filename) as TextAsset;

    //    //Print the text from the file
    //    //Debug.Log(asset.text);
    //}

    // Update is called once per frame
    void Update()
    {
        //FPS calculation
        deltaTime += (double)Time.deltaTime;
        deltaTime /= 2.0;
        fps = 1.0 / deltaTime;

        // Track first user 
        // Track the first user
        if (CurrentUserTracker.CurrentUser != 0)
        {
            nuitrack.Skeleton body = CurrentUserTracker.CurrentSkeleton;
            //Debug.Log("Skeleton found");

            // Get the information of the angles
            ShowAngles(body);

        }
        else
        {
            return;
            //Debug.Log("Skeleton not found");
        }

        // Emergency stop
        //EmergencyStop(body);

        //calibrate the Threshold
        CalibrateThreshold(_spine.y);
        //calibrate(_spinBase.y);

        //track knee lateral motion
        TrackKneeLateralMotion(_kneeLeft.x, _kneeRight.x);

        //detect if Stopped
        DetectStop(_spine.y);

        //update the exercise state
        UpdateExerciseState(_kneeLeftAngle, _kneeRightAngle, _hipLeftAngle, _hipRightAngle);

    }

    #region Events

    void ShowAngles(nuitrack.Skeleton body)
    {

        // Convert the nuitrack.Vector to Unity vector 
        _footLeft = body.Joints[(int)nuitrack.JointType.LeftFoot].ToVector3() * 0.001f;
        _ankleLeft = body.Joints[(int)nuitrack.JointType.LeftAnkle].ToVector3() * 0.001f;
        _kneeLeft = body.Joints[(int)nuitrack.JointType.LeftKnee].ToVector3() * 0.001f;
        _hipLeft = body.Joints[(int)nuitrack.JointType.LeftHip].ToVector3() * 0.001f;

        _spine = body.Joints[(int)nuitrack.JointType.Waist].ToVector3() * 0.001f;

        _footRight = body.Joints[(int)nuitrack.JointType.RightFoot].ToVector3() * 0.001f;
        _ankleRight = body.Joints[(int)nuitrack.JointType.RightAnkle].ToVector3() * 0.001f;
        _kneeRight = body.Joints[(int)nuitrack.JointType.RightKnee].ToVector3() * 0.001f;
        _hipRight = body.Joints[(int)nuitrack.JointType.RightHip].ToVector3() * 0.001f;

        // Calculation of the angles
        //float _ankleLeftAngle = _anglesCalculation.AngleBetweenTwoVectors(_ankleLeft - _kneeLeft, _ankleLeft - _footLeft);
        _kneeLeftAngle = _anglesCalculation.AngleBetweenTwoVectors(_kneeLeft - _hipLeft, _kneeLeft - _ankleLeft);
        _hipLeftAngle = _anglesCalculation.AngleBetweenTwoVectors(_hipLeft - _spine, _hipLeft - _kneeLeft);
        _kneeRightAngle = _anglesCalculation.AngleBetweenTwoVectors(_kneeRight - _hipRight, _kneeRight - _ankleRight);
        _hipRightAngle = _anglesCalculation.AngleBetweenTwoVectors(_hipRight - _spine, _hipRight - _kneeRight);

        //LeftAnkleText.text = "The left ankle angle is: " + _ankleLeftAngle.ToString();
        LeftKneeText.text = "The left knee angle is: " + _kneeLeftAngle.ToString();
        LeftHipText.text = "The left hip angle is: " + _hipLeftAngle.ToString();
        RightKneeText.text = "The right knee is: " + _kneeRightAngle.ToString();
        RightHipText.text = "The right hip angle is: " + _hipRightAngle.ToString();

    }

    //Experimental Function to Calibrate the threshold variable
    private void CalibrateThreshold(double CurrentSpineBaseVertical)
    {
        if (Calibrated)
            return;

        //get the size of the tracking window
        if (StopWindowSize == 0)
        {
            StopWindowSize = (int)(fps * TrackingWindow);
            return;
        }
        int trackingsize = StopWindowSize;

        //add the new positions to the positions lists
        SpineBaseVerticalPositions.Add(CurrentSpineBaseVertical);

        //still need more values
        if (SpineBaseVerticalPositions.Count < trackingsize)
            return;

        //calculate our first delta list
        for (int i = 0; i < trackingsize - 1; i++)
        {
            SpineBaseVerticalDeltas.Add(SpineBaseVerticalPositions[i + 1] - SpineBaseVerticalPositions[i]);
        }

        //get the absolute value delta maximum
        double AverageDelta = 0.0;
        for (int i = 0; i < SpineBaseVerticalDeltas.Count; i++)
        {
            AverageDelta += Math.Abs(SpineBaseVerticalDeltas[i]);
        }
        AverageDelta = AverageDelta / SpineBaseVerticalDeltas.Count;

        //set Threshold
        threshold = Math.Abs(AverageDelta) + noisefilter;

        //calibrated
        Calibrated = true;
        StatusLightColor = Color.green;
        colorActuator = 100;


    }

    private void calibrate(double currentSpinePoint)
    {
        if (Calibrated == true)
        {
            //Console.WriteLine("Already calibrated!!");
            return;
        }

        else if (Calibrated == false)
        {
            //calculate the number of samples for the tracking time (3 seconds in this case)
            if (StopWindowSize == 0)
            {
                StopWindowSize = (int)(fps * calTrackingWindow);
                return;
            }
            int trackingtime = StopWindowSize;

            //add newest data point to our list
            _calspineList.Add(currentSpinePoint);

            //if we dont have 3 seconds of values yet, stop here
            if (_calspineList.Count < trackingtime)
                return;


            //get the delta values
            for (int i = 0; i < trackingtime - 1; i++)
            {
                _deltacalspineY.Add(_calspineList[i + 1] - _calspineList[i]);
            }

            //calculate the average of the delta values
            double temp = 0.0;
            for (int i = 0; i < _deltacalspineY.Count; i++)
            {
                temp += Math.Abs(_deltacalspineY[i]); //if needed, the value must be rounded off
            }
            _caldeltaAVG = temp / _deltacalspineY.Count;

            //Threshold value
            threshold = Math.Abs(_caldeltaAVG);
            Calibrated = true;
            StatusLightColor = Color.green;
            colorActuator = 100;
        }
    }

    //Update the exercise state depending on the current state and whether or not the subject has stopped or started moving.
    private void UpdateExerciseState(double leftkneeangle, double rightkneeangle, double lefthipangle, double righthipangle)
    {

        MotionTimer += (double)Time.deltaTime;

        //if subject starts moving down, change state to 1
        if (Calibrated && !Stopped && ExerciseState == 0)
        {
            ExerciseState = 1;

            //reset the lateral variables
            LeftKneeAverageDelta = 0.0;
            RightKneeAverageDelta = 0.0;
            LeftKneeLateralPositions = new List<double>();
            RightKneeLateralPositions = new List<double>();
            LeftKneeLateralDeltas = new List<double>();
            RightKneeLateralDeltas = new List<double>();

            //reset the motion timer
            MotionTimer = 0.0;

            StatusLightColor = Color.yellow;
            colorActuator = 1010;
        }
        //if subject stops moving down, change state to 2
        else if (Stopped && ExerciseState == 1)
        {
            ExerciseState = 2;

            LeftKneeFlexionAngle = leftkneeangle;
            RightKneeFlexionAngle = rightkneeangle;
            LeftHipFlexionAngle = lefthipangle;
            RightHipFlexionAngle = righthipangle;
            FlexionTime = MotionTimer;

            StatusLightColor = Color.green;
            colorActuator = 100;
        }
        //if subject starts moving up, change state to 3
        else if (!Stopped && ExerciseState == 2)
        {
            ExerciseState = 3;

            //reset the lateral variables
            LeftKneeAverageDelta = 0.0;
            RightKneeAverageDelta = 0.0;
            LeftKneeLateralPositions = new List<double>();
            RightKneeLateralPositions = new List<double>();
            LeftKneeLateralDeltas = new List<double>();
            RightKneeLateralDeltas = new List<double>();

            //reset the motion timer
            MotionTimer = 0.0;

            StatusLightColor = Color.yellow;
            colorActuator = 1010;
        }
        //if subject stops moving up, change state to 4
        else if (Stopped && ExerciseState == 3)
        {
            ExerciseState = 0;

            LeftKneeExtensionAngle = leftkneeangle;
            RightKneeExtensionAngle = rightkneeangle;
            LeftHipExtensionAngle = lefthipangle;
            RightHipExtensionAngle = righthipangle;
            ExtensionTime = MotionTimer;

            StatusLightColor = Color.green;
            colorActuator = 100;

            // This code is a probe
            double resultProbeLeft = 180.0 - Math.Abs(LeftKneeExtensionAngle - LeftKneeFlexionAngle);
            double resultProbeRight = 180.0 - Math.Abs(RightKneeExtensionAngle - RightKneeFlexionAngle);

            if (resultProbeLeft > 60.0 && resultProbeRight > 60.0 && maxDeltaLeft > 0.1 && maxDeltaRight > 0.1)
            {
                resistanceMode = false;
            }
            else
            {
                resistanceMode = true;
            }

            //upload current values to neural network here
            //WriteLog(LeftKneeFlexionAngle + " " + LeftKneeExtensionAngle + " " + RightKneeFlexionAngle + " " + RightKneeExtensionAngle + " " + LeftHipFlexionAngle + " " + LeftHipExtensionAngle + " " + RightHipFlexionAngle + " " + RightHipExtensionAngle + " " + LeftKneeAverageDelta + " " + RightKneeAverageDelta + " " + _differencesSpinYMaxAverage + " " + _differencesSpinYMinAverage + " " + FlexionTime + " " + ExtensionTime);
        }

        if (!Calibrated)
        {
            StatusLightColor = Color.red;
            colorActuator = 1001;
        }
    }

    //Experimental function to potentially detect when a subject has stopped motion
    private void DetectStop(double CurrentSpineBaseVertical)
    {
        if (!Calibrated)
            return;

        //get the size of the tracking window
        if (StopWindowSize == 0)
        {
            StopWindowSize = (int)(fps * TrackingWindow);
            return;
        }
        int trackingsize = StopWindowSize;

        //add the new positions to the positions lists
        SpineBaseVerticalPositions.Add(CurrentSpineBaseVertical);

        //still need more values
        if (SpineBaseVerticalPositions.Count < trackingsize)
            return;
        else if (SpineBaseVerticalPositions.Count > trackingsize)
        {

            //update the positions lists
            SpineBaseVerticalPositions.RemoveAt(0);

            //update the deltas lists
            SpineBaseVerticalDeltas.RemoveAt(0);
            SpineBaseVerticalDeltas.Add(SpineBaseVerticalPositions[trackingsize - 1] - SpineBaseVerticalPositions[trackingsize - 2]);

            //get the current average
            //SpineBaseCurrentAverage = SpineBaseVerticalDeltas.Average();

            SpineBaseCurrentAverage = 0;

            //if(Math.Abs(SpineBaseCurrentAverage) > SpineBaseAverageDelta)

            double AverageDelta = 0.0;
            for (int i = 0; i < SpineBaseVerticalDeltas.Count; i++)
            {
                AverageDelta += Math.Abs(SpineBaseVerticalDeltas[i]);
            }
            AverageDelta = AverageDelta / SpineBaseVerticalDeltas.Count;

            SpineBaseCurrentAverage = AverageDelta;

            //    SpineBaseAverageDelta = Math.Abs(SpineBaseCurrentAverage);

            //update stopped state
            if (Math.Abs(SpineBaseCurrentAverage) <= Math.Abs(threshold) && !Stopped)
            {
                Stopped = true;
            }
            else if (Math.Abs(SpineBaseCurrentAverage) > Math.Abs(threshold) && Stopped)
            {
                Stopped = false;
            }

        }
        else
        {
            //calculate our first delta list
            for (int i = 0; i < trackingsize - 1; i++)
            {
                SpineBaseVerticalDeltas.Add(SpineBaseVerticalPositions[i + 1] - SpineBaseVerticalPositions[i]);
            }
        }

    }

    private void TrackKneeLateralMotion(double CurrentLeftKneeLateral, double CurrentRightKneeLateral)
    {

        //get the size of the tracking window
        if (LateralsWindowSize == 0)
        {
            LateralsWindowSize = (int)(fps * TrackingWindow);
            return;
        }
        int trackingsize = LateralsWindowSize;

        //add the new positions to the positions lists
        LeftKneeLateralPositions.Add(CurrentLeftKneeLateral);
        RightKneeLateralPositions.Add(CurrentRightKneeLateral);

        //still need more values
        if (LeftKneeLateralPositions.Count < trackingsize)
            return;
        else if (LeftKneeLateralPositions.Count > trackingsize)
        {
            //update the positions lists
            LeftKneeLateralPositions.RemoveAt(0);
            RightKneeLateralPositions.RemoveAt(0);

            //update the deltas lists
            LeftKneeLateralDeltas.RemoveAt(0);
            RightKneeLateralDeltas.RemoveAt(0);
            LeftKneeLateralDeltas.Add(Math.Abs(LeftKneeLateralPositions[trackingsize - 1] - LeftKneeLateralPositions[trackingsize - 2]));
            RightKneeLateralDeltas.Add(Math.Abs(RightKneeLateralPositions[trackingsize - 1] - RightKneeLateralPositions[trackingsize - 2]));

            //get the delta averages
            double LeftKneeCurrentAverage = LeftKneeLateralDeltas.Average();
            double RightKneeCurrentAverage = RightKneeLateralDeltas.Average();

            // This is the maximum delta of the left and right legs
            maxDeltaLeft = LeftKneeLateralDeltas.Max();
            maxDeltaRight = RightKneeLateralDeltas.Max();

            //update the deltas
            if (Math.Abs(LeftKneeCurrentAverage) > LeftKneeAverageDelta)
                LeftKneeAverageDelta = Math.Abs(LeftKneeCurrentAverage);
            if (Math.Abs(RightKneeCurrentAverage) > RightKneeAverageDelta)
                RightKneeAverageDelta = Math.Abs(RightKneeCurrentAverage);
        }
        else
        {

            //calculate our first delta list for knees
            for (int i = 0; i < trackingsize - 1; i++)
            {
                LeftKneeLateralDeltas.Add(LeftKneeLateralPositions[i + 1] - LeftKneeLateralPositions[i]);
                RightKneeLateralDeltas.Add(RightKneeLateralPositions[i + 1] - RightKneeLateralPositions[i]);
            }

        }


    }

    private void SpinDeltaYTracking(Vector3 _spinBase)
    {

        // Calculate the max for 3 seconds
        if (_spinTrackingMax == true && fps < TrackingWindow)
        {

            // Add the y spin values in the list
            _spineList.Add(_spinBase.y);
            // Calculate the deltas
            for (int i = 0; i < _spineList.Count - 1; i++)
            {
                _differencesSpinY.Add((_spineList[i + 1] - _spineList[i]));
            }
            // This variable has the delta average of the spin user
            _differencesSpinYMaxAverage = _differencesSpinY.Average();
            // Activate the flag to track the minimum
            _spinTrackingMin = true;
            // refresh the list to use them again
            _spineList.Clear();
            _differencesSpinY.Clear();
            _spinTrackingMax = false;

        }

        // Start the time to detect if the user is moving here
        // initialize the timer of squat exercise
        _ySpinTimerStart += Time.deltaTime;

        // Calculate the min
        if (_spinTrackingMin == true && Stopped)
        {

            _ySpinTimerDownStop = _ySpinTimerStart;
            _ySpinTimerStart = 0;
            _ySpinTimerStart += Time.deltaTime;
            // Add the y spin values in the list
            _spineList.Add(_spinBase.y);

        }

        // Calculate the delta of the min value
        for (int i = 0; i < _spineList.Count - 1; i++)
        {
            _differencesSpinY.Add((_spineList[i + 1] - _spineList[i]));
        }

        // Calculate the average of the min delta
        _differencesSpinYMinAverage = _differencesSpinY.Average();
        _spinTrackingMin = false;

        // Calculate the time to achieve the normal posture again
        if (Stopped)
        {

            _ySpinTimerHighStop = _ySpinTimerStart;
            _ySpinTimerStart = 0;
            _spinTracking = false;
            _spineList.Clear();
            _differencesSpinY.Clear();
        }
    }

    private void EmergencyStop(nuitrack.Skeleton body)
    {
        _jointHandL = body.Joints[(int)nuitrack.JointType.LeftHand].ToVector3() * 0.001f;
        _jointHead = body.Joints[(int)nuitrack.JointType.Head].ToVector3() * 0.001f;

        float w = Math.Abs(_jointHandL.y - _jointHead.y);
        //Debug.Log("This is the abs value " + w + "and this is the threshold " + emergencythreshold);

        if (Math.Abs(_jointHandL.y - _jointHead.y) < emergencythreshold)
        {
            emergencycounter++;
            if (emergencycounter > 200)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            {
                emergencycounter = 0;
            }
        }
    }

    #endregion
}
