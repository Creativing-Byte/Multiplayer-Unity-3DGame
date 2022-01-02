using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SfxManager : MonoBehaviour
{
    public AudioMixerGroup[] SoundOutput; //0 Master, 1 Music, 2 Sfx
    public AudioClip[] Tracks_Background;
    [HideInInspector]
    public AudioSource Music;
    AudioListener AudioOutput;
    void Start()
    {
        AudioOutput = GetComponent<AudioListener>();

        Music = gameObject.AddComponent<AudioSource>();
        Music.outputAudioMixerGroup = SoundOutput[1];
        Music.clip = Tracks_Background[0];
        Music.Play();
        Music.loop = true;
    }
    void Update()
    {
        if (AudioOutput.enabled == false)
        {
            AudioOutput.enabled = true;
        }

        if (PlayerPrefs.GetInt("Music",1) == 0)
        {
            SoundOutput[1].audioMixer.SetFloat("VolumeMusic", -80f);
        }
        else
        {
            SoundOutput[1].audioMixer.SetFloat("VolumeMusic", 0f);
        }
        if (PlayerPrefs.GetInt("Sfx", 1) == 0)
        {
            SoundOutput[2].audioMixer.SetFloat("VolumeSfx", -80f);
        }
        else
        {
            SoundOutput[2].audioMixer.SetFloat("VolumeSfx", 0f);
        }
    }
}
