using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public float respawnDelay;
    public PlayerMovement gamePlayer;

    // Use this for initialization
    void Start()
    {
        gamePlayer = FindObjectOfType<PlayerMovement>();
    }

    public void Respawn()
    {
        StartCoroutine("RespawnCoroutine");
    }

    public IEnumerator RespawnCoroutine()
    {
        gamePlayer.gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnDelay);
        gamePlayer.transform.position = gamePlayer.respawnPoint;
        gamePlayer.gameObject.SetActive(true);
    }
}
