using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public Vector2 respawnPointMonkey;
    public GameObject DeathUI;
    public float respawnDelay;
    public PlayerMovement Player;
    public AsyncOperation LoadingScene;
    public Monkey monkey;
    public Animator animatorPlayer;
    public Animator animatorMonkey;

    bool facingRight = true;
    public float JumpForce = 90f;

    // Use this for initialization
    void Awake()
    {
        Player = FindObjectOfType<PlayerMovement>();
    }

    public void Respawn()
    {
        Debug.Log("132");
        animatorPlayer.SetTrigger("UnDeath");
        StartCoroutine("RespawnCoroutine");        
    }

    public IEnumerator RespawnCoroutine()
    {
        monkey.IsGrounded = true;
        monkey.GetComponent<BoxCollider2D>().enabled = true;
        monkey.animator.SetBool("IsJumping", false);

        DeathUI.SetActive(false);
        monkey.transform.position = respawnPointMonkey;
        Player.GetComponent<PlayerMovement>().PlayerControlable = false;
        yield return new WaitForSeconds(respawnDelay);
        Player.transform.position = Player.respawnPoint;
        Player.GetComponent<PlayerMovement>().PlayerControlable = true;
        monkey.gameObject.SetActive(false);
        animatorMonkey.SetBool("IsAttacking", false);
        yield return new WaitForSeconds(1);
        monkey.gameObject.SetActive(true);
        monkey.GetComponent<Monkey>().enabled = true;
    }

    
}
