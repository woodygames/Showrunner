using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    [SerializeField]
    private bool triggered = false;



    
    public override void Trigger()
    {
       
        triggered = !triggered;
        Renderer renderer = this.gameObject.GetComponent<Renderer>();

        if (triggered)
        {
            renderer.material.SetColor("_Color", Color.green);
        }
        else
        {
            renderer.material.SetColor("_Color", Color.red);
        }
        
        NotifyAllObservers();
        Debug.Log(triggered);

    }

    public override bool GetPass()
    {
        return triggered;
    }

    void Update()
    {
        
    }
}
