using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPController : MonoBehaviour {

    public Sprite gem1;
    public Sprite gem3;
    private SpriteRenderer CPSpriteRenderer;
    public bool CPReached;

	// Use this for initialization
	void Start () {
        CPSpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            CPReached = true;
        }

    }
}
