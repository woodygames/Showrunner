using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorSound
{
    deny,
    open,
}

public class DoorAudioController : MonoBehaviour
{
    [SerializeField]
    private AudioClip denySound;

    [SerializeField]
    private AudioClip openSound;

    private AudioSource audioSource;


    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
   
    public void PlaySound(DoorSound doorSound)
    {
        switch (doorSound)
        {
            case DoorSound.deny:
                audioSource.clip = denySound;
                audioSource.Play();
                break;
            case DoorSound.open:
                audioSource.clip = openSound;
                audioSource.Play();
                break;
        }
    }
}
