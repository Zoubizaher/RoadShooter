using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{

    //CarComponents
    Rigidbody _rb;
    //Wheels
    public WheelCollider[] Twheels;
    public WheelCollider[] Rwheels;

    [SerializeField] float JumpForce = 1500.0f;
    [SerializeField] float MotorForce = 1500.0f;
    [SerializeField] float steerSpeed = 0.2f;
    [SerializeField] float steerAngle = 30.0f;

    public float MaxSpeed = 70f;
    public float CurrentSpeed = 0;

    //Ground SET
    public LayerMask GroundMasking;
    public bool isGrounded = false;
    public float rayCastLineDistanceToGround = 2f;
    public float DownForceVal = 20f;
    public Transform centerOfMass;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.ResetCenterOfMass();
        _rb.ResetInertiaTensor();
        _rb.centerOfMass = new Vector3(0, -1.9f, 0);

    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = getGround(rayCastLineDistanceToGround);

    }
    private void FixedUpdate()
    {
        SetTheCarControls();
    }
    public void SetTheCarControls()
    {
        

        SpeedUpdate();
        DownForce();

        float motor = Input.GetAxis("Horizontal") * MotorForce;
        SetTheEngine(motor);


        float steer = Input.GetAxis("Vertical") * steerAngle;
        SetTheSteeringWheels(steer * -1);

        if(isGrounded)
        {
            float Jump = Input.GetAxis("Jump");
            if(Jump == 1)
            {
                Jumping(JumpForce);

            }
        }


    }


    public void SetTheEngine(float inputKeysPress)
    {
        float speedLocal = Mathf.Abs(CurrentSpeed);

        foreach (WheelCollider RSWheels in Rwheels)
        {

            if (speedLocal < MaxSpeed)
            {
                RSWheels.motorTorque = inputKeysPress + 0.001f;

            }
            else
            {
                RSWheels.motorTorque = 0.001f;
            }
        }
        foreach (WheelCollider TSWheels in Twheels)
        {

            if (speedLocal > MaxSpeed)
            {
                TSWheels.motorTorque = 0.001f;

            }
            else
            {
                TSWheels.motorTorque = inputKeysPress + 0.001f;

            }

        }
    }

    public void SetTheSteeringWheels(float steer)
    {
        //Mathf.Clamp TO limit the min and the max value
        float steerWheels = Mathf.Clamp(steer, -steerAngle, steerAngle);

        //Adjusting The Value into the wheels
        foreach (WheelCollider Turnwheel in Twheels)
        {
            Turnwheel.steerAngle = steerWheels;

        }
    }
    public void DownForce()
    {
        _rb.AddForce(-Vector3.up * DownForceVal);
    }
    public void SpeedUpdate()
    {
        float CheckSpeed = transform.InverseTransformDirection(_rb.velocity).z * 3.6f;
        CurrentSpeed = CheckSpeed;
    }

    public void Jumping(float jumpForce)
    {
        _rb.AddForce(transform.up * jumpForce,ForceMode.Impulse);
    }    
    bool getGround(float RayFromGround)
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, RayFromGround, GroundMasking))
        {

            return true;
        }


        return false;



    }
}
