using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGO : MonoBehaviour
{
    public GameManager GameManage;
    private void Start()
    {
        GameManage = FindObjectOfType<GameManager>();

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "Player")
        {

            VehicleController VC = other.gameObject.GetComponent<VehicleController>();
            VC.GameIsStarted = false;
            VC.enabled = false;
            GameManage.WonTheGame();
        }
    }
}
