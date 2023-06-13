using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFiring : MonoBehaviour
{

    public Transform p_Fire;
    public GameObject Bullet;
    public float FireR = 0.5f;
    public float currentTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.F) && FireR < currentTime)
        {
            Fire();
            currentTime = 0;
        }
    }
    public void Fire()
    {
        GameObject bulletIns = Instantiate(Bullet, p_Fire.transform.position, Quaternion.identity);
        Vector3 direction = p_Fire.transform.forward;
        bulletIns.transform.forward = direction;
    }
}
