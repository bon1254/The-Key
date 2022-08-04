using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyParticleCollision : MonoBehaviour
{
    ParticleSystem particleSystem;
    List<ParticleCollisionEvent> particleCollsionEvents = new List<ParticleCollisionEvent>();

    public GameObject instantiateOnParticleCollision;
    
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = particleSystem.GetCollisionEvents(other, particleCollsionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Instantiate(instantiateOnParticleCollision, particleCollsionEvents[i].intersection, Quaternion.identity);
        }
    }
}
