using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.5f;
    public Vector3 offset;
    public float setYPosCam = 45f;
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(90, 90, 0);
      offset = new Vector3(150, 600.7f, 0f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            FollowPlayer();

        }
    }


    public void FollowPlayer()
    {
        Vector3 DefinedPosTargetSpecificAxis = new Vector3(target.position.x, target.position.y, offset.z);
        Vector3 desiredPosition = DefinedPosTargetSpecificAxis + offset;

        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }

}