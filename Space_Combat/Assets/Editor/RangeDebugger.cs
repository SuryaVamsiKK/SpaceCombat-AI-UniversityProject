using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDebugger : MonoBehaviour
{
    public Color debugColor = Color.red;
    public Color debugColorDetection = Color.yellow;
    public Color debugDetectionColor = Color.cyan;

    private void OnDrawGizmos()
    {
        Movement hook = GetComponent<Movement>();

        if (hook.type == EType.AI)
        {
            UnityEditor.Handles.color = debugColor;
            UnityEditor.Handles.DrawWireDisc(this.transform.position, transform.up, GetComponent<Movement>().range);
            UnityEditor.Handles.color = debugColorDetection;
            UnityEditor.Handles.DrawWireDisc(this.transform.position, transform.up, GetComponent<Movement>().rangeDetection);

            Gizmos.color = debugDetectionColor;
            Vector3 origin = hook.dectorsOffset;
            origin = transform.forward + transform.TransformPoint(origin);

            for (int i = 0; i < hook.dectors.Length; i++)
            {
                Vector3 dir = hook.dectors[i] - transform.position;
                Ray ray = hook.pathFindingType == EpathFindingType.Conical ? new Ray(origin, dir) : new Ray(hook.dectors[i], transform.forward);
                Gizmos.DrawRay(ray.origin, ray.direction * hook.objectDetectionRange);
                Gizmos.DrawSphere(ray.GetPoint(hook.objectDetectionRange), 0.15f);
            }
        }
    }
}
