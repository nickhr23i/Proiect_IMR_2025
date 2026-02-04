using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip clipBiting;
    public AudioClip clipEscaped;
    public AudioClip clipCaught;
    public float volume = 1f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false; // Don't play automatically
    }

    public void PlayClip(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning("No AudioClip assigned to PlaySound on " + gameObject.name);
        }
    }

}
