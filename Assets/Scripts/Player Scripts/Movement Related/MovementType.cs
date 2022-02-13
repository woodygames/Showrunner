using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementType : MonoBehaviour
{
    public MovementState changeTo;

    protected PlayerInput input;
    protected PlayerMovement playerMovement;
    protected PlayerMovementController movementController;

    protected void Start()
    {
        input = PlayerInput.singleton;
        playerMovement = GetComponent<PlayerMovement>();
        movementController = GetComponent<PlayerMovementController>();
        movementController.AddMovementType(this);
    }

    /// <summary>
    /// Moves the player in a specific way, depending on the type of movement
    /// </summary>
    public virtual void Move(bool isGrounded)
    {
        // Movement code
    }
}
