using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingMovement : MovementType
{
    [Tooltip("Specifies how far the player is away from the ladder.")]
    [SerializeField]
    private float climbingOffset = 0.8f;
    [SerializeField]
    private float speedMultiplier = 0.5f;

    private GameObject ladder;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Move(bool isGrounded)
    {
        Vector3 move = transform.right * input.horizontal + transform.up * input.vertical;

        playerMovement.Move(move * speedMultiplier, 0);
    }

    public void Prepare(GameObject ladder)
    {
        this.ladder = ladder;

        Vector3 climbingPosition = ladder.transform.position - ladder.transform.forward * climbingOffset;

        playerMovement.MoveInstantaniously(climbingPosition - transform.position);
    }
}
