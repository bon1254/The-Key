using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
using System.Threading.Tasks;

namespace Funcy.ConcatenationAnimaStateTool
{
    public class SetParameters : StateMachineManager
    {        
        public string targetAnimatorName = "";
        public List<ParmeterInfo> parameterInfo = new List<ParmeterInfo>();
        [Serializable]
        public class ParmeterInfo
        {
            public string headerStr = "";
            public AnimatorControllerParameterType VariableType = AnimatorControllerParameterType.Bool;
            public int selectIndex = 0;

            public bool AddMode = false;
            public bool SetOneShot = false;
            public bool disabled = false;

            public bool RandomLate = false;
            public Vector2 RandomLateRange = Vector2.zero;

            public bool EnableWaitTime = false;
            public bool CancelInvokeWhenStateExit = false;

            public float WaitTime = 0.5f;
            public string parameterName = "";
            public float floatValue = 0.1f;
            public int intValue = 0;
            public bool boolValue = false;
        }
        bool onState = false;

        public override void OnEnable()
        {
            base.OnEnable();
        }
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            onState = true;
            foreach (var p in parameterInfo)
                Processing(animator, p);
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            onState = false;
        }
        async void Processing(Animator animator, ParmeterInfo p)
        {
            if (p.disabled) return;
            if (p.SetOneShot) p.disabled = true;

            if (p.EnableWaitTime)
            {
                if (p.RandomLate)
                {
                    await
                    Wait(
                    UnityEngine.Random.Range(p.RandomLateRange.x, p.RandomLateRange.y)
                    );
                }
                else
                    await Wait(p.WaitTime);
            }

            if (!onState && p.CancelInvokeWhenStateExit) return;
            switch (p.VariableType)
            {
                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(p.parameterName, p.boolValue);
                    break;
                case AnimatorControllerParameterType.Float:
                    animator.SetFloat(p.parameterName, (!p.AddMode) ?
                       p.floatValue
                       :
                       animator.GetFloat(p.parameterName) + p.floatValue
                       );
                    break;
                case AnimatorControllerParameterType.Int:
                    animator.SetInteger(p.parameterName, (!p.AddMode) ?
                       p.intValue
                       :
                       animator.GetInteger(p.parameterName) + p.intValue
                       );
                    break;
                case AnimatorControllerParameterType.Trigger:
                    animator.SetTrigger(p.parameterName);
                    break;
            }
        }
    }
}