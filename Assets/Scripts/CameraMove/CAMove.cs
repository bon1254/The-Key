using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CAMove : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera c_VirtualCamera;
    public Transform target;
    public Transform target2;

    public void ChangeTarget()
    {
        c_VirtualCamera.m_LookAt = target.transform;
        c_VirtualCamera.m_Follow = target.transform;
        Invoke("BackToTarget", 3);
    }

    public void BackToTarget()
    {
        c_VirtualCamera.m_LookAt = target2.transform;
        c_VirtualCamera.m_Follow = target2.transform;
    }
}
