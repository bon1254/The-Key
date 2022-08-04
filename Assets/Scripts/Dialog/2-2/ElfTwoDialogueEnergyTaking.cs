using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfTwoDialogueEnergyTaking : MonoBehaviour
{
    public bool InEnergyTaking = false;
    public BoxCollider2D EnergyTakingBox;
    public GameObject DialogueElfes;
    public AudioSource audioSource;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && InEnergyTaking == true)
        {
            EnergyTakingBox.enabled = false;
            DialogueElfes.SetActive(true);
            audioSource.Play();
            InEnergyTaking = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            InEnergyTaking = true;
        }
    }
}
