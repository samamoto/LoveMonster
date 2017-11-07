using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// クライムの登るモーション
/// </summary>

public class Climb : StateMachineBehaviour {

    AllPlayerManager m_PM;        //プレイヤーの基礎データ取得用

    private float m_Climb;        //スクリプト内変数
    public  float m_ClimbUpSpeed; //外部受取用変数
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_PM = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
        m_Climb = 0;

       
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //クライムに移行していたらフラグを切る
        animator.SetBool("is_Climb", false);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    
    }

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
