using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public Transform target;
    public float rotationDamp;
    public float updateValue;
    public float movementSpeed = 10f;

    void Start()
    {
        
    }

    void Update()
    {
        Turn();
        Move();
    }

    void Turn()
    {
        Vector3 pos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, updateValue * rotationDamp);
    }

    void Move()
    {
        transform.position += transform.forward * updateValue * movementSpeed;
    }

}
