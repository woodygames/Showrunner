using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingMovement : MovementType
{
    private void Start()
    {
        base.Start();
    }

    /// <summary>
    /// the jump is directed to where the player is walking towards right now
    /// </summary>
    public override void Move(bool isGrounded)
    {
        playerMovement.Jump(Vector3.right * PlayerInput.singleton.horizontal + Vector3.forward * PlayerInput.singleton.vertical, true);
        playerMovement.FreeFall();
    }
}
