using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public List<AudioClip> clips;
    private AudioSource[] sources;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        clips = new List<AudioClip>();
        // Get all clips in Audio/Resources
        int index = 0;
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Audio/Resources");
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            if(f.Name.EndsWith("wav"))
            {
                clips.Add(Resources.Load<AudioClip>(Path.GetFileNameWithoutExtension(f.Name)));
                index++;
            }
        }

        sources = new AudioSource[clips.Count];
        // creates an audio source for each clip
        for (int i = 0; i < clips.Count; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = clips[i];
            sources[i] = audioSource;
        }
    }

    public void PlaySound(string name, GameObject playFrom=null)
    {
        for (int i = 0; i < clips.Count; i++)
        {
            if (clips[i].name.Equals(name))
            {
                // play sound from the manager
                if(playFrom == null)
                    sources[i].Play();
                // play sound on another object
                else
                {
                    // create audio source
                    AudioSource audioSource = playFrom.AddComponent<AudioSource>();
                    // attach clip to audioSource
                    audioSource.clip = clips[i];
                    // play the clip
                    audioSource.Play();
                    // destroy audiosource after clip ends
                    Destroy(audioSource, clips[i].length);
                }
            }
        }
    }
}
