using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyParticleAudio : MonoBehaviour
{
    AudioSource audioSource;
    public float liftime = 1.0f;

    public float pitchRandomizer = 1.0f;
    public float volumeRandomizer = 0.25f;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.pitch += Random.RandomRange(-pitchRandomizer / 2.0f, pitchRandomizer / 2.0f);
        audioSource.volume += Random.Range(-volumeRandomizer / 2.0f, volumeRandomizer / 2.0f);

        audioSource.Play();

        Destroy(gameObject, liftime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
