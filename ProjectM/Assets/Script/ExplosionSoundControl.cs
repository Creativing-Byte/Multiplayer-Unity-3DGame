using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSoundControl : MonoBehaviour
{
    [SerializeField]
    public AudioSource _audioSource;
    public AudioClip VfxClip;


    void Start()

    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("El Componente de Audio esta vacio");
        }
        else
        {
            _audioSource.clip = VfxClip;
        }
    }

    void PlaySound()
    {
        _audioSource.Play();
    }
}
