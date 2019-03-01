using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public EType type;
    [Header("Targeting")]
    [ConditionalHideEnum("type", 1)] public Transform target;
    [ConditionalHideEnum("type", 1)] public float range;
    [ConditionalHideEnum("type", 1)] public float rangeDetection;
    [Header("Path Finding")]
    [ConditionalHideEnum("type", 1)] public float objectDetectionRadius;
    [ConditionalHideEnum("type", 1)] public float objectDetectionThreshold;
    [ConditionalHideEnum("type", 1)] public float objectDetectionRange;
    [ConditionalHideEnum("type", 1)] public int accuracy;
    [ConditionalHideEnum("type", 1)] public Vector3 dectorsOffset;
    [ConditionalHideEnum("type", 1)] public LayerMask layerMask;
    [HideInInspector] public Vector3[] dectors;

    [Header("Thrust Values")]
    public float maxThrust = 3f;
    public float maxRetroThrust = 3f;

    [HideInInspector] public bool enableValues = false;
    [Space]
    [ConditionalHide("enableValues")] public float thrust = 1;
    [ConditionalHide("enableValues")] public float retroThrustYaw = 1;
    [ConditionalHide("enableValues")] public float retroThrustPitch = 1;
    [ConditionalHide("enableValues")] public float retroThrustRoll = 1;
    [ConditionalHide("enableValues")] public float updateValue = 0.1f;

    [Space]
    public Transform thrustObject;
    [Space]

    [Header("Color Settings")]
    public Color flameColorForward;
    public Color flameColorBackward;

    private void OnValidate()
    {
        if (type == EType.AI)
        {
            if(accuracy % 2 != 0)
            {
                accuracy++;
            }
            detectorsSetup();
            //PathFinding();
        }
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        if (type == EType.Player)
        {
            CusrosHide();
        }
    }

    private void Update()
    {
        if (type == EType.AI)
        {
            if ((transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).sqrMagnitude < rangeDetection * rangeDetection)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
            else
            {
                target = null;
            }
            detectorsSetup();
        }

        if (type == EType.Player)
        {
            thrusterBasedMovement(Input.GetAxis("Thrust") / 10);
            retorThrusterBasedMovement(Input.GetAxis("Yaw") / 10, Input.GetAxis("Pitch") / 10, -Input.GetAxis("Roll") / 10);
        }
        else
        {
            thrusterBasedMovement(AI_Thrust());
            PathFinding();
            //AI_RetroThrust();
        }

        if (type == EType.Player)
        {
            if (Input.GetKey(KeyCode.X))
            {
                KillEngines();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                RetroThrustresKill();
                //ResetPitchYawRoll();
            }
        }
    }

    public void thrusterBasedMovement(float inputThrust)
    {
        #region clamping rawThrust with max thrust

        thrust += inputThrust;

        if (thrust > maxThrust)
        {
            thrust = maxThrust;
        }
        if (thrust < -maxThrust)
        {
            thrust = -maxThrust;
        }

        #endregion

        if (thrust >= 0)
        {
            thrustObject.GetComponent<MeshRenderer>().material.color = flameColorForward;
        }
        else
        {
            thrustObject.GetComponent<MeshRenderer>().material.color = flameColorBackward;
        }

        thrustObject.localScale = new Vector3(Mathf.Abs((1.75f * thrust) / maxThrust), thrustObject.localScale.y, Mathf.Abs((1.75f * thrust) / maxThrust));
        transform.position += transform.forward * thrust * updateValue;
    }

    #region For AI

    public float AI_Thrust()
    {
        if (target != null)
        {
            if ((transform.position - target.position).sqrMagnitude > range * range)
            {
                return accelerate();
            }
            else
            {
                return decelarate();
            }
        }
        else
        {
            return decelarate();
        }
    }

    public void AI_RetroThrust()
    {
        if (target != null)
        {
            Vector3 pos = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(pos);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, updateValue);
        }
    }

    void detectorsSetup()
    {
        dectors = new Vector3[accuracy];

        float divisionAngle = 360/dectors.Length;
        float angle = 0;

        for (int i = 0; i < dectors.Length; i++)
        {
            dectors[i] = new Vector3();
            dectors[i].x = objectDetectionRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
            dectors[i].y = objectDetectionRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            dectors[i].z = objectDetectionThreshold;

            dectors[i] = transform.forward + transform.TransformPoint(dectors[i]);

            #region Debugging the dectors it self when needed uncomment
            //Vector3 dir = detectionAccuracy[i] - transform.position;
            //Debug.DrawRay(raycastOffset, dir, Color.cyan);
            #endregion

            angle += divisionAngle;
        }
    }

    void PathFinding()
    {
        RaycastHit[] hit;

        Vector3 mean = Vector3.zero;

        if(type == EType.AI)
        {
            Vector3 origin = dectorsOffset;
            origin = transform.forward + transform.TransformPoint(origin);

            for (int i = 0; i < dectors.Length; i++)
            {
                Vector3 dir = dectors[i] - transform.position;
                Ray ray = new Ray(origin, dir);

                hit = Physics.RaycastAll(ray, objectDetectionRange, layerMask);

                for (int j = 0; j < hit.Length; j++)
                {
                    if (hit[j].transform.root.gameObject != this.gameObject)
                    {
                        int opposite = i + dectors.Length / 2;

                        if (opposite >= dectors.Length)
                        {
                            opposite -= dectors.Length;
                        }


                        mean = dectors[opposite];
                        transform.Rotate(mean * updateValue * 2);
                    }
                }

                if (hit.Length == 0)
                {
                    AI_RetroThrust();
                }

            }

           

        }
    }

    public float accelerate()
    {
        if (thrust < maxThrust)
        {
            return updateValue;
        }
        else
        {
            return maxThrust;
        }
    }

    public float decelarate()
    {
        if (thrust > 0)
        {
            return -updateValue;
        }
        else if (thrust < 0)
        {
            return updateValue;
        }
        else
        {
            return 0;
        }
    }

    #endregion

    #region For Player

    public void KillEngines()
    {
        if (thrust > 0)
        {
            thrust -= updateValue;
        }
        else if (thrust < 0)
        {
            thrust += updateValue;
        }
        else
        {
            thrust = 0;
        }

    }

    public void RetroThrustresKill()
    {
        retroThrustYaw = 0;
        retroThrustRoll = 0;
        retroThrustPitch = 0;
    }

    public void ResetPitchYawRoll()
    {
        transform.rotation = Quaternion.identity;
    }

    public void retorThrusterBasedMovement(float inputRetroThrustYaw, float inputRetroThrustPitch, float inputRetroThrustRoll)
    {
        retroThrustYaw += inputRetroThrustYaw;
        retroThrustPitch += inputRetroThrustPitch;
        retroThrustRoll += inputRetroThrustRoll;

        #region Retro Thruster Yaw Clamp
        if (retroThrustYaw > maxRetroThrust)
        {
            retroThrustYaw = maxRetroThrust;
        }
        if (retroThrustYaw < -maxRetroThrust)
        {
            retroThrustYaw = -maxRetroThrust;
        }
        #endregion

        #region Retro Thruster Roll Clamp
        if (retroThrustRoll > maxRetroThrust)
        {
            retroThrustRoll = maxRetroThrust;
        }
        if (retroThrustRoll < -maxRetroThrust)
        {
            retroThrustRoll = -maxRetroThrust;
        }
        #endregion

        #region Retro Thruster Pitch Clamp
        if (retroThrustPitch > maxRetroThrust)
        {
            retroThrustPitch = maxRetroThrust;
        }
        if (retroThrustPitch < -maxRetroThrust)
        {
            retroThrustPitch = -maxRetroThrust;
        }
        #endregion

        float yaw = retroThrustYaw * updateValue;
        float pitch = retroThrustPitch * updateValue;
        float roll = retroThrustRoll * updateValue;

        transform.Rotate(pitch, yaw, roll);
    }

    public void CusrosHide()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (type == EType.AI)
        {
            detectorsSetup();
            //PathFinding();
        }
    }
}

public enum EType
{
    Player, AI
}
