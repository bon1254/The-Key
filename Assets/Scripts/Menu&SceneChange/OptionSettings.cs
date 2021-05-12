using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Canvas Graphic;
    public Button Low;
    public Button Medium;
    public Button High;

    void Start()
    {       
      
    }

    public void LowQulity()
    {
        QualitySettings.SetQualityLevel(0);
    }

    public void MidQulity()
    {
        QualitySettings.SetQualityLevel(1);
    }

    public void HighQulity()
    {
        QualitySettings.SetQualityLevel(2);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void Mute()
    {
        Debug.Log("123");
        AudioListener.pause = !AudioListener.pause;
    }

   
}
