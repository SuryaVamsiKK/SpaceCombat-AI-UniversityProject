using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 defaultDistance = new Vector3(0,2.5f, -10f);
    public float distanceDamp = 10f;
    public float rotationDamp = 10f;
    public float updateValue = 0.01f;

    void LateUpdate()
    {
        Vector3 toPos = target.position + (target.rotation * defaultDistance);
        Vector3 curPos = Vector3.Lerp(transform.position, toPos, distanceDamp * updateValue);
        transform.position = curPos;

        Quaternion toRot = Quaternion.LookRotation(target.position - transform.position, target.up);
        Quaternion curRot = Quaternion.Slerp(transform.rotation, toRot, rotationDamp * updateValue);
        transform.rotation = curRot;
    }
}
