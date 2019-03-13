using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerJump : MonoBehaviour
{

    Rigidbody rb;

    // Actual impulse level
    //public ImpulseLevel impulseLevel = ImpulseLevel.two;

    private float impulse = 0.03f;
    [HideInInspector]
    public bool firstJump = true;
    float _impulseJump;

    [HideInInspector]
    public bool jump = false;

    public static PlayerJump sharedInstance;

    void Awake()
    {
        sharedInstance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (jump)
        {
            UserJump(impulse);
        }

    }

    float ChooseSpeed(ImpulseLevel _impulseLevel)
    {

        if (_impulseLevel == ImpulseLevel.one)
        {
            _impulseJump = 0.23f;
        }
        else if (_impulseLevel == ImpulseLevel.two)
        {
            _impulseJump = 0.26f;
        }
        else if (_impulseLevel == ImpulseLevel.three)
        {
            _impulseJump = 0.29f;
        }
        else if (_impulseLevel == ImpulseLevel.four)
        {
            _impulseJump = 0.32f;
        }
        //else if (_impulseLevel == ImpulseLevel.five)
        //{
        //    _impulseJump = 0.35f;
        //}
        return _impulseJump;
    }

    void UserJump(float _impulse)
    {
        //Debug.Log(_impulse);
        rb.AddForce(Vector3.up * _impulse, ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Ramp"))
        {
            //Debug.Log("Enter");
            jump = true;
            if (firstJump)
            {
                firstJump = false;
            }
            else
            {
                impulse = ChooseSpeed(PlayerSkiController.sharedInstance.impulseLevel);
            }
        }
        else
        {
            jump = false;
        }
    }

    public void ResetJumpVariables()
    {
        firstJump = true;
        jump = false;
    }
}
