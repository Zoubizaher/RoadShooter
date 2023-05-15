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
    [SerializeField] float steerAngle1 = 30.0f;

    [SerializeField] float NegativesteerAngle1 = -30.0f;

    [SerializeField] float PositivesteerAngle1 = 30.0f;

    public float MaxSpeed = 70f;
    public float CurrentSpeed = 0;

    //Ground SET
    public LayerMask GroundMasking;
    public bool isGrounded = false;
    public float rayCastLineDistanceToGround = 2f;
    public float DownForceVal = 20f;
    public Transform centerOfMass;
    public float roadAdjustmentSpeed = 2f;
    public ParticleSystem[] psExhust;
    public TrailRenderer[] TRRoadTrack;

    public float rayToWall = 2f;
    public LayerMask WallLimitMasking;
    public float tempAngleCheck;
    public bool WallIsDetect;

    public float minRotationAngle = 60f;
    public float maxRotationAngle = 120f;

    public bool isFinishedLERPING;

    public bool canMoveRight;

    public float ClampZCarPosRight = -100;
    public float ClampZCarPosleft = 100;

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
        SpeedUpdate();

    }
    private void FixedUpdate()
    {
        ClampingPos();
        SetTheCarControls();
        SetExhustEmit();

    }

    public void ClampingPos()
    {
        if (transform.position.z <= ClampZCarPosRight)
        {
            WallIsDetect = true;
            transform.position = new Vector3(transform.position.x, transform.position.y, ClampZCarPosRight);

        }
        else if(transform.position.z >= ClampZCarPosleft)
        {
            WallIsDetect = true;
            transform.position = new Vector3(transform.position.x, transform.position.y, ClampZCarPosleft);

        }
    }
    public void SetTheCarControls()
    {
        

        DownForce();
        ClampCarRotation();
        float steer = Input.GetAxis("Vertical") * steerAngle1;
        //Edit
        SetTheSteeringWheels1(steer * -1);
        float motor = MotorForce;
        SetTheEngine(Mathf.Abs(motor));




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
        //bool wallLimit = isDetectLimitWall(steer);

        //bool setLerpBackToCurrectDIrection = makeTheWheelSteerBackToRightDirectionReturnBool(steer);

        if (steer != 0)
        {
            //Mathf.Clamp TO limit the min and the max value
            float steerWheels = Mathf.Clamp(steer, NegativesteerAngle1, PositivesteerAngle1);
            //
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.forward, out hit, rayToWall, WallLimitMasking))
            {
                //NegativesteerAngle1 = -30f;
                //PositivesteerAngle1 = 30f;

                WallIsDetect = true;
                canMoveRight = true;
                if (WallIsDetect)
                {
                    if (canMoveRight)
                    {
                        if (steer < 0)
                        {
                            makeTheWheelSteerBackToRightDirection(steer);

                        }
                        else
                        {
                            foreach (WheelCollider Turnwheel in Twheels)
                            {
                                Turnwheel.steerAngle = steerWheels;

                            }
                        }
                    }

                }
            }
            else if (Physics.Raycast(transform.position, -Vector3.forward, out hit, rayToWall, WallLimitMasking))
            {
                //NegativesteerAngle1 = -30f;
                //PositivesteerAngle1 = 30f;

                WallIsDetect = true;
                canMoveRight = false;
                if (WallIsDetect)
                {
                    if (canMoveRight == false)
                    {
                        if (steer > 0)
                        {
                            makeTheWheelSteerBackToRightDirection(steer);

                        }
                        else
                        {
                            foreach (WheelCollider Turnwheel in Twheels)
                            {
                                Turnwheel.steerAngle = steerWheels;

                            }
                        }
                    }

                }

            }

            else
            {
                WallIsDetect = false;

                foreach (WheelCollider Turnwheel in Twheels)
                {
                    Turnwheel.steerAngle = steerWheels;

                }



            }
  

            //


        }
 
        else
        {
            makeTheWheelSteerBackToRightDirection(steer);


        }
    }




    public void SetTheSteeringWheels1(float steer)
    {
        //bool wallLimit = isDetectLimitWall(steer);

        //bool setLerpBackToCurrectDIrection = makeTheWheelSteerBackToRightDirectionReturnBool(steer);

        if (steer != 0)
        {
            //Mathf.Clamp TO limit the min and the max value
            float steerWheels = Mathf.Clamp(steer, NegativesteerAngle1, PositivesteerAngle1);
            //
            if (transform.position.z <= ClampZCarPosRight)
            {
                //NegativesteerAngle1 = -30f;
                //PositivesteerAngle1 = 30f;

                WallIsDetect = true;
                canMoveRight = true;
                if (WallIsDetect)
                {
                    if (canMoveRight)
                    {
                        if (steer > 0)
                        {
                            PositivesteerAngle1 = 0f;
                            makeTheWheelSteerBackToRightDirection(steer);

                        }
                        else
                        {
                            foreach (WheelCollider Turnwheel in Twheels)
                            {
                                Turnwheel.steerAngle = steerWheels;

                            }
                        }
                    }

                }
            }
            else if (transform.position.z >= ClampZCarPosleft)
            {
                //NegativesteerAngle1 = -30f;
                //PositivesteerAngle1 = 30f;

                WallIsDetect = true;
                canMoveRight = false;
                if (WallIsDetect)
                {
                    if (canMoveRight == false)
                    {
                        if (steer < 0)
                        {
                            NegativesteerAngle1 = 0f;
                            makeTheWheelSteerBackToRightDirection(steer);

                        }
                        else
                        {
                            foreach (WheelCollider Turnwheel in Twheels)
                            {
                                Turnwheel.steerAngle = steerWheels;

                            }
                        }
                    }

                }

            }

            else
            {
 

                foreach (WheelCollider Turnwheel in Twheels)
                {
                    if(WallIsDetect)
                    {
                        steerWheels = Mathf.Clamp(steer, NegativesteerAngle1, PositivesteerAngle1);
                        WallIsDetect = false;

                    }
                    else
                    {
                        if (transform.position.z <= ClampZCarPosleft - 3f && transform.position.z >= ClampZCarPosRight + 3f)
                        {
                            PositivesteerAngle1 = 30f;
                            NegativesteerAngle1 = -30f;
                        }
      
                    }
                    Turnwheel.steerAngle = steerWheels;

                }



            }


            //


        }

        else
        {
            makeTheWheelSteerBackToRightDirection(steer);


        }
    }

    public void SetTheSteeringWheels2(float steer)
    {
        //bool wallLimit = isDetectLimitWall(steer);

        //bool setLerpBackToCurrectDIrection = makeTheWheelSteerBackToRightDirectionReturnBool(steer);

        if (steer != 0)
        {
            //Mathf.Clamp TO limit the min and the max value
            float steerWheels = Mathf.Clamp(steer, NegativesteerAngle1, PositivesteerAngle1);
            //
            if (transform.position.z <= ClampZCarPosRight)
            {
                //NegativesteerAngle1 = -30f;
                //PositivesteerAngle1 = 30f;

                WallIsDetect = true;
                canMoveRight = true;
                if (WallIsDetect)
                {
                    if (canMoveRight)
                    {
                        if (steer > 0)
                        {

                            makeTheWheelSteerBackToRightDirection(steer);

                        }
                        else
                        {
                            foreach (WheelCollider Turnwheel in Twheels)
                            {
                                Turnwheel.steerAngle = steerWheels;

                            }
                        }
                    }

                }
            }
            else if (transform.position.z >= ClampZCarPosleft)
            {
                //NegativesteerAngle1 = -30f;
                //PositivesteerAngle1 = 30f;

                WallIsDetect = true;
                canMoveRight = false;
                if (WallIsDetect)
                {
                    if (canMoveRight == false)
                    {
                        if (steer < 0)
                        {
                            makeTheWheelSteerBackToRightDirection(steer);

                        }
                        else
                        {
                            foreach (WheelCollider Turnwheel in Twheels)
                            {
                                Turnwheel.steerAngle = steerWheels;

                            }
                        }
                    }

                }

            }

            else
            {

                foreach (WheelCollider Turnwheel in Twheels)
                {

                    Turnwheel.steerAngle = steerWheels;

                }
                WallIsDetect = false;


            }


            //


        }

        else
        {
            makeTheWheelSteerBackToRightDirection(steer);


        }
    }


    public void ClampCarRotation()
    {
        // Clamp the car's rotation within a certain range
        Quaternion rotation = transform.rotation;
        Vector3 euler = rotation.eulerAngles;
        euler.y = Mathf.Clamp(euler.y, minRotationAngle, maxRotationAngle);
        rotation.eulerAngles = euler;
        transform.rotation = rotation;
    }
    public void makeTheWheelSteerBackToRightDirection(float steer)
    {
        Vector3 rightDir = transform.right;
        Vector3 fwdDir = transform.forward;

        foreach (WheelCollider Turnwheel in Twheels)
        {

            if (Turnwheel.transform.localPosition.z > 0f)
            {
                Turnwheel.GetWorldPose(out Vector3 pos, out Quaternion rot);
                fwdDir = rot * Vector3.forward;
                rightDir = rot * Vector3.right;
                break;
            }


        }
        // Calculate the angle between the current right direction and the global right direction
        float angle = Vector3.SignedAngle(rightDir, Vector3.right, transform.up);

        // Calculate the desired forward direction based on the angle of the road
        fwdDir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;

        // Calculate the angle to steer each front wheel towards the desired forward direction
        float steerAngle = Vector3.SignedAngle(transform.forward, fwdDir, transform.up);
        steerAngle = Mathf.Clamp(steerAngle, -steerAngle1, steerAngle1);

        // Lerp the front wheel steer angles towards the desired steer angle
        foreach (WheelCollider Turnwheel in Twheels)
        {
            if (Turnwheel.transform.localPosition.z > 0f)
            {
                float steer1 = Mathf.Lerp(Turnwheel.steerAngle, steerAngle, Time.deltaTime * roadAdjustmentSpeed);
                Turnwheel.steerAngle = steer1;
            }

        }

    }

    public void makeTheWheelSteerBackToRightDirection1(float steer)
    {
        Vector3 rightDir = transform.right;
        Vector3 fwdDir = transform.forward;

        foreach (WheelCollider Turnwheel in Twheels)
        {

            if (Turnwheel.transform.localPosition.z > 0f)
            {
                Turnwheel.GetWorldPose(out Vector3 pos, out Quaternion rot);
                fwdDir = rot * Vector3.forward;
                rightDir = rot * Vector3.right;
                break;
            }


        }
        // Calculate the angle between the current right direction and the global right direction
        float angle = Vector3.SignedAngle(rightDir, Vector3.right, transform.up);

        // Calculate the desired forward direction based on the angle of the road
        fwdDir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;

        // Calculate the angle to steer each front wheel towards the desired forward direction
        float steerAngle = Vector3.SignedAngle(transform.forward, fwdDir, transform.up);
        steerAngle = Mathf.Clamp(steerAngle, NegativesteerAngle1, PositivesteerAngle1);

        // Lerp the front wheel steer angles towards the desired steer angle
        foreach (WheelCollider Turnwheel in Twheels)
        {
            if (Turnwheel.transform.localPosition.z > 0f)
            {
                float steer1 = Mathf.Lerp(Turnwheel.steerAngle, steerAngle, Time.deltaTime * roadAdjustmentSpeed);

                Turnwheel.steerAngle = steer1;
            }
   
        }

    }

    public bool makeTheWheelSteerBackToRightDirectionReturnBool(float steer)
    {
        if(WallIsDetect)
        {
            Vector3 rightDir = transform.right;
            Vector3 fwdDir = transform.forward;

            foreach (WheelCollider Turnwheel in Twheels)
            {

                if (Turnwheel.transform.localPosition.z > 0f)
                {
                    Turnwheel.GetWorldPose(out Vector3 pos, out Quaternion rot);
                    fwdDir = rot * Vector3.forward;
                    rightDir = rot * Vector3.right;
                    break;
                }


            }
            // Calculate the angle between the current right direction and the global right direction
            float angle = Vector3.SignedAngle(rightDir, Vector3.right, transform.up);
            if (angle > -91 && angle < -89f)
            {
                // Calculate the desired forward direction based on the angle of the road
                fwdDir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;

                // Calculate the angle to steer each front wheel towards the desired forward direction
                float steerAngle = Vector3.SignedAngle(transform.forward, fwdDir, transform.up);
                steerAngle = Mathf.Clamp(steerAngle, -steerAngle1, steerAngle1);

                // Lerp the front wheel steer angles towards the desired steer angle
                foreach (WheelCollider Turnwheel in Twheels)
                {
                    if (Turnwheel.transform.localPosition.z > 0f)
                    {
                        float steer1 = Mathf.Lerp(Turnwheel.steerAngle, steerAngle, Time.deltaTime * roadAdjustmentSpeed);
                        Turnwheel.steerAngle = steer1;
                    }
                    else
                    {

                        Turnwheel.steerAngle = steer;
                    }
                }
                isFinishedLERPING = true;

                return isFinishedLERPING;
            }
            else
            {
                isFinishedLERPING = false;

                return isFinishedLERPING;

            }

        }

        return false;

    }

    public void DownForce()
    {
        _rb.AddForce(-Vector3.up * DownForceVal);
    }
    public void SpeedUpdate()
    {
        float CheckSpeed = transform.InverseTransformDirection(_rb.velocity).z * 3.6f;
        CurrentSpeed = CheckSpeed;
        if(CheckSpeed > MaxSpeed)
        {
            CurrentSpeed = MaxSpeed;
        }
    }

    public void Jumping(float jumpForce)
    {
        _rb.AddForce(transform.up * jumpForce,ForceMode.Impulse);
    }
    public void SetExhustEmit()
    {
        if(isGrounded)
        {
            foreach(ParticleSystem psEx in psExhust)
            {
                float rate = 20 * MaxSpeed / CurrentSpeed;
                var emissionModule = psEx.emission;
                emissionModule.rateOverTime = rate;

            }
            foreach (TrailRenderer TrWheel in TRRoadTrack)
            {
                TrWheel.emitting = true;

            }
        }
        else
        {
            foreach (ParticleSystem psEx in psExhust)
            {
                var emissionModule = psEx.emission;
                emissionModule.rateOverTime = 0;
            }
            foreach (TrailRenderer TrWheel in TRRoadTrack)
            {
                TrWheel.emitting = false;

            }
        }
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

    bool isDetectLimitWall(float steer)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, rayToWall, WallLimitMasking))
        {
            //NegativesteerAngle1 = 30f;
            //makeTheWheelSteerBackToRightDirection(steer);
            return true;
        }
        else if (Physics.Raycast(transform.position, -Vector3.forward, out hit, rayToWall, WallLimitMasking))
        {
            //PositivesteerAngle1 = -30f;
            //makeTheWheelSteerBackToRightDirection(steer);

            return true;

        }
        else
        {
            //PositivesteerAngle1 = steerAngle1;

            //NegativesteerAngle1 = -steerAngle1;
            return false;

        }
    }

    
}

