using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound[] grunts;
    public Sound[] screams;

    void Awake ()
    {
        foreach (Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach (Sound s in grunts){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach (Sound s in screams){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

    }
    
    void Start(){
        
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning($"Sound not found: {name}");
            return;
        }

        if (s.source == null)
        {
            Debug.LogWarning($"AudioSource missing for: {name}");
            return;
        }

        s.source.Play();
        Debug.Log("Playing" + name);
        }

    public void PlayGrunt()
    {
        Sound s = grunts[UnityEngine.Random.Range(0, grunts.Length)];

        if (s.source != null)
        {
            s.source.Play();
        }
    }

    public void PlayScream()
    {
        Sound s = screams[UnityEngine.Random.Range(0, screams.Length)];

        if (s.source != null)
        {
            s.source.Play();
        }
    }
}
