using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSceneAniamtionPrologue : MonoBehaviour
{
    [SerializeField] PrologueSceneChange Main;

    public void Load_AnimationPlaybackFinish()
    {
        Main.DoSwitchScene();
    }

    public void AnimationPlaybackFinish()
    {
        Main.DestoryThis();
    }
}
