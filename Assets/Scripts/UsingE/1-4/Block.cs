using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Block1 : MonoBehaviour
{
    public bool UnLock = false;
    public bool IsRotate = false;
    public GameObject block1;
    Tweener tweener;
    Quaternion currentAngle;

    Transform BlockTrans;
    float RotateSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        currentAngle = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {       
        if (IsRotate && BlockTrans != null)
        {
            if (Quaternion.Angle(currentAngle, BlockTrans.rotation) > 1)
            {
                //print("開始旋轉");
                BlockTrans.rotation = Quaternion.Slerp(BlockTrans.rotation, currentAngle, RotateSpeed * Time.deltaTime);
            }
            else
            {
                BlockTrans.rotation = currentAngle;
                IsRotate = false;
            }
        }
      
    }

    public void ChangeCurrentAngle()
    {
        if (!IsRotate)
        {
            Debug.Log("12345");
            IsRotate = true;
        }             
    }

    
}
