using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelVisualSteer : MonoBehaviour
{
    public bool cancelSteerAngle = false;
    public GameObject wheelModel;
    private WheelCollider _wheelCollider;

    public Vector3 localRotOffset;

    private float lastUpdate;

    void Start()
    {
        lastUpdate = Time.realtimeSinceStartup;

        _wheelCollider = GetComponent<WheelCollider>();
    }

    void FixedUpdate()
    {
        // We don't really need to do this update every time, keep it at a maximum of 60FPS
        if (Time.realtimeSinceStartup - lastUpdate < 1f / 60f)
        {
            return;
        }
        lastUpdate = Time.realtimeSinceStartup;

        if (wheelModel && _wheelCollider)
        {
            Vector3 pos = new Vector3(0, 0, 0);
            Quaternion quat = new Quaternion();
            _wheelCollider.GetWorldPose(out pos, out quat);

            wheelModel.transform.rotation = quat;
            if (cancelSteerAngle)
                wheelModel.transform.rotation = transform.parent.rotation;

            wheelModel.transform.localRotation *= Quaternion.Euler(localRotOffset);
            wheelModel.transform.position = pos;

            WheelHit wheelHit;
            _wheelCollider.GetGroundHit(out wheelHit);
        }
    }
    }
