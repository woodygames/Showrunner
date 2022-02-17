using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoor : Interactable, Observer
{
    /// <summary>
    /// List of interactables the door observes
    /// </summary>
    [SerializeField]
    private Interactable[] interactables;

    /// <summary>
    /// door is unlocked when all interactables return true for GetPass()
    /// </summary>
    private bool unlocked = false;
    private bool opened = false;
    
    void Start()
    {
        //Set door as observer in interactables
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
        return opened;
    }

    /// <summary>
    /// Trigger opens the door
    /// </summary>
    public override void Trigger()
    {
        //canceling when player is too far away or door is already open
        float distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position);
        if (distance > range)
        {
            gameObject.GetComponent<DoorAudioController>().PlaySound(DoorSound.deny);
            return;
        }
        if (opened) return;
        
        Debug.Log("door wurde unlocked: " + unlocked);
        if (unlocked)
        {
            OpenDoor();
        }
        else
        {
            //playing deny sound if door isn´t yet unlocked
            gameObject.GetComponent<DoorAudioController>().PlaySound(DoorSound.deny);
        }
    }

    /// <summary>
    /// called from observed interactables,
    /// unlocks door when all interactables return true for GetPass()
    /// </summary>
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

    /// <summary>
    /// Starting open door animation
    /// </summary>
    private void OpenDoor()
    {
        if (opened) return;
         opened = true;
        Animator animator = GetComponent<Animator>();
        animator.SetBool("closed", false);

        
        Debug.Log("slide sounds");
        //Destroy(this.gameObject);
    }
}
