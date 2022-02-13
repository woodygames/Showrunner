using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    [SerializeField]
    private LayerMask ladderLayer;
    [SerializeField]
    private float ladderDistance = 3.2f;

    [SerializeField]
    private int maxDashes = 3;
    private int dashStock;
    [SerializeField]
    private float dashRefillTime = 1f;
    private float dashTimer = 0f;

    private void Start()
    {
        dashStock = maxDashes;
    }

    private void FixedUpdate()
    {
        
        if(dashStock < maxDashes)
        {
            dashTimer += Time.fixedDeltaTime;
            if (dashTimer >= dashRefillTime)
            {
                dashTimer = 0f;
                dashStock++;
                print(dashStock);
            }

        }
           
    }

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
        RaycastHit hit = Camera.main.GetComponent<CameraController>().GetCursorHit();

        bool ladderClicked = false;

        #region case walking
        if (newState == MovementState.walking)
        {
            if(input.jumping && isGrounded)
            {
                newState = MovementState.jumping;
                idle = false;
            }

            if(ladderClicked)
            {
                newState = MovementState.climbing;
                idle = false;
            }

            if(!isGrounded)
            {
                newState = MovementState.midAir;
                idle = false;
            }

            if (input.horizontal != 0f || input.vertical != 0f)
                idle = false;
        }
        #endregion

        #region case jumping
        else if (newState == MovementState.jumping)
        {
            if(!isGrounded)
            {
                newState = MovementState.midAir;
                idle = false;
            }

            if(ladderClicked)
            {
                newState = MovementState.climbing;
                idle = false;
            }
        }
        #endregion

        #region case midAir
        else if (newState == MovementState.midAir)
        {
            idle = false;

            if (isGrounded)
            {
                if (input.horizontal != 0f || input.vertical != 0f)
                    newState = MovementState.walking;
                else
                    idle = true;
            }

            if(ladderClicked)
            {
                newState = MovementState.climbing;
            }

        }
        #endregion

        #region case climbing
        else if(newState == MovementState.climbing)
        {
            if(!Physics.CheckSphere(transform.position, 1f, ladderLayer))
            {
                newState = MovementState.walking;
            }
            idle = false;
        }

        if(input.attacking && 
            Vector3.Distance(transform.position, hit.point) <= ladderDistance && 
            hit.transform?.gameObject.layer == Mathf.Log(ladderLayer.value, 2f) &&
            Vector3.Angle(hit.transform.forward, transform.position - hit.point) > 90f)
        {
            newState = MovementState.climbing;
            idle = false;
        }
        #endregion

        #region case dashing
        if (newState == MovementState.dashing)
        {
            newState = MovementState.walking;
        }

        if (input.stopCrouch && dashStock > 0)
        {
            newState = MovementState.dashing;
            dashStock--;
            idle = false;
        }
        #endregion

        #region case idle
        else if (newState == MovementState.idle)
        {
            if(input.horizontal != 0f || input.vertical != 0f)
            {
                newState = MovementState.walking;
                idle = false;
            }

            if(ladderClicked)
            {
                newState = MovementState.climbing;
                idle = false;
            }

            if(!isGrounded)
            {
                newState = MovementState.midAir;
                idle = false;
            }
            else if(input.jumping)
            {
                newState = MovementState.jumping;
                idle = false;
            }
        }
        #endregion

        if (idle)
            newState = MovementState.idle;

        if(newState != state)
        {
            if(newState == MovementState.climbing)
            {
                GetComponent<ClimbingMovement>()?.Prepare(hit.transform.gameObject);
            }
        }
        
        return newState;
    }
}
