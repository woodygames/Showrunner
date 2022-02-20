using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightController : MonoBehaviour
{

    private Vector3 mousePosition;
    private GameObject lookAt;

    [SerializeField]
    private Material material;

    [SerializeField]
    private Color colorAccepted;
    [SerializeField]
    private Color colorDenied;

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
                
                if(lookAt.GetComponent<Outline>() != null) lookAt.GetComponent<Outline>().SetOutline(false);
                lookAt = newLookAt;
                if (lookAt.GetComponent<Outline>() != null) lookAt.GetComponent<Outline>().SetOutline(true);
                
            }
            else
            {
                if (lookAt.GetComponent<Outline>() != null) lookAt.GetComponent<Outline>().SetOutline(true);
            }
        }
    }

    public void SetOutlineRed(bool b)
    {
        if (b)
        {
            material.SetColor("_Color", colorDenied);
        }
        else
        {
            material.SetColor("_Color", colorAccepted);
        }
    }
}
