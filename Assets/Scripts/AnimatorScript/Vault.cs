using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ヴォルトモーション
/// </summary>

public class Vault : AnimatorBase {

	//AllPlayerManager m_PM; //プレイヤーの基礎データ取得用
  

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

		// m_PM = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();	// 基底クラス内で取得

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {      
       
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

		//ヴォルトに移行していたらフラグを切る
		if (stateInfo.normalizedTime >= 1.0f) {
			animator.SetBool("is_Vault", false);
		}
	}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		
		resetAnimatorParameter(animator, stateInfo, layerIndex);

    }
 
    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
