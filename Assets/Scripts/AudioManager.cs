using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound[] grunts;

    void Awake ()
    {
        foreach (Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        foreach (Sound s in grunts){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string category)
{
    Sound[] array = category == "grunt" ? grunts : sounds;

    if (array == null || array.Length == 0) return;

    Sound s = array[UnityEngine.Random.Range(0, array.Length)];
    s.source.Play();
}

}
