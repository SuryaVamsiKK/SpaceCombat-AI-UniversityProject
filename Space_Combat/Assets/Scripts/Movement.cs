using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool player = true;

    [ConditionalHide("player", 1)] public float maxThrust = 3f;
    [ConditionalHide("player", 1)] public float maxRetroThrust = 3f;
    [Space]
    public Transform thrustObject;

    [Range(-3, 3)] public float thrust = 1;
    [Range(-3, 3)] public float retroThrustYaw = 1;
    [Range(-3, 3)] public float retroThrustPitch = 1;
    [Range(-3, 3)] public float retroThrustRoll = 1;
    [Range (0, 1)] public float updateValue = 0.1f;

    [Header ("Color Settings")]
    public Color flameColorForward;
    public Color flameColorBackward;

    private void Update()
    {
        if (player)
        {
            thrusterBasedMovement(Input.GetAxis("Thrust") / 10);
            retorThrusterBasedMovement(Input.GetAxis("Yaw") / 10, Input.GetAxis("Pitch") / 10, Input.GetAxis("Roll") / 10);
        }
        else
        {
            thrusterBasedMovement(AI_Thrust());
        }
    }

    public void thrusterBasedMovement(float inputThrust)
    {
        if(!player)
        {
            maxThrust = 3f;
            thrust = 1f;
        }
        
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
            thrustObject.GetComponent<MeshRenderer>().sharedMaterial.color = flameColorForward;
        }
        else
        {
            thrustObject.GetComponent<MeshRenderer>().sharedMaterial.color = flameColorBackward;
        }

        thrustObject.localScale = new Vector3(Mathf.Abs((1.75f * thrust) / maxThrust), thrustObject.localScale.y, Mathf.Abs((1.75f * thrust) / maxThrust));
        transform.position += transform.forward * thrust * updateValue;
    }

    public void retorThrusterBasedMovement(float inputRetroThrustYaw, float inputRetroThrustPitch, float inputRetroThrustRoll)
    {
        if (!player)
        {
            maxRetroThrust = 3f;
            retroThrustYaw = 1f;
            retroThrustPitch = 1f;
            retroThrustRoll = 1f;
        }

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

    public float AI_Thrust()
    {
        return 1f;
    }
}
