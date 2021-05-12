using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSceneAnimationEvery : MonoBehaviour
{
    public NextLevel Main;   
    void Awake()
    {
       
    }

    public void Load_AnimationPlaybackFinish()
    {       
        Main.DoSwitchScene();      
    }

    public void Destroy()
    {
        Main.DestoryThis();
    }
}
