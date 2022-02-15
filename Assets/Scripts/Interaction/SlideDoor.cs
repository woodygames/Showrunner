using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoor : Interactable, Observer
{
    [SerializeField]
    private Interactable[] interactables;

   

    private bool unlocked = false;
    private bool opened = false;
    
    void Start()
    {
        foreach(Interactable interactable in interactables)
        {
            interactable.SetObserver(this);
        }
        if(interactables.Length == 0)
        {
            unlocked = true;
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
        float distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position);
        if (distance > range) return;
        if (opened) return;
        
        Debug.Log("door wurde unlocked: " + unlocked);
        if (unlocked)
        {
            OpenDoor();
        }
        else
        {
            gameObject.GetComponent<DoorAudioController>().PlaySound(DoorSound.deny);
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
        if (opened) return;
         opened = true;
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.enabled = false;
        Animator animator = GetComponent<Animator>();
        animator.SetBool("closed", false);

        
        Debug.Log("slide sounds");
        //Destroy(this.gameObject);
    }
}
