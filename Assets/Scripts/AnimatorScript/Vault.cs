﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ヴォルトモーション
/// </summary>

public class Vault : StateMachineBehaviour {


	float m_Time = 1.0f;



    AllPlayerManager PM; //プレイヤーの基礎データ取得用
  
    private float m_Vaiult;         //スクリプト内変数
    public  float m_VaultUpSpeed;   //外部受取用変数

    Transform m_PlayerTrans;
    Transform m_MovePos;

	private float startTIme;
	private Vector3 startPos;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PM = AllPlayerManager.Instance;
        //PM = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
        m_Vaiult = m_VaultUpSpeed;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {      
       
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //アニメーターにヴォルトのジャンプの値をセット
        animator.SetFloat("JumpPower", m_Vaiult);

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Vaiult = 0;
        animator.SetFloat("JumpPower", m_Vaiult);

        //ヴォルトに移行していたらフラグを切る
        animator.SetBool("is_Vault", false); 
    }
 
    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
