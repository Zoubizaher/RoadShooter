using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public float dmg = 50f;
    public float bulletForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-transform.right * bulletForce,Space.Self);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.gameObject.tag == "Wall")
        {
            WallDetect wd = other.transform.gameObject.GetComponent<WallDetect>();
            Debug.Log("DetectWall");
            wd.DamageWall(dmg);
            Destroy(gameObject);

        }
        else if (other.transform.gameObject.tag == "Shield")
        {
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
