using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public EType type;
    [ConditionalHideEnum("type", 1)] public Transform target;
    [ConditionalHideEnum("type", 1)] public float range;
    [ConditionalHideEnum("type", 1)] public Color debugColor = Color.red;
    [Header("Thrust Values")]
    public float maxThrust = 3f;
    public float maxRetroThrust = 3f;

    [Space]

    public bool enableValues = false;
    [ConditionalHide("enableValues")] public float thrust = 1;
    [ConditionalHide("enableValues")] public float retroThrustYaw = 1;
    [ConditionalHide("enableValues")] public float retroThrustPitch = 1;
    [ConditionalHide("enableValues")] public float retroThrustRoll = 1;
    [ConditionalHide("enableValues")] public float updateValue = 0.1f;

    [Space]
    public Transform thrustObject;
    [Space]

    [Header ("Color Settings")]
    public Color flameColorForward;
    public Color flameColorBackward;

    private void Start()
    {
        if (type == EType.Player)
        {
            CusrosHide();
        }
    }

    private void Update()
    {
        if (type == EType.Player)
        {
            thrusterBasedMovement(Input.GetAxis("Thrust") / 10);
            retorThrusterBasedMovement(Input.GetAxis("Yaw") / 10, - Input.GetAxis("Pitch") / 10, -Input.GetAxis("Roll") / 10);
        }
        else
        {
            if ((transform.position - target.position).sqrMagnitude > range * range)
            {
                thrusterBasedMovement(AI_Thrust());
            }
            AI_RetroThrust();
        }

        if (type == EType.Player)
        {
            if (Input.GetKeyDown(KeyCode.X))
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
        thrust += inputThrust;

        if (thrust > maxThrust)
        {
            thrust = maxThrust;
        }
        if (thrust < -maxThrust)
        {
            thrust = -maxThrust;
        }

        if(thrust >= 0)
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
        return maxThrust;
    }

    public void AI_RetroThrust()
    {
        Vector3 pos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, updateValue * maxRetroThrust);
    }

    #endregion

    #region For Player

    public void KillEngines()
    {
        thrust = 0;
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
        if(type == EType.Enemy)
        {
            Gizmos.color = debugColor;
            Gizmos.DrawWireSphere(this.transform.position, range);
        }
    }

}

public enum EType
{
    Player, Enemy
}
