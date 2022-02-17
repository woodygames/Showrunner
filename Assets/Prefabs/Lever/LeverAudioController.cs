using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LeverSound
{
    deny,
    trigger,
}

public class LeverAudioController : MonoBehaviour
{
    [SerializeField]
    private AudioClip denySound;

    [SerializeField]
    private AudioClip triggerSound;

    private AudioSource audioSource;


    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays a certain lever sound.
    /// </summary>
    /// <param name="doorSound">Enum describing the sound that shall play</param>
    public void PlaySound(LeverSound leverSound)
    {
        switch (leverSound)
        {
            case LeverSound.deny:
                audioSource.clip = denySound;
                audioSource.Play();
                break;
            case LeverSound.trigger:
                audioSource.clip = triggerSound;
                audioSource.Play();
                break;
        }
    }
}
