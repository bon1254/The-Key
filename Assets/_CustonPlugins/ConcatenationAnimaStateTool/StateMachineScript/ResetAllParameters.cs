using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Funcy.ConcatenationAnimaStateTool
{
    public class ResetAllParameters : StateMachineManager
    {
        public List<string> IgronParameters = new List<string>();
        public int selectTarget = 0;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ResetAnimParameters(animator);
        }
        public void ResetAnimParameters(Animator animator)
        {
            foreach (var p in animator.parameters)
                if (!IgronParameters.Contains(p.name))
                    switch (p.type)
                    {
                        case AnimatorControllerParameterType.Bool:
                        case AnimatorControllerParameterType.Trigger:
                            animator.SetBool(p.name, false);
                            break;
                        case AnimatorControllerParameterType.Float:
                            animator.SetFloat(p.name, 0);
                            break;
                        case AnimatorControllerParameterType.Int:
                            animator.SetInteger(p.name, 0);
                            break;
                    }
        }
    }
}