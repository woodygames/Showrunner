using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
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

        if(newState == MovementState.walking)
        {
            if(input.jumping && isGrounded)
            {
                newState = MovementState.jumping;
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

        if(newState == MovementState.jumping)
        {
            if(!isGrounded)
            {
                newState = MovementState.midAir;
                idle = false;
            }
        }

        if(newState == MovementState.midAir)
        {
            idle = false;

            if (isGrounded)
            {
                if (input.horizontal != 0f || input.vertical != 0f)
                    newState = MovementState.walking;
                else
                    idle = true;
            }

        }

        if(newState == MovementState.idle)
        {
            if(input.horizontal != 0f || input.vertical != 0f)
            {
                newState = MovementState.walking;
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

        if (idle)
            newState = MovementState.idle;

        if(input.jumping) print(input.jumping);

        return newState;
    }
}
