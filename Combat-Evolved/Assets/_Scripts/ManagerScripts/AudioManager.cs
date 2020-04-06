using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public List<AudioClip> clips;
    // parallel array, associates AudioClip to an AudioSource
    private AudioSource[] sources;
    // audio fade duration
    public float fadeDuration = .75f;

    private void Start()
    {
        instance = this;
        clips = new List<AudioClip>();
        // Get all clips in Audio/Resources
        int index = 0;
        // DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Audio/Resources");
        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath + "/Audio/Resources");
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            if (f.Name.EndsWith("wav") || f.Name.EndsWith("mp3"))
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

    // gets an audio clip index by name
    private int GetAudioClipIndex(string name)
    {
        for (int i = 0; i < clips.Count; i++)
        {
            if (clips[i].name.Equals(name))
            {
                return i;
            }
        }
        return -1;
    }

    // plays an audio clip by name
    // specify playFrom if want to create audio source on separate object
    public void PlaySound(string name, GameObject playFrom = null, bool fadeIn = false, float volume = 1f, bool looping = false)
    {
        gameObject.name = "Manager";
        if(name.Equals(""))
        {
            Debug.Log("NO SOUND EQUIPPED!");
            return;
        }
        int i = GetAudioClipIndex(name);
        Debug.Assert(i != -1, "AudioManager:PlaySound:: AudioSource Manager has no sound: " + name + "! Check the Audio/Resources folder!!");
        AudioSource audioSource = null;
        // determine where to play sound from
        // play sound from the manager
        if (playFrom == null)
        {
            audioSource = sources[i];
        }
        // play sound on another object
        else
        {
            // create audio source
            audioSource = playFrom.AddComponent<AudioSource>();
            // attach clip to audioSource
            audioSource.clip = clips[i];
            // destroy audiosource after clip ends
            Destroy(audioSource, clips[i].length);
        }

        // plays the sound
        audioSource.loop = looping;
        audioSource.volume = volume;
        if(!audioSource.isPlaying)
            audioSource.Play();

        // if want to fade in
        if (fadeIn)
            StartCoroutine(FadeIn(audioSource, fadeDuration));
    }

    public void StopSound(string name, GameObject stopFrom = null, bool fadeOut = false)
    {
        AudioSource audioSource = null;
        // if stopping sound from manager
        if (stopFrom == null)
        {
            int i = GetAudioClipIndex(name);
            Debug.Assert(i == -1, "AudioManager:StopSound:: AudioSource Manager has no sound: " + name + "! Check the Audio/Resources folder!!");
            audioSource = sources[i];
        }
        else
        {
            AudioSource[] audioSources = stopFrom.GetComponents<AudioSource>();
            Debug.Assert(audioSources.Length >= 1, "AudioManager:StopSound::" + stopFrom.name + " has no AudioSources!");
            bool sourceFound = false;
            // find the correct audio source associated with the clip
            for (int i = 0; i < audioSources.Length; i++)
            {
                // if found correct source
                if (audioSources[i].clip.name.Equals(name))
                {
                    // if duplicate source
                    if (sourceFound)
                        Destroy(audioSources[i]);
                    // set source
                    else
                    {
                        audioSource = audioSources[i];
                        sourceFound = true;
                    }
                }
            }
            Debug.Assert(audioSource != null, "AudioManager:StopSound:: No AudioSource with " + name + " found!");
            Destroy(audioSource, fadeDuration * 2);
        }

        if(fadeOut)
            StartCoroutine(FadeOut(audioSource, fadeDuration));
        else
            audioSource.Stop();
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        audioSource.volume = 0;
        while (audioSource.volume < 1)
        {
            audioSource.volume += 1 * Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.volume = 1;
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = 1;
    }
}
