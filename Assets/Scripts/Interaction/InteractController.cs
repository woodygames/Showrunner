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
            //if an gameObject with an interactable is returned the trigger method of the interactable is called
            RaycastHit hit = Camera.main.GetComponent<CameraController>().GetCursorHit();
            Interactable interactable = hit.collider?.gameObject.GetComponent<Interactable>();

            interactable?.Trigger();
        }
     
    }

}
