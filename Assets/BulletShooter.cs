using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
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

    [SerializeField] List<Collider> AllCollidersRefrence;

    private void Start()
    {
        Destroy(gameObject, 20f);
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.gameObject.tag == "Player")
        {

            VehicleController VC = collision.gameObject.GetComponent<VehicleController>();
            if (VC.CarIsShield)
            {

                DestroyBullet(transform.position);

            }

            else
            {

                VC.CarIsDestroyed(VC.transform.position);
                DestroyBullet(transform.position);

            }
        }
        else if (collision.transform.gameObject.tag == "Shield")
        {
            return;
        }
        else
        {

            DestroyBullet(transform.position);

        }
    }
    public void DestroyBullet(Vector3 posExplosion)
    {

        Destroy(this.gameObject);
    }
}
