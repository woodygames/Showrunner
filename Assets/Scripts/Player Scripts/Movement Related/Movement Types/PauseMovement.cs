using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMovement : MovementType
{
    private void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Blocks new Movement, continues ongoing movement
    /// </summary>
    public override void Move(bool isGrounded)
    {
        playerMovement.FreeFall();
    }
}
