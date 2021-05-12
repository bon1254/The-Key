using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAnimaionSlateBlock : MonoBehaviour
{
    public DialogueSystemMonkey4 dialogueSystemMonkey4;
    public AudioClip[] BirghtSound;

    public AudioSource audioSource;
    public void SwitchPlayStoryMap()
    {
        dialogueSystemMonkey4.StoryMapFadeIn();
    }

    public void PlayBlocksSound1()
    {
        audioSource.clip = BirghtSound[0];
        audioSource.Play();
    }

    public void PlayBrightSound2()
    {
        audioSource.clip = BirghtSound[1];
        audioSource.Play();
    }

   
}
