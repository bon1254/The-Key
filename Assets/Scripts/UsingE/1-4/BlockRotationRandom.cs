using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class BlockRotationRandom : MonoBehaviour
{
    public bool IsFinish = false;
    [SerializeField] List<BlockRotationRandom> blocks = new List<BlockRotationRandom>();
    Animator anim;
    float currentAnimationTime,nowAnimationTiome;    
    public float RrequireRotationTime = 0.8f;
    float NowTime = -1;
    float CurrentTime;

    void Awake()
    {
        blocks = FindObjectsOfType<BlockRotationRandom>().ToList();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        while(true)
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
                    block.GetComponent<SpriteRenderer>().color = Color.yellow;
                }
            }
        }
    }

    public void ChangeCurrentAngle()
    {
        if (CurrentTime < 1 || IsFinish)
        {
            return;
        }

        currentAnimationTime += 0.25f;
        IsFinish = currentAnimationTime % 1.0f == 0.0f;
        if(IsFinish)
        {
            GetComponent<SpriteRenderer>().color = Color.gray;
        }
        NowTime = Time.realtimeSinceStartup;
    }   
}
