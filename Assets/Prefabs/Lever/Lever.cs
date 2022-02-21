using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    /// <summary>
    /// State of the lever
    /// </summary>
    [SerializeField]
    private bool triggered = false;

    /// <summary>
    /// Triggers the lever
    /// </summary>
    public override void Trigger()
    {
        float distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position);
        //cancels when player is too far away
        if (distance > range)
        {
            gameObject.GetComponent<LeverAudioController>().PlaySound(LeverSound.deny);
            return;
        }

        triggered = !triggered;

        Animator animator = GetComponent<Animator>();
        animator.SetBool("triggered", triggered);

        gameObject.GetComponent<LeverAudioController>().PlaySound(LeverSound.trigger);
        
        NotifyAllObservers();
        Debug.Log(triggered);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>lever is triggered</returns>
    public override bool GetPass()
    {
        return triggered;
    }

    void Update()
    {
        
    }

    public override bool OutlineIsRed()
    {
        float distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position);
        return (distance > range);
    }
}
