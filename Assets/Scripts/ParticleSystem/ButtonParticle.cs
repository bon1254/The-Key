using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonParticle : MonoBehaviour
{
    public ParticleSystem[] PS;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Reset()
    {
        for (int i = 0; i < PS.Length; i++)
        {           
            if (PS[i] != null)
            {
                PS[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }

    public void PlayEffect(int index)
    {       
        if (PS[index] != null)
        {
            PS[index].Play();
        }
    }
}
