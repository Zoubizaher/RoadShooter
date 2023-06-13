using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoneArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            VehicleController VC = other.gameObject.GetComponent<VehicleController>();
            if (VC.CarIsShield)
            {
                   
            }
            else
            {
                VC.CarIsDestroyed(VC.transform.position);


            }
        }
    }
}
