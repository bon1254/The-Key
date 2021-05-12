using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaLdisplay : MonoBehaviour
{
    public Text textDisPL;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 DTPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        textDisPL.transform.position = DTPosition;
    }
}
