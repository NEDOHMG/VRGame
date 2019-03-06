using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private float originalFieldView = 20f;

    // The target of the camera 
    public GameObject target;
    public float yOffset = 0.35f;

    [HideInInspector]
    public float xOffset = 0.0f, zOffset = 0.35f;

    private Vector3 totalOffSet;

    // Triggers
    GameObject startPoint;
    CameraTriggerRotations cameraTriggerRotations;

    GameObject ramp;
    RampGenerator rampGenerator;

    // To sum the values
    float xi = 0.0f, yi = 0.0f, zi = 0.0f;

    //This is the field of view that the Camera has
    Camera _camera;

    public static CameraController sharedInstance;

    // To reset all the values of the camera
    public Quaternion originalRotationValue; 
    Vector3 originalPosition;
    // Use of quaternion instead of Vector
    public Quaternion cameraRotation;

    // To do the rotation of the camera
    Vector3 originalEulerRotation; // This save the original euler rotation
    Vector3 EulerRotation;
    Vector3 rotateValue;

    void Awake()
    {
        sharedInstance = this;
    }

    // Use this for initialization
    void Start()
    {
        _camera = GetComponent<Camera>();
        _camera.fieldOfView = originalFieldView;

        ramp = GameObject.Find("RampSpotGenerator");
        rampGenerator = ramp.GetComponent<RampGenerator>();

        totalOffSet = new Vector3(xOffset, yOffset, zOffset);
        startPoint = GameObject.FindGameObjectWithTag("Player");
        cameraTriggerRotations = startPoint.GetComponent<CameraTriggerRotations>();

        // Original values of the camera
        originalRotationValue = transform.rotation; // save the initial rotation
        originalPosition = transform.position;

        originalEulerRotation = transform.rotation.eulerAngles; // Save the original rotation in Euler, just in the case separate it from the others
        EulerRotation = originalEulerRotation;
        rotateValue = EulerRotation;

    }

    public void ResetCamera()
    {
        transform.position = originalPosition;
        // cameraRotation = originalRotationValue;
        transform.rotation = originalRotationValue;
        _camera.fieldOfView = originalFieldView;

        EulerRotation = originalEulerRotation;
        rotateValue = EulerRotation;
        xi = 0.0f;
        yi = 0.0f;
        zi = 0.0f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.transform.position + totalOffSet;

        if (cameraTriggerRotations.cameraRotationX == true)
        {
            xi += 0.025f;

            if (rotateValue.y < 364.0f && cameraTriggerRotations.cameraRotationY == true)
            {
                yi += 0.03f;
                rotateValue.y = EulerRotation.y + yi;
            }
            else if (rotateValue.y > 364.0f && cameraTriggerRotations.cameraRotationY == false)
            {

                rotateValue.y = 364.30f;
            }

            if (rotateValue.z <= 360f && cameraTriggerRotations.cameraRotationZ == true)
            {
                zi += 0.001f;
            }
            else
            {
                zi = 0;
            }

            if (rotateValue.x > -5.9f)
            {
                //Debug.Log("x: " + rotateValue.x + " y: " + rotateValue.y + " z: " + rotateValue.z);
                rotateValue.x = EulerRotation.x - xi;
                // rotateValue.y = originalRotation.y + yi;
                rotateValue.z = EulerRotation.z + zi;
                transform.eulerAngles = rotateValue;
            }
        }

        if (cameraTriggerRotations.cameraFieldView == true)
        {

            if (rampGenerator.levelOne == true && _camera.fieldOfView < 65)
            {
                _camera.fieldOfView += 0.05f;
            }
            else if (_camera.fieldOfView < 65)
            {
                _camera.fieldOfView += 0.02f;
            }
        }

    }

}
