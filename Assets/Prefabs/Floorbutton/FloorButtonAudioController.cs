using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FloorButtonSound
{
    press,
    release,
}

public class FloorButtonAudioController : MonoBehaviour
{
    [SerializeField]
    private AudioClip pressed;

    [SerializeField]
    private AudioClip released;

    private AudioSource audioSource;


    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays a certain lever sound.
    /// </summary>
    /// <param name="doorSound">Enum describing the sound that shall play</param>
    public void PlaySound(FloorButtonSound buttonSound)
    {
        switch (buttonSound)
        {
            case FloorButtonSound.press:
                audioSource.clip = pressed;
                audioSource.Play();
                break;
            case FloorButtonSound.release:
                audioSource.clip = released;
                audioSource.Play();
                break;
        }
    }
}
