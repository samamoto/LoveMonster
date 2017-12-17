using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スライドのモーション
/// 音声の再生が遅いので、こちら側から制御する　2017年12月17日 oyama add　
/// </summary>

public class Slide : AnimatorBase {

    //AllPlayerManager m_PM;        //プレイヤーの基礎データ取得用

    private float m_Slide;      //スクリプト内変数
    public  float m_SlideSpeed; //外部受取用変数
	// ①再生するAudioListを指定する
	AudioList.SoundList_SE PlaySE = AudioList.SoundList_SE.SE_ActionSlide;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		// m_PM = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();	// 基底クラス内で取得

		//通常の速度に加えて加速させる
		m_Slide = animator.GetFloat("Velocity") + m_SlideSpeed;
		// ②DelayをかけてSEを再生するための遅延時間をセット
		setPlaySEDelay(0.1f);	// 0.1f後に再生
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		PlaySEDelay(PlaySE);// ③Delayをかけてサウンド再生
		SEDelay();			// ③重複再生を防止するためのDelay機構(使ってないけど)
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
		PlaySEDelay();  // ④引数無しでクリア
	}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
