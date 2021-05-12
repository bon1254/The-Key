using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLightBoradSound : MonoBehaviour
{
    public AudioSource audioSource;
    public SwitchAnimaionSlateBlock SASB;

    public void PlaySound()
    {
        SASB.PlayBlocksSound1();
    }

}
