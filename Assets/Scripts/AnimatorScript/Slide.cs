using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スライドのモーション
/// </summary>

public class Slide : AnimatorBase {

    //AllPlayerManager m_PM;        //プレイヤーの基礎データ取得用

    private float m_Slide;      //スクリプト内変数
    public  float m_SlideSpeed; //外部受取用変数

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		// m_PM = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();	// 基底クラス内で取得

		//通常の速度に加えて加速させる
		m_Slide = animator.GetFloat("Velocity") + m_SlideSpeed; 
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }


    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //アニメーターにスライドの値をセット
        animator.SetFloat("Velocity", m_SlideSpeed);

		//スライドに移行していたらフラグを切る
		if (stateInfo.normalizedTime >= 1.0f) {
			animator.SetBool("is_Slide", false);
		}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Slide = 0;
    }

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
