using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slates : MonoBehaviour
{
    public GameObject[] slates;
    public void Slate1Appear()
    {
        slates[0].SetActive(true);
    }
    public void Slate2Appear()
    {
        slates[1].SetActive(true);
    }
    public void Slate3Appear()
    {
        slates[2].SetActive(true);
    }
    public void Slate4Appear()
    {
        slates[3].SetActive(true);
    }
}
