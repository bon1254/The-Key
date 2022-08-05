using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAnimaionSlateBlock : MonoBehaviour
{
    public DialogueSystemMonkey4 dialogueSystemMonkey4;
    public AudioSource audioSource;

    public void SwitchPlayStoryMap()
    {
        dialogueSystemMonkey4.StoryMapFadeIn();
    }

    public void PlayBrightSound()
    {
        audioSource.Play();
    } 
}
