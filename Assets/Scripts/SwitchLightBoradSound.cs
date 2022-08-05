using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLightBoradSound : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlaySound()
    {
        audioSource.Play();
    }

}
