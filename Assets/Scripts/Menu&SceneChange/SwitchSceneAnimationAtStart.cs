using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSceneAnimationAtStart : MonoBehaviour
{
    [SerializeField] SceneChange Main;

    public void Load_AnimationPlaybackFinish()
    {
        Main.DoSwitchScene_Start();
    }

    public void AtStartAnimationPlaybackFinish()
    {
        print("123");
        Main.DestoryThis_Start();
    }
}
