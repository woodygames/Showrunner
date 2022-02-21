using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    #region Variables
    [Tooltip("The character model")]
    [SerializeField]
    private Transform character;
    [Tooltip("The animator component of the character model")]
    [SerializeField]
    private Animator animator;
    private PlayerStateController stateController;
    private PlayerInput input;
    // the old direction the character was moving towards
    private Vector3 oldDirection = Vector3.zero;
    // the current type of movement
    private MovementState playerState;
    #endregion

    #region Main Methods
    // Start is called before the first frame update
    void Start()
    {
        input = PlayerInput.singleton;
        stateController = GetComponent<PlayerStateController>();
        stateController.AddListener(ChangeAnimation);
    }

    // Update is called once per frame
    void Update()
    {
        SetCharacterTransform();  

        if ((int)playerState == 0)
        {
            SetWalkAnimation();
        }
    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// Sets the position and rotation of the model accordingly to the current type of movement 
    /// </summary>
    private void SetCharacterTransform()
    {
        if ((int)playerState != 3)
        {
            // due to inconsistencies in the playback of the animations, position and rotation have to be reset every frame
            character.position = transform.position + Vector3.down;
            character.rotation = transform.rotation;
        }
        else
        {
            // the character model is not influenced by rotation when climbing
            Transform ladder = stateController.Ladder;
            if (ladder)
            {
                character.rotation = ladder.rotation;

                float x = ladder.position.x - ladder.forward.x * 0.4f;
                float y = transform.position.y + Vector3.down.y*2;
                float z = ladder.position.z - ladder.forward.z * 0.4f;
                character.position = new Vector3(x, y, z);
            }
        }
    }

    /// <summary>
    /// Maps the move direction to a blend tree and differentiates between walking and running
    /// </summary>
    private void SetWalkAnimation()
    {
        #region rectangle draw


        //Vector3 pos1 = transform.position + Vector3.left + Vector3.forward;
        //Vector3 pos2 = transform.position + Vector3.right + Vector3.forward;
        //Vector3 pos3 = transform.position + Vector3.right + Vector3.back;
        //Vector3 pos4 = transform.position + Vector3.left + Vector3.back;

        //Debug.DrawLine(pos1, pos2, Color.red);
        //Debug.DrawLine(pos2, pos3, Color.blue);
        //Debug.DrawLine(pos3, pos4, Color.green);
        //Debug.DrawLine(pos4, pos1, Color.yellow);
        #endregion

        MapMoveDirectionToBlendTree();
        animator.SetBool("IsRunning", input.running);

        if (animator.GetFloat("HorizontalDirection") == float.NaN || animator.GetFloat("VerticalDirection") == float.NaN)
        {
            animator.SetFloat("HorizontalDirection", 0f);
            animator.SetFloat("VerticalDirection", 0f);
        }
        
    }

    /// <summary>
    /// * GENERAL CONCEPT: The animation played while walking is dependent on two parameters, the horizontal direction ( how much the 
    /// 
    /// * player is moving to the sides ) and the vertical direction(how much the player is moving back / forth ). These depend on
    /// 
    /// * the player's rotation ( which depends on the cursor's position ) and on the direction he is moving in. Firstly, the rotation
    /// 
    /// * of the player is displayed by a normalized vector using the cursor's position. This vector is then rotated by a certain
    /// 
    /// * angle, depending on the direction the player is moving in. Afterwards, the vectors which always points to the edge of a
    ///  
    /// * circle is mapped onto a square, because for blending diagonal movement, the absolute values of the parameters have to be 1.
    /// </summary>
    public void MapMoveDirectionToBlendTree()
    {

        // calculate the vector between player and cursor and map it onto a unit circle
        RaycastHit hit = Camera.main.GetComponent<CameraController>().GetCursorHit();
        Vector3 heightenedPoint = hit.point;
        heightenedPoint.y = transform.position.y;
        Vector3 distanceNorm = (heightenedPoint - transform.position).normalized;

        // this section determines the angle to which the distanceNorm vector should be rotated
        float angle = 0f;
        if (input.horizontal != 0 && input.vertical != 0)
        {
            int sign = input.horizontal > 0 ? -1 : 1;
            float absAngle = input.vertical > 0 ? 45f : 135f;
            angle = sign * absAngle;
        }
        else
        {
            if (input.horizontal != 0)
                angle = input.horizontal > 0 ? -90f : 90f;
            else if (input.vertical != 0)
                angle = input.vertical > 0 ? 0f : 180f;
        }

        Vector3 rotatedDistance = Quaternion.Euler(0, angle, 0) * distanceNorm;

        // map circle coordinates to square coordinates
        float u = rotatedDistance.x;
        float v = rotatedDistance.z;
        float x = 0.5f * Mathf.Sqrt(2 + u * u - v * v + 2 * u * Mathf.Sqrt(2)) - 0.5f * Mathf.Sqrt(2 + u * u - v * v - 2 * u * Mathf.Sqrt(2));
        float z = 0.5f * Mathf.Sqrt(2 - u * u + v * v + 2 * v * Mathf.Sqrt(2)) - 0.5f * Mathf.Sqrt(2 - u * u + v * v - 2 * v * Mathf.Sqrt(2));
        Vector3 distanceOnSquare = new Vector3(x, rotatedDistance.y, z);

        // mirror the vector on the x axis (symmetry reasons)
        distanceOnSquare.x *= -1;

        // to achieve smooth blends transitions when changing direction, the new animation is gradually set
        float horizontal = Mathf.Lerp(animator.GetFloat("HorizontalDirection"), distanceOnSquare.x, 4 * Time.deltaTime);
        float vertical = Mathf.Lerp(animator.GetFloat("VerticalDirection"), distanceOnSquare.z, 4 * Time.deltaTime);

        // sometimes the calculations result in NaN, since I don't know yet when and why, the case is just excluded
        if (horizontal != float.NaN)
            animator.SetFloat("HorizontalDirection", horizontal);
        if (vertical != float.NaN)
            animator.SetFloat("VerticalDirection", vertical);
    }
    #endregion

    #region Event Methods
    public void ChangeAnimation(MovementState oldState, MovementState newState)
    {
        playerState = newState;
        animator.SetInteger("MovementState", (int)newState);
    }
    #endregion

    #region Gizmo Methods
    private void OnDrawGizmos()
    {
        RaycastHit hit = Camera.main.GetComponent<CameraController>().GetCursorHit();
        Vector3 heightenedPoint = hit.point;
        heightenedPoint.y = transform.position.y;
        Vector3 distanceNorm = (heightenedPoint - transform.position).normalized;

        float angle = 0f;
        if (input.horizontal != 0 && input.vertical != 0)
        {
            int sign = input.horizontal > 0 ? -1 : 1;
            float absAngle = input.vertical > 0 ? 45f : 135f;

            angle = sign * absAngle;
        }
        else
        {
            if (input.horizontal != 0)
                angle = input.horizontal > 0 ? -90f : 90f;
            else if (input.vertical != 0)
                angle = input.vertical > 0 ? 0f : 180f;
        }

        Vector3 rotatedDistance = Quaternion.Euler(0, angle, 0) * distanceNorm;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, rotatedDistance);

        // map circle coordinates to square coordinates
        float u = rotatedDistance.x;
        float v = rotatedDistance.z;
        float x = 0.5f * Mathf.Sqrt(2 + u*u - v*v + 2*u * Mathf.Sqrt(2)) - 0.5f * Mathf.Sqrt(2 + u*u - v*v - 2*u * Mathf.Sqrt(2));
        float z = 0.5f * Mathf.Sqrt(2 - u*u + v*v + 2*v * Mathf.Sqrt(2)) - 0.5f * Mathf.Sqrt(2 - u*u + v*v - 2*v * Mathf.Sqrt(2));
        Vector3 distanceOnSquare = new Vector3(x, rotatedDistance.y, z);

        distanceOnSquare.x *= -1;

        Vector3 displayedVector = Vector3.RotateTowards(oldDirection, distanceOnSquare, Time.deltaTime, 1f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, displayedVector);

        oldDirection = distanceOnSquare;
    }
    #endregion
}
