using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tipfoloow : MonoBehaviour
{
    public GameObject TrackObject;
    public Vector3 Offset;

    void Update()
    {
        /*
        gameObject.transform.position = Camera.main.WorldToScreenPoint(TrackObject.transform.position) + Offset;
        */
    }
}
