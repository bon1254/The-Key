using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BlockRotationRandom : MonoBehaviour
{
    public bool IsFinish = false;
    public GameObject LightBoard;
    public GameObject DialogueSlateBlock1;
    public AudioSource audioSource;
    public AudioClip BlockSound;
    public Animator animator , animator2;
    [SerializeField] List<BlockRotationRandom> blocks = new List<BlockRotationRandom>();
    Animator anim;
    float currentAnimationTime,nowAnimationTiome;    
    public float RrequireRotationTime = 0.8f;
    float NowTime = -1;
    float CurrentTime;

    void Awake()
    {
        //audioSource = GetComponent<AudioSource>();
        blocks = FindObjectsOfType<BlockRotationRandom>().ToList();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        while (true)
        {
            int randomNumber = Random.Range(0, 4);
            if (randomNumber == 0) continue;
            currentAnimationTime = 1.0f * randomNumber / 4.0f;
            nowAnimationTiome = currentAnimationTime;
            break;
        }
    }
    void Update()
    {       
        CurrentTime = Mathf.Clamp01((Time.realtimeSinceStartup - NowTime) / RrequireRotationTime);
        nowAnimationTiome = Mathf.Lerp(nowAnimationTiome, currentAnimationTime, CurrentTime);

        anim.Play("BlockRotation", 0, nowAnimationTiome);

        if (blocks.IndexOf(this) == 0)
        {
            bool IsLevelPassed = blocks.Find(b => !b.IsFinish) == null;
            if (IsLevelPassed)
            {
                Debug.Log("過關");
                foreach (var block in blocks)
                {
                    animator.Play("LightBorad");
                    animator2.Play("altarUp");
                    DialogueSlateBlock1.SetActive(true);
                }
            }
        }
    }

    public void ChangeCurrentAngle()
    {
        audioSource.clip = BlockSound;
        if (CurrentTime < 1)
        {
            return;
        }

        currentAnimationTime += 0.25f;
        audioSource.Play(); 
        IsFinish = currentAnimationTime % 1.0f == 0.0f;
        if(IsFinish)
        {
            Debug.Log("OK");
        }
        NowTime = Time.realtimeSinceStartup;
    }

    public void TwirlBlockSound()
    {
        //audioSource.mute = false;
        Debug.Log("123");
        //audioSource.Play();
    }
}
