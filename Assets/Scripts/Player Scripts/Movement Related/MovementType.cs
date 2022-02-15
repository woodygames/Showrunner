using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementType : MonoBehaviour
{
    [Tooltip("The movement state that corresponds to this type of movement")]
    public MovementState changeTo;
    // user input
    protected PlayerInput input;
    // class providing methods for moving the player
    protected PlayerMovement playerMovement;
    // controller handling everything movement related
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
