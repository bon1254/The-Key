using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Test : MonoBehaviour
{
    public Vector3 someVector3;
    public Vector2 myVector2;

    void Start()
    {
        myVector2 = Vector2.one + someVector3.ToVector2();    
    }
}
