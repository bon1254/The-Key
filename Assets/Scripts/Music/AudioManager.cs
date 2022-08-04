using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] BGM;
    public AudioSource audioSource;

    private static AudioManager instance = null;
    public static AudioManager Instance
    {
        get { return instance; }
    }
    

    void Awake()
    {       
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void Monkey1BGM()
    {
        audioSource.clip = BGM[0];
        audioSource.Play();
    }
    public void Monkey2to4BGM()
    {
        audioSource.clip = BGM[1];
        audioSource.Play();
    }
    public void ElfBGM()
    {
        audioSource.clip = BGM[2];
        audioSource.Play();
    }
}
