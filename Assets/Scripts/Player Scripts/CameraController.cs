using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// The vertical distance of the camera to the target
    /// </summary>
    [SerializeField]
    private float height = 12f;
    /// <summary>
    /// The horizontal distance of the camera to the target
    /// </summary>
    [SerializeField]
    private float distance = 8f;

    [SerializeField]
    private float angle = 45f;
    /// <summary>
    /// The angle the camera is looking at to the target
    /// </summary>
    public float Angle
    {
        get
        {
            return angle;
        }
    }

    /// <summary>
    /// The target to be looked at
    /// </summary>
    [SerializeField]
    private Transform target;

    /// <summary>
    /// defines how fast the interpolation is
    /// </summary>
    [SerializeField]
    private float smoothTime = 0.5f;

    private Vector3 refVelocity;


    // Start is called before the first frame update
    void Start()
    {
        HandleCamera();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleCamera();
    }

    /// <summary>
    /// Sets the top-down camera at a specified angle and smoothes the interpolation while the player is moving.
    /// </summary>
    public void HandleCamera()
    {
        if(!target)
        {
            return;
        }

        Vector3 worldPosition = Vector3.forward * -distance + Vector3.up * height;

        Vector3 rotatedPosition = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;

        Vector3 finalPosition = rotatedPosition + target.position;

        Vector3 flatTargetPosition = target.position;
        flatTargetPosition.y = 0f;

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothTime);

        transform.LookAt(target.position);
    }

    /// <summary>
    /// Shoots a ray from the camera to the cursor's position and returns the hit info
    /// </summary>
    /// <returns>The Raycast hit info</returns>
    public RaycastHit GetCursorHit()
    {
        // todo: check whether the cursor is on an UI element

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        return hit;
    }
}
