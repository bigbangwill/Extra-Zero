using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTransition : StateMachineBehaviour
{


    [SerializeField] private Animator orderChangeAnimator;


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //orderChangeAnimator.SetTrigger("");
        
    }




}
