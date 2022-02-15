using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    [SerializeField]
    private bool triggered = false;



    
    public override void Trigger()
    {
        float distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position);
        if (distance > range) return;
        triggered = !triggered;

        Animator animator = GetComponent<Animator>();
        animator.SetBool("triggered", triggered);

        gameObject.GetComponent<AudioSource>().Play();
        
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
