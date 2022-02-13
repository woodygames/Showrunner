using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingMovement : MovementType
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Moves the player two-dimensionally
    /// </summary>
    public override void Move(bool isGrounded)
    {
        playerMovement.Move(isGrounded);
    }
}
