using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
[ExecuteInEditMode]
public class StateMachineManager : StateMachineBehaviour
{
    public StateMachineDirector director;
    public RuntimeAnimatorController targetController = null;

    public virtual void OnEnable()
    {        
        
    }
    public virtual async Task Wait(float time)
    {
        float nowTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - nowTime < time) await Task.Delay(1000 / 60);
    }
}
