using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ジャンプのモーション
/// </summary>

public class Jump : AnimatorBase {

    //AllPlayerManager m_PM;            //プレイヤーの基礎データ取得用

    private float m_Jump;           //スクリプト内変数
    public  float m_JumpUpSpeed;    //外部受取用変数

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		// m_PM = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();	// 基底クラス内で取得

		m_Jump = m_JumpUpSpeed;      
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {      
       
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //ジャンプの値をアニメーターにセット
        animator.SetFloat("JumpPower", m_Jump);

        //ジャンプに移行していたらフラグを切る
        animator.SetBool("is_Jump", false);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //データのリセット
        m_Jump = 0;
        animator.SetFloat("JumpPower", m_Jump);

        //ジャンプモーションが終わったら落ちるモーションフラグをオン
        //未実装
        animator.SetBool("is_Fall", true);
    }

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
