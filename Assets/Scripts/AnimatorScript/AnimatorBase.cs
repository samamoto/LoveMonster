using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーター用スクリプトのベース
/// 共通処理はこちらで管理し、派生クラスで具体的な処理を書く
/// </summary>

public class AnimatorBase : StateMachineBehaviour {

    static protected AllPlayerManager m_PM;    //プレイヤーの基礎データ取得用はStateMacineBehavior内で一個だけ

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		m_PM = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
		//m_PM = AllPlayerManager.Instance;	//Singleton化してるのでFindからインスタンス取得手段を変更 2017/11/10 oyama add

	}
	/*
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
	*/
	//--------------------------------------------------------------------------------
	// 自作継承用関数
	//--------------------------------------------------------------------------------
	/// <summary>
	/// Animatorに与えた値をリセットする。Stateの切り替え時などに使う
	/// Float値のみ
	/// </summary>
	protected void resetAnimatorParameter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetFloat("JumpPower", 0f);
		animator.SetFloat("Velocity", 0f);
		//animator.SetFloat("AngularSpeed", 0f);
		animator.SetFloat("Direction", 0f);
		//animator.SetFloat("Jump", 0f);
		animator.SetFloat("JumpLeg", 0f);
	}

}
