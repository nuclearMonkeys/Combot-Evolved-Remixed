using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip[] clips;
    AudioSource source;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(string name)
    {
        foreach (AudioClip ac in clips)
        {
            if (ac.name.Equals(name))
            {
                source.clip = ac;
                source.Play();
            }
        }
    }
}
