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
    /// the jump is directed to where the player is walking towards right now, then sets the player movement state to midAir
    /// </summary>
    public override void Move(bool isGrounded)
    {
        playerMovement.Jump(transform.right * PlayerInput.singleton.horizontal + transform.forward * PlayerInput.singleton.vertical, true);
        playerMovement.FreeFall();
    }
}