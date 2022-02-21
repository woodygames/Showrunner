using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMovement : MovementType
{
    [Tooltip("The force that pushes the player forward during the dash.")]
    [SerializeField]
    private float dashForce = 5f;
    [Tooltip("Determines how long the player dashes.")]
    [SerializeField]
    private float dashDuration = 0.3f;

    private Vector3 cursorPosition;

    private void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Performs a dash in the same direction the player is moving in
    /// </summary>
    public override void Move(bool isGrounded)
    {
        Vector3 direction = (cursorPosition - transform.position).normalized;
        playerMovement.AddForce(direction * dashForce, dashDuration);
    }

    public void Prepare(Vector3 cursor)
    {
        cursorPosition = cursor;
    }
}
