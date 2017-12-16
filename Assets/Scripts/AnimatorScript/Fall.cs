using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 落下のモーション
/// </summary>
public class Fall : AnimatorBase {


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {      
       
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		//animator.SetBool("is_Fall", false);
		// 10回ループされたら長時間落ちるモーションに
		if(stateInfo.normalizedTime >= 20.0f) {
			animator.SetBool("is_LongFall", true);
			animator.SetBool("is_Fall", false);
		}

		animator.SetBool("is_Fall", false);
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		animator.SetBool("is_LongFall", false);
		animator.SetBool("is_Fall", false);
	}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
