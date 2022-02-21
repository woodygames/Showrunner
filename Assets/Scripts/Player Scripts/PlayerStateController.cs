using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateEvent : UnityEvent<MovementState, MovementState> { }

public class PlayerStateController : MonoBehaviour
{
    #region Variables
    // the event that is triggered when the type of movement has changed
    private StateEvent onStateChanged;
    

    [Header("Climbing Related")]

    [Tooltip("The layer used for detecting ladders.")]
    [SerializeField]
    private LayerMask ladderLayer;
    [Tooltip("The maximum distance between the player and the ladder for the player to climb onto it.")]
    [SerializeField]
    private float ladderDistance = 3.2f;

    private Transform ladder;
    /// <summary>
    /// if the player is climbing the ladder, its gameObject is stored here
    /// </summary>
    public Transform Ladder
    {
        get
        {
            return ladder;
        }
    }

    [Header("Dash Related")]

    [Tooltip("The maximum number of available dashes.")]
    [SerializeField]
    private int maxDashes = 3;
    private int dashStock;
    [Tooltip("The time in seconds it takes for the player to gain a new dash.")]
    [SerializeField]
    private float dashRefillTime = 1f;
    private float dashTimer = 0f;

    [Header("Attacking Related")]

    [Tooltip("The time in seconds that an attack should pause the movement")]
    [SerializeField]
    private float attackPause = 0.5f;
    private float pauseTimer = 0f;
    #endregion

    #region Main Methods
    private void Start()
    {
        dashStock = maxDashes;
    }

    private void FixedUpdate()
    {
        // check if dashes were used, refill if so
        if(dashStock < maxDashes)
        {
            dashTimer += Time.fixedDeltaTime;
            if (dashTimer >= dashRefillTime)
            {
                dashTimer = 0f;
                dashStock++;
            }

        }
           
    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// Checks for each movement state if a certain condition is met that would change the type of movement
    /// </summary>
    /// <param name="state">The current type of movement</param>
    /// <param name="isGrounded">Tells if the player is on the ground or not</param>
    /// <returns></returns>
    public MovementState GetState(MovementState state, bool isGrounded)
    {
        MovementState newState = state;
        bool idle = true;
        PlayerInput input = PlayerInput.singleton;
        

        #region case walking
        if (newState == MovementState.walking)
        {
            // moving jump
            if(input.jumping && isGrounded)
            {
                newState = MovementState.jumping;
                idle = false;
            }
            // falling off a slope
            if(!isGrounded)
            {
                newState = MovementState.midAir;
                idle = false;
            }
            // coninuing to walk
            if (input.horizontal != 0f || input.vertical != 0f)
                idle = false;
        }
        #endregion

        #region case jumping
        else if (newState == MovementState.jumping)
        {
            // after performing the jump
            if(!isGrounded)
            {
                newState = MovementState.midAir;
                idle = false;
            }
        }
        #endregion

        #region case midAir
        else if (newState == MovementState.midAir)
        {
            idle = false;
            // re-entering the ground
            if (isGrounded)
            {
                if (input.horizontal != 0f || input.vertical != 0f)
                    newState = MovementState.walking;
                else
                    idle = true;
            }
        }
        #endregion

        #region case climbing
        else if(newState == MovementState.climbing)
        {
            // leaving the ladder
            if(!Physics.CheckSphere(transform.position, 1f, ladderLayer))
            {
                newState = MovementState.midAir;
            }
            // jumping off the ladder
            if(input.jumping)
            {
                newState = MovementState.jumping;
            }

            idle = false;
        }
        #endregion

        #region case dashing
        if (newState == MovementState.dashing)
        {
            // after performing the dash
            newState = MovementState.walking;
            idle = false;
        }
        #endregion

        #region case attacking
        if (newState == MovementState.attacking)
        {
            //idle = false;
            //pauseTimer += Time.deltaTime;
            //// attack has ended
            //if(pauseTimer >= attackPause)
            //{
            //    pauseTimer = 0f;
            //    idle = true;
            //    // player wants to walk
            //    if (input.horizontal != 0f || input.vertical != 0f)
            //    {
            //        newState = MovementState.walking;
            //        idle = false;
            //    }
            //}
            newState = MovementState.walking;
            idle = false;
        }
        #endregion

        #region case idle
        else if (newState == MovementState.idle)
        {
            // player wants to walk
            if(input.horizontal != 0f || input.vertical != 0f)
            {
                newState = MovementState.walking;
                idle = false;
            }
            // ground has left the game
            if(!isGrounded)
            {
                newState = MovementState.midAir;
                idle = false;
            }
            // standing jump
            else if(input.jumping)
            {
                newState = MovementState.jumping;
                idle = false;
            }
        }
        #endregion

        #region case always
        // player can and wants to perform dash
        if (input.stopCrouch && dashStock > 0)
        {
            newState = MovementState.dashing;
            dashStock--;
            idle = false;
        }
        
        if (input.attacking)
        {
            RaycastHit hit = Camera.main.GetComponent<CameraController>().GetCursorHit();
            // use a ladder
            if (hit.transform?.gameObject.layer == Mathf.Log(ladderLayer.value, 2f) &&          // Did the user click on a ladder?
                Vector3.Distance(transform.position, hit.point) <= ladderDistance &&            // Is the player near enough to climb onto the ladder?
                Vector3.Angle(hit.transform.forward, transform.position - hit.point) > 90f)     // Is the player facing the back side of the ladder?
            {
                newState = MovementState.climbing;
                ladder = hit.transform;
                GetComponent<ClimbingMovement>()?.Prepare(Ladder);
            }

            idle = false;
        }

        if(input.use)
        {
            RaycastHit hit = Camera.main.GetComponent<CameraController>().GetCursorHit();

            if (!GetComponent<CombatController>().currentWeapon.GetIsRanged())
            {
                GetComponent<AttackMovement>()?.Prepare(hit.point);
                // perform an attack
                newState = MovementState.attacking;

                idle = false;
            }

        }

        // if no other condition is met, then player is idle
        if (idle)
            newState = MovementState.idle;
        #endregion

        if(newState != state)
        {
            onStateChanged.Invoke(state, newState);
        }
        
        return newState;
    }

    /// <summary>
    /// adds a listener to the event system that sends messages when the type of movement has changed
    /// </summary>
    public void AddListener(UnityAction<MovementState, MovementState> action)
    {
        if(onStateChanged == null) onStateChanged = new StateEvent();
        onStateChanged.AddListener(action);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 0f, 0.5f);
        Gizmos.DrawSphere(transform.position, 1f);
    }
}
