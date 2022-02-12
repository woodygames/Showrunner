using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightController : MonoBehaviour
{

    private Vector3 mousePosition;
    private GameObject lookAt;

    void Start()
    {
        mousePosition = Input.mousePosition;
        lookAt = null;
    }

    void Update()
    {
        Vector3 newMousePos = Input.mousePosition;
        if(mousePosition != newMousePos)
        {
            mousePosition = newMousePos;
            RaycastHit hit = Camera.main.GetComponent<CameraController>().GetCursorHit();

            GameObject newLookAt = hit.collider?.gameObject;
            if(newLookAt == null)
            {
                return;
            }
            if(lookAt == null)
            {
                lookAt = newLookAt;
            }


            if (!newLookAt.Equals(lookAt))
            {
                Debug.Log("Showing Outline");
                if(lookAt.GetComponent<Outline>() != null) lookAt.GetComponent<Outline>().enabled = false;
                lookAt = newLookAt;
                if (lookAt.GetComponent<Outline>() != null) lookAt.GetComponent<Outline>().enabled = true;
                
            }
            else
            {
                if (lookAt.GetComponent<Outline>() != null) lookAt.GetComponent<Outline>().enabled = true;
            }
        }
    }
}
