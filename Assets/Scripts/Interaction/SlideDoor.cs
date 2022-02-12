using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoor : Interactable, Observer
{
    [SerializeField]
    private Interactable[] interactables;

    private bool unlocked = false;
    
    void Start()
    {
        foreach(Interactable interactable in interactables)
        {
            interactable.SetObserver(this);
        }
    }

    void Update()
    {
    }

    public override bool GetPass()
    {
        return true;
    }

    public override void Trigger()
    {
        Debug.Log("door wurde unlocked: " + unlocked);
        if (unlocked)
        {
            OpenDoor();
        }
    }

    public void Notify()
    {
        bool canOpen = true;
        foreach(Interactable interactable in interactables)
        {
            if (!interactable.GetPass())
            {
                canOpen = false;
            }
        }

        unlocked = canOpen;
    }


    private void OpenDoor()
    {
        Renderer renderer = this.gameObject.GetComponent<Renderer>();

        renderer.material.SetColor("_Color", Color.green);
        Debug.Log("slide sounds");
        //Destroy(this.gameObject);
    }
}
