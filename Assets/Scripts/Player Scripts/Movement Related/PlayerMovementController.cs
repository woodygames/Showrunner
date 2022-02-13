using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float checkRadius = 0.4f;
    [SerializeField]
    private LayerMask groundLayer;

    private bool isGrounded;
    private List<MovementType> movements;
    private PlayerStateController stateController;
    private MovementState state;

    #region Main Methods
    private void Awake()
    {
        movements = new List<MovementType>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isGrounded = true;
        stateController = GetComponent<PlayerStateController>();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckForGround();
        ChooseMovement();
    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// Iterates through all types of movement until the one with the correct movement state is found, then executes its movement code
    /// </summary>
    private void ChooseMovement()
    {
        state = stateController.GetState(state, isGrounded);

        foreach (MovementType movement in movements)
        {
            if (state == movement.changeTo)
            {
                movement.Move(isGrounded);
                break;
            }
        }
    }

    /// <summary>
    /// Adds a movement type to the list so that its movement can be executed when needed
    /// </summary>
    /// <param name="type"> The type of movement to be added </param>
    public void AddMovementType(MovementType type)
    {
        movements.Add(type);
    }

    /// <summary>
    /// continually checks if the player is colliding with the ground
    /// </summary>
    public void CheckForGround()
    {
        if (!groundCheck)
            return;

        isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, groundLayer);
    }
    #endregion

    #region Gizmo Methods
    private void OnDrawGizmos()
    {
        // Draw sphere that shows ground collision
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawSphere(groundCheck.position, checkRadius);
    }
    #endregion
}
