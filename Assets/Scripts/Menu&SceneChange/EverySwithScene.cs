using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EverySwitchScene : MonoBehaviour
{
    [SerializeField] NextLevel EveryMain;

    public void Load_AnimationPlaybackFinish()
    {
        EveryMain.DoSwitchScene();
    }

    public void AnimationPlaybackFinish()
    {
        EveryMain.DestoryThis();
    }
}