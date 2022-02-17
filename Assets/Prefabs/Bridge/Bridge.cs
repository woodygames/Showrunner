using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : Interactable, Observer
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
    private bool extended = false;

    /// <summary>
    /// the unit of lenght is tiles
    /// </summary>
    [SerializeField]
    private int length;

    [SerializeField]
    private GameObject bridgeCopy;


    void Start()
    {
        //Set door as observer in interactables
        foreach (Interactable interactable in interactables)
        {
            interactable.SetObserver(this);
        }
        if (interactables.Length == 0)
        {
            unlocked = true;
        }
    }

    public override void Trigger()
    {
        float distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position);
        if (distance > range)
        {
            gameObject.GetComponent<BridgeAudioController>().PlaySound(BridgeSound.deny);
            return;
        }
        if (extended) return;

        if (unlocked)
        {
            ExtendContinue();
        }
        else
        {
            
            gameObject.GetComponent<BridgeAudioController>().PlaySound(BridgeSound.deny);
        }
    }

    public override bool GetPass()
    {
        return extended;
    }

    /// <summary>
    /// called from observed interactables,
    /// unlocks door when all interactables return true for GetPass()
    /// </summary>
    public void Notify()
    {
        bool canOpen = true;
        foreach (Interactable interactable in interactables)
        {
            if (!interactable.GetPass())
            {
                canOpen = false;
            }
        }
        unlocked = canOpen;
    }

    public void ExtendBegin(int len)
    {
        extended = true;
       
        Animator animator = GetComponent<Animator>();
        animator.SetBool("extended", true);
        length = len;
    }

    public void ExtendContinue()
    {
        if (length == 0)
            return;
        extended = true;
       
        GameObject subBridge = Instantiate(bridgeCopy, gameObject.transform.position,
          bridgeCopy.transform.rotation);
        Bridge subBridgeScript = subBridge.GetComponentInChildren<Bridge>();
        subBridgeScript.SetBridgeCopy(subBridge);
        subBridgeScript.ExtendBegin(length - 1);
    }

    public void SetBridgeCopy(GameObject copy)
    {
        bridgeCopy = copy;
    }
}
