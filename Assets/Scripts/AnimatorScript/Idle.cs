using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーションのスタート位置
/// 立ちモーション
/// </summary>

public class Idle : AnimatorBase {

    //AllPlayerManager m_PM;        //プレイヤーの基礎データ取得用

    private float m_dashSpeed;    //プレイヤーのダッシュ用変数

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		// m_PM = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();	// 基底クラス内で取得
		m_dashSpeed = 0.0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		if (animator.GetBool("is_Grounded")) {
			animator.SetBool("is_Fall", false);
			animator.SetBool("is_LongFall", false);
		}
	}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // アニメーターの中のデータをもらってくる
        m_dashSpeed = animator.GetFloat("Velocity");

        // 走り始めたらWorkRunのフラグをオン
        if (m_dashSpeed >= 0.1f)
        {
            animator.SetBool("is_WalkRun", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK(inverse kinematics) should be implemented here.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}

/*
 * プログラムメモ
 * アニメーションのプログラムに使える
 * または忘れそうなものを残しておく
 * 
    private Animator anim;
    private CharacterController charaCon;

    void Start()
    {
        anim = GetComponent<Animator>();
        charaCon = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 a = Vector3.zero;


        if (Input.GetKey(KeyCode.X))
        {
            anim.SetBool("is_Slider", true);
        }

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("Locomotion.Slider"))
        {
            //アニメーションしてる時
            a.y = 0.5f;
            charaCon.height = 1;
            charaCon.center = a;
            anim.SetBool("is_Slider", false);
        }
        else
        {
            //アニメーションしてない時　
            a.y = 1;
            charaCon.height = 2;
            charaCon.center = a;
         
        }

    }

}  
     */

