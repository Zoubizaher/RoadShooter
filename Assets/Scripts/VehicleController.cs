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
    [SerializeField] float JumpForceRightDirection = 150.0f;

    [SerializeField] float MotorForce = 1500.0f;
    public float motorForceInstance;
    [SerializeField] float steerSpeed = 0.2f;
    [SerializeField] float steerAngle1 = 30.0f;

    [SerializeField] float NegativesteerAngle1 = -30.0f;

    [SerializeField] float PositivesteerAngle1 = 30.0f;

    public float MaxSpeed = 70f;
    public float CurrentSpeed = 0;
    public float brakingPower = 500f;

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


    public bool CarIsShield = false;
    public bool PlayerISDead = false;

    //Destruction Values
    public float radius = 5.0F;
    public float power = 10.0F;

    [Range(1, 500000)]
    public float Minpower = 10.0F;

    [Range(1, 500000)]
    public float Maxpower = 10.0F;

    public float uplifter = 3.0f;

    [Range(1, 50000)]
    public float Minuplifter = 3.0f;

    [Range(1, 50000)]
    public float Maxuplifter = 3.0f;

    [SerializeField] List<Collider> AllCollidersRefrence;


    public GameObject SHIELD_GO;

    public GameManager gameUIMange;
    public bool GameIsStarted;
    public float currentTime;
    public float maxTimeNotMoving = 10f;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.ResetCenterOfMass();
        _rb.ResetInertiaTensor();
        _rb.centerOfMass = centerOfMass.localPosition;
        motorForceInstance = MotorForce;
        gameUIMange = FindObjectOfType<GameManager>();
        Invoke("setGameIsStart", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = getGround(rayCastLineDistanceToGround);
        checkCarNotMoving();

    }
    private void FixedUpdate()
    {
        ClampingPos();
    
            SpeedUpdate();

     
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
        float steer = Input.GetAxis("Horizontal") * steerAngle1;
        //Edit
        SetTheSteeringWheels1(steer * 1);




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
        float speedDiff = CurrentSpeed - MaxSpeed;

        foreach (WheelCollider RSWheels in Rwheels)
        {

            if (CurrentSpeed < MaxSpeed)
            {

                RSWheels.motorTorque = inputKeysPress + 0.001f;
                RSWheels.brakeTorque = 0f;

            }
            else
            {
                float brakeForce = Mathf.Clamp(speedDiff * brakingPower, 0f, Mathf.Infinity);
                RSWheels.brakeTorque = brakeForce;
                float setMotor = MotorForce;
                setMotor *= Mathf.Clamp01(1f - speedDiff / MaxSpeed);
                RSWheels.motorTorque = setMotor;
            }
        }
        foreach (WheelCollider TSWheels in Twheels)
        {

            if (CurrentSpeed < MaxSpeed)
            {
                TSWheels.motorTorque = inputKeysPress + 0.001f;
                TSWheels.brakeTorque = 0f;


            }
            else
            {
                float brakeForce = Mathf.Clamp(speedDiff * brakingPower, 0f, Mathf.Infinity);
                TSWheels.brakeTorque = brakeForce;
                float setMotor = MotorForce;
                setMotor *= Mathf.Clamp01(1f - speedDiff / MaxSpeed);
                TSWheels.motorTorque = setMotor;

            }

        }
    }
    public void SetTheEngine1(float inputKeysPress)
    {

        foreach (WheelCollider RSWheels in Rwheels)
        {

            if (CurrentSpeed < MaxSpeed)
            {
                RSWheels.motorTorque = inputKeysPress + 0.001f;

            }
            else
            {
                RSWheels.motorTorque = 0.0001f;
            }
        }
        foreach (WheelCollider TSWheels in Twheels)
        {

            if (CurrentSpeed > MaxSpeed)
            {
                TSWheels.motorTorque = 0.001f;

            }
            else
            {
                TSWheels.motorTorque = inputKeysPress + 0.0001f;

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
        ClampCarRotation();

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
                        if (transform.position.z <= ClampZCarPosleft - 7f && transform.position.z >= ClampZCarPosRight + 7f)
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
        if(CurrentSpeed > MaxSpeed)
        {
            MotorForce = 0.001f;
            SetTheEngine(MotorForce);

            //CurrentSpeed = MaxSpeed;
        }
        else
        {
            MotorForce = motorForceInstance;
            SetTheEngine(MotorForce);

        }
    }

    public void Jumping(float jumpForce)
    {
        _rb.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);
        _rb.AddForce(Vector3.right * JumpForceRightDirection, ForceMode.Impulse);

    }
    public void SetExhustEmit()
    {
        if(isGrounded)
        {
            foreach(ParticleSystem psEx in psExhust)
            {
                float rate = 20 ;
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

    public void CarIsDestroyed(Vector3 posExplosion)
    {

        MeshRenderer[] getAllRendTemp = GetComponentsInChildren<MeshRenderer>(true);
        List<MeshRenderer> getR = new List<MeshRenderer>();
        for (int i = 0; i < getAllRendTemp.Length; i++)
        {

            if(getAllRendTemp[i].gameObject.tag == "Shield")
            {

            }
            else
            {
                GameObject go = Instantiate(getAllRendTemp[i].transform.gameObject, getAllRendTemp[i].transform.position, getAllRendTemp[i].transform.rotation);
                go.transform.localScale = getAllRendTemp[i].transform.lossyScale;
                getR.Add(go.GetComponent<MeshRenderer>());
            }



        }

        MeshRenderer[] getAllRend = getR.ToArray();




        //
        foreach (MeshRenderer SaperatedMesh in getAllRend)
        {
            GameObject go = SaperatedMesh.transform.gameObject;
            if (go.GetComponent<Rigidbody>() == null)
            {
                go.AddComponent<Rigidbody>();
                go.AddComponent<BoxCollider>();
                AllCollidersRefrence.Add(go.GetComponent<BoxCollider>());
            }
            Destroy(go, 20f);
        }

        //Explosion

        //Destroy(this.gameObject.GetComponent<Rigidbody>());
        //Destroy(this.gameObject.GetComponent<BoxCollider>());

        Vector3 explosionPos = posExplosion;
        power = Random.Range(Minpower, Maxpower);
        uplifter = Random.Range(Minuplifter, Maxuplifter);

        foreach (Collider hit in AllCollidersRefrence)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, uplifter);
            rb.AddRelativeTorque(Vector3.right * power);
        }

        this.transform.gameObject.SetActive(false);
        PlayerISDead = true;
        gameUIMange.LostTheGame();
        enabled = false;

    }


    public void ActiveShield(float duration)
    {
        SHIELD_GO.transform.gameObject.SetActive(true);

        CarIsShield = true;
        Invoke("DeactivateShield", duration);

    }
    public void DeactivateShield()
    {


        SHIELD_GO.transform.gameObject.SetActive(false);
        CarIsShield = false;

    }

    public void checkCarNotMoving()
    {
        if (GameIsStarted && PlayerISDead == false)
        {
            if(CurrentSpeed <= 5)
            {
                currentTime += Time.deltaTime;
                if(currentTime > maxTimeNotMoving)
                {
                    CarIsDestroyed(transform.position);
                    GameIsStarted = false;
                }

            }
            else
            {
                currentTime = 0;

            }
        }
    }

    public void setGameIsStart()
    {
        GameIsStarted = true;
    }
}

