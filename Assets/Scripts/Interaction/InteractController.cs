using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = Camera.main.GetComponent<CameraController>().GetCursorHit();
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();

            interactable?.Trigger();
        }
     
    }

}
