using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetect : MonoBehaviour
{
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
    public float health;
    public float maxHealth = 100;

    [SerializeField] List<Collider> AllCollidersRefrence;
    private void Start()
    {
        health = maxHealth;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            VehicleController VC = other.gameObject.GetComponent<VehicleController>(); 
            if(VC.CarIsShield)
            {
                WallDestruction();
            }
            else
            {
                VC.CarIsDestroyed(VC.transform.position);

             
            }
        }
    }


    public void WallDestruction()
    {
        Destroyed1(transform.position);
    }

    public void DamageWall(float dmg)
    {
        health -= dmg;
        if(health < 0)
        {
            WallDestruction();
        }
    }
    public void Destroyed1(Vector3 posExplosion)
    {

        MeshRenderer[] getAllRendTemp = GetComponentsInChildren<MeshRenderer>(true);
        List<MeshRenderer> getR = new List<MeshRenderer>();
        for (int i = 0; i < getAllRendTemp.Length; i++)
        {

          
                GameObject go = Instantiate(getAllRendTemp[i].transform.gameObject, getAllRendTemp[i].transform.position, getAllRendTemp[i].transform.rotation);
                getR.Add(go.GetComponent<MeshRenderer>());
            

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
        //Destroy(this.gameObject, 10f);
    }
}


