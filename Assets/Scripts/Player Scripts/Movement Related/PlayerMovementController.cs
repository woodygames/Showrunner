using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    #region Variables
    [Tooltip("A gameObject on the bottom of the player's body, used for checking ground collision.")]
    [SerializeField]
    private Transform groundCheck;
    [Tooltip("Ground detection is done spherically with the groundCheck's position as the center and this variable as the sphere's radius.")]
    [SerializeField]
    private float checkRadius = 0.4f;
    [Tooltip("The layer used for detecting ground.")]
    [SerializeField]
    private LayerMask groundLayer;

    // Is the player on the ground?
    private bool isGrounded;
    // A list containing all different types of movement
    private List<MovementType> movements;
    // A controller used for calculating the type of movement to be executed
    private PlayerStateController stateController;
    // The state of the current movement type
    private MovementState state;
    #endregion

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
        LookAtCursor();
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

    /// <summary>
    /// Accesses the cursor's position inside the scene and looks towards it
    /// </summary>
    public void LookAtCursor()
    {
        RaycastHit hit = Camera.main.GetComponent<CameraController>().GetCursorHit();
        if (hit.point == null)
            return;

        Vector3 heightenedHit = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        transform.LookAt(heightenedHit);
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

    #region Getters and Setters
    /// <returns>The current movement type of the player</returns>
    public MovementState GetState()
    {
        return state;
    }

    /// <returns>The rotation of the player</returns>
    public Quaternion GetRotation()
    {
        return transform.rotation;
    }
    #endregion
}
