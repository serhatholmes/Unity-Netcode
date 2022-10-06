using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;
    void Start()
    {
        offset = transform.position - target.position;
    }

    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(transform.position.x,offset.y+target.position.y, offset.z+target.position.z);
        transform.position = Vector3.Lerp(transform.position, newPos, Time.fixedDeltaTime*10);
    }
}
