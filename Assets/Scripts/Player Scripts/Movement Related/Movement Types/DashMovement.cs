using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMovement : MovementType
{
    [SerializeField]
    private float dashForce = 5f;
    [SerializeField]
    private float dashDuration = 0.3f;

    private void Start()
    {
        base.Start();
    }

    /// <summary>
    /// the jump is directed to where the player is walking towards right now
    /// </summary>
    public override void Move(bool isGrounded)
    {
        Vector3 direction = transform.right * input.horizontal + transform.forward * input.vertical;
        playerMovement.AddForce(direction * dashForce, dashDuration);
    }
}
