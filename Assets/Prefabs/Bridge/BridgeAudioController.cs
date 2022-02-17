using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BridgeSound
{
    deny,
    extended,
}

public class BridgeAudioController : MonoBehaviour
{
    [SerializeField]
    private AudioClip denySound;

    [SerializeField]
    private AudioClip extendSound;

    private AudioSource audioSource;


    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays a certain bridge sound.
    /// </summary>
    /// <param name="bridgeSound">Enum describing the sound that shall play</param>
    public void PlaySound(BridgeSound bridgeSound)
    {
        switch (bridgeSound)
        {
            case BridgeSound.deny:
                audioSource.clip = denySound;
                audioSource.Play();
                break;
            case BridgeSound.extended:
                audioSource.clip = extendSound;
                audioSource.Play();
                break;
        }
    }
}
