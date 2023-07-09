using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterFromSky : MonoBehaviour
{

    public float radius = 5;
    public LayerMask layer;

    public GameObject BulletFromSky;
    public float bulletSpeed = 10f;
    public GameObject playerObj;

    public float FireRATIO = 2f;
    public float currentTime;

    public float maxFireRATIO = 0.3f;
    public float minFireRATIO = 3f;

    public bool BallNonDestructive = false;
    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, layer);

        if (hitColliders != null)
        {
            foreach (Collider hitCollider in hitColliders)
            {
                playerObj = hitCollider.gameObject;

            }
            if(playerObj != null)
            {
                //transform.LookAt(playerObj.transform.position);
                currentTime += Time.deltaTime;
                if(FireRATIO < currentTime)
                {
                    Shoot();
                    FireRATIO = Random.Range(minFireRATIO, maxFireRATIO);
                    currentTime = 0;
                }
            }
        }
    }


    void Shoot()
    {
        Vector3 aimDirection = (playerObj.transform.position - transform.position).normalized;
        GameObject bullet = Instantiate(BulletFromSky, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = aimDirection * bulletSpeed;
        bullet.GetComponent<BulletShooter>().isDestroyWhenTouchThePlayer = BallNonDestructive;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
