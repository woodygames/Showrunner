using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingMovement : MovementType
{
    [Tooltip("Specifies how far the player is away from the ladder.")]
    [SerializeField]
    private float climbingOffset = 0.8f;
    [Tooltip("Climbing speed should be slower than walking speed; this float specifies the fraction")]
    [SerializeField]
    private float speedFraction = 0.5f;
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
    /// Moves the player two-dimensionally, but on the x-y-plane
    /// </summary>
    /// <param name="isGrounded">Indicates if the player is on the ground</param>
    public override void Move(bool isGrounded)
    {
        Vector3 move = transform.right * input.horizontal + transform.up * input.vertical;

        playerMovement.Move(move * speedFraction, 0);
    }

    /// <summary>
    /// Teleports the player to a position in front of the ladder
    /// </summary>
    /// <param name="ladder"></param>
    public void Prepare(Transform ladder)
    { 
        Vector3 climbingPosition = ladder.position - ladder.forward * climbingOffset;

        playerMovement.MoveInstantaniously(climbingPosition);
    }
}
