using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private PlayerInput input;
	private CameraController camController;

	[Header("Movement")]
	
	[SerializeField]
	float gravity = -9.81f;
	/// <summary>
	/// speed of the player while walking
	/// </summary>
	[SerializeField]
	float walkSpeed = 8f;
	/// <summary>
	/// speed of the player while running
	/// </summary>
	[SerializeField]
	float runSpeed = 16f;
	/// <summary>
	/// additional force pulling the player down while on a slope
	/// </summary>
	[SerializeField]
	private float slopeForce = 5;
	/// <summary>
	/// Distance indicating the maximum steepness of a slope
	/// </summary>
	[SerializeField]
	private float slopeForceRayLength = 2;
	/// <summary>
	/// height the player can reach while jumping
	/// </summary>
	[SerializeField]
	float jumpHeight = 3f;
	
	private CharacterController controller;

	private Transform moveTransform;
	
	Vector3 velocity;
	public Vector3 Velocity
	{
		get
        {
			return velocity;
        }
	}
	
	float speed;

	void Start()
	{
		input = PlayerInput.singleton;
		camController = Camera.main.GetComponent<CameraController>();
		controller = GetComponent<CharacterController>();

		// the direction the player should move relative to is dependent on the angle the camera is looking at the player
		moveTransform = transform;
		moveTransform.Rotate(Vector3.up, camController.Angle);
	}

    private void Update()
    {
        
    }

    /// <summary>
    /// sets the script's attributes since it has to be added to the player prefab as a component dynamically
    /// </summary>
    public void SetAttributes(float g, float w, float r, float c, float s, float sLength, float j, CharacterController con, Transform cT, Transform gT)
	{
		gravity = g;
		walkSpeed = w;
		runSpeed = r;
		slopeForce = s;
		slopeForceRayLength = sLength;
		jumpHeight = j;
		controller = con;
	}
	
	public void StandStill()
    {
		velocity.y = 0f;
    }
	
	/// <summary>
	/// handles 2-dimensional movement, running, gravity application and slopes
	/// </summary>
	public void Move(bool isGrounded)
	{
		
		float lastSpeed = speed;
		
		speed = (input.running) ? runSpeed : walkSpeed;
		
		if(!isGrounded) speed = lastSpeed;

		Vector3 move = moveTransform.right * input.horizontal + moveTransform.forward * input.vertical;
		
		controller.Move(move * speed * Time.deltaTime);
		
		if(velocity.x != 0f || velocity.z != 0f)
		{
			velocity.x = 0f;
			velocity.z = 0f;
		}

		velocity.y = 0f;//-2f;
		
		controller.Move(velocity * Time.deltaTime);
		
		if(input.horizontal != 0 && input.vertical != 0 && OnSlope())
			controller.Move(Vector3.down * controller.height/2 * slopeForce * Time.deltaTime);
	}
	
	
	/// <summary>
	/// used to move in mid-air, just 2-dimensional movement
	/// </summary>
	public void SimpleMove()
	{
		Vector3 move = moveTransform.right * input.horizontal + moveTransform.forward * input.vertical;
		
		controller.Move(move * speed/2f * Time.deltaTime);

		if (input.horizontal != 0 && input.vertical != 0 && OnSlope())
			controller.Move(Vector3.down * controller.height / 2 * slopeForce * Time.deltaTime);
	}
	
	/// <summary>
	/// method for wallrun movement, or just 3-dimensional movement if the second parameter is 0
	/// </summary>
	public void Move(Vector3 direction, float appliedGravity)
	{
		if(appliedGravity > 0) direction.y += gravity * Time.deltaTime * appliedGravity;
		controller.Move(direction * runSpeed * Time.deltaTime);
	}
	
	bool OnSlope()
	{
		if(Physics.Raycast(transform.position, Vector3.down, out var hit, controller.height/2 * slopeForceRayLength))
			if(hit.normal != Vector3.up)
				return true;
		return false;
	}
	
	
	/// <summary>
	/// applies gravity
	/// </summary>
	public void FreeFall()
	{
		
		velocity.y += gravity * Time.deltaTime;
		
		controller.Move(velocity * Time.deltaTime);
		
	}
	
	/// <summary>
	/// method for simple jump
	/// </summary>
	public void Jump()
	{
		velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
	}
	
    /// <summary>
    /// jump method combined with a Vector3, telling in which direction to jump
    /// </summary>
	public void Jump(Vector3 direction, bool applySpeed = false)
	{
		velocity = direction;
		
		if (applySpeed) velocity *= speed;
		
		velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
		
		controller.Move(velocity * Time.deltaTime);
	}

	/// <summary>
	/// Simulates the Rigidbody.AddForce() method by continually moving the player using a coroutine. The force is gradually decreased.
	/// </summary>
	/// <param name="force"> The force that pushes the player forward </param>
	/// <param name="duration"> The duration of the movement </param>
	public void AddForce(Vector3 force, float duration)
	{
		controller.Move(force * Time.deltaTime);
		float timer = 0f;
		StartCoroutine(KeepApplyingForce(force,timer,duration));
	}
	
	IEnumerator KeepApplyingForce(Vector3 force, float timer, float duration) 
	{
		
		Vector3 applicableForce = force;
		
		while(timer < duration)
		{
			timer += Time.deltaTime;
			
			if(timer > duration) timer = duration;
			
			if(applicableForce.x != 0)applicableForce.x = QuadraticBrakeFormular(timer,duration,force.x);
			if(applicableForce.y != 0)applicableForce.y = QuadraticBrakeFormular(timer,duration,force.y);
			if(applicableForce.z != 0)applicableForce.z = QuadraticBrakeFormular(timer,duration,force.z);
			
			controller.Move(applicableForce * Time.deltaTime);
			
			yield return null;
		}
		
		StopCoroutine(KeepApplyingForce(force, timer, duration));
		
	}
	
	private float QuadraticBrakeFormular(float x, float t, float f){
			
		x = x*x;
		t = t*t;
		return x - t + f;
	
	}

	public float GetSpeed()
    {
		return speed;
    }
}