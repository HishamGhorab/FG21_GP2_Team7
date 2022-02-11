using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundComponent : MonoBehaviour
{
    public List<SoundEffect> Sounds;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip) => source.PlayOneShot(clip);
    public void PlaySound(string name)
    {
        foreach (SoundEffect s in Sounds)
        {
            if(s.name == name)
            {
                source.PlayOneShot(s.clip);
            }
        }
    }
    public void PlayAudioSound(string name)
    {
        foreach (SoundEffect s in Sounds)
        {
            if (s.name == name)
            {
                source.clip = s.clip;
                source.Play();
            }
        }
    }
    public void StopAudioSource() => source.Stop();
    public void SetAudioSourceVolume(float volume) => source.volume = volume;
    public void RandomizeAudioSourcePitch(float min, float max) => source.pitch = Random.Range(min, max);
    public void RandomizeAudioSourcePitch(float delta) => source.pitch = Random.Range(source.pitch - delta, source.pitch + delta);


    [System.Serializable]
    public struct SoundEffect
    {
        public string name;
        public AudioClip clip;

        public SoundEffect(string n, AudioClip c)
        {
            name = n;
            clip = c;
        }
    }
}
