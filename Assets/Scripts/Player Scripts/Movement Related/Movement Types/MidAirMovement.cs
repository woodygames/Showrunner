using UnityEngine;

[DisallowMultipleComponent]
public class MidAirMovement : MovementType
{
	
	/// <summary>
	/// applies gravity to the player and lets them move 2-dimensionally, but only to a certain extent
	/// </summary>
	public override void Move(bool isGrounded)
	{
		playerMovement.SimpleMove();
		playerMovement.FreeFall();
	}
	
}