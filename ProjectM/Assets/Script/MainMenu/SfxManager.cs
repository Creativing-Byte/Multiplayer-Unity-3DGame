using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class SfxManager : MonoBehaviour
{
    public AudioMixerGroup[] SoundOutput; //0 Master, 1 Music, 2 Sfx
    public AudioClip[] Tracks_Background;
    [HideInInspector]
    public AudioSource Music;
    AudioListener AudioOutput;
    public Scene scene;
    public string sceneCargada;
    public bool active;

    void Start()
    {
        AudioOutput = GetComponent<AudioListener>();

        Music = gameObject.AddComponent<AudioSource>();
        Music.outputAudioMixerGroup = SoundOutput[1];
        //Music.clip = Tracks_Background[0];
        Music.Play();
        Music.loop = true;
    }
    void Update()
    {

        scene=SceneManager.GetActiveScene();
        sceneCargada = scene.name.ToString();

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
        if (active)
        {
            switch (scene.name)
            {
                case "LoadingGame":
                    StartCoroutine("MusicChange");
                    Music.clip = Tracks_Background[0];
                    StartCoroutine("MusicChangeAnd");
                    Music.Play();
                    active = false;
                    break;
                case "Lobby":
                    StartCoroutine("MusicChange");
                    Music.clip = Tracks_Background[0];
                    StartCoroutine("MusicChangeAnd");
                    Music.Play();
                    active = false;
                    break;

                case "World1":
                    StartCoroutine("MusicChange");
                    Music.clip = Tracks_Background[1];
                    StartCoroutine("MusicChangeAnd");
                    Music.Play();
                    active = false;
                    break;
                case "World2":
                    StartCoroutine("MusicChange");
                    Music.clip = Tracks_Background[2];
                    StartCoroutine("MusicChangeAnd");
                    Music.Play();
                    active = false;
                    break;                
                case "World3":
                    StartCoroutine("MusicChange");
                    Music.clip = Tracks_Background[3];
                    StartCoroutine("MusicChangeAnd");
                    Music.Play();
                    active = false;
                    break;
                case "World4":
                    StartCoroutine("MusicChange");
                    Music.clip = Tracks_Background[4];
                    StartCoroutine("MusicChangeAnd");
                    Music.Play();
                    active = false;
                    break;
                case "World5":
                    StartCoroutine("MusicChange");
                    Music.clip = Tracks_Background[5];
                    StartCoroutine("MusicChangeAnd");
                    Music.Play();
                    active = false;
                    break;
            }
        }




    }
     IEnumerator MusicChange()
    {
        Music.volume = 0;
        yield return new WaitForSecondsRealtime(2);
        
    }
    IEnumerator MusicChangeAnd()
    {
        Music.volume = 1;
        yield return new WaitForSecondsRealtime(2);

    }
}
