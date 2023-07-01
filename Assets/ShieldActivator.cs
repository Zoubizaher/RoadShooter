using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShieldActivator : MonoBehaviour
{
    public float durationShield = 30f;

    public TextMeshProUGUI TXT_Shield;
    private void Start()
    {
        TXT_Shield.text = "SHIELD TIME   + " + durationShield;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "Player")
        {

            VehicleController VC = other.gameObject.GetComponent<VehicleController>();
            VC.ActiveShield(durationShield);
            Destroy(gameObject);
        }
    }
}
