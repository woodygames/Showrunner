using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButtonScript : Interactable
{
    private bool pressed = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Trigger()
    {

    }

    public override bool GetPass()
    {
        return pressed;
    }

    public override bool OutlineIsRed()
    {
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!pressed)
        {
            GetComponent<FloorButtonAudioController>().PlaySound(FloorButtonSound.press);
            GetComponent<ParticleSystem>().Play();
        }
        pressed = true;
        NotifyAllObservers();
    }
    void OnCollisionStay(Collision collision)
    {
        pressed = true;
        NotifyAllObservers();
    }

    void OnCollisionExit(Collision collision)
    {
        pressed = false;
        GetComponent<FloorButtonAudioController>().PlaySound(FloorButtonSound.release);
        NotifyAllObservers();
    }
}
