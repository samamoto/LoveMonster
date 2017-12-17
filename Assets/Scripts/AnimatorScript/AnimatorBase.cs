using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーター用スクリプトのベース
/// 共通処理はこちらで管理し、派生クラスで具体的な処理を書く
/// 音声の再生が遅いので、こちら側から制御する　2017年12月17日 oyama add　
/// </summary>

public class AnimatorBase : StateMachineBehaviour {
	// staticがついているやつは共通で一つしか持たない
    static protected AllPlayerManager m_PM;    //プレイヤーの基礎データ取得用はStateMacineBehavior内で一個だけ
	static protected AudioList m_Audio;
	static protected float m_seDelay = 0f;      // 重複再生制御用
	static protected float m_sePlayDelay = 0f;  // SEをずらして再生する場合
	private bool m_isSePlayDelay = false;		// ずらして再生するフラグ
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		m_PM = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
		m_Audio = GameObject.Find("SoundManager").GetComponent<AudioList>();
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
	/// SEを再生する
	/// </summary>
	protected void PlaySE(AudioList.SoundList_SE se) {
		m_Audio.PlayOneShot((int)se);
	}


	protected void setPlaySEDelay(float delay) {
		m_sePlayDelay = delay;
	}
	/// <summary>
	/// sePlayDelayが指定されたタイミングになったら再生するようにする
	/// </summary>
	/// <param name="selist">再生する音</param>
	/// <returns>再生されたらtrue</returns>
	protected bool PlaySEDelay(AudioList.SoundList_SE selist) {
		// 
		if (!m_isSePlayDelay) {
			m_sePlayDelay -= Time.deltaTime;

			if (m_sePlayDelay <= 0) {
				m_isSePlayDelay = true;
				m_Audio.PlayOneShot((int)selist);   // 投げられた音声を再生
				return true;
			}
		}
		return false;
	}
	/// <summary>
	/// sePlayDelayが指定されたタイミングになったら再生するようにする
	/// 引数無しでクリアする(もしくはなにもしない)
	/// </summary>
	protected bool PlaySEDelay() {
		m_isSePlayDelay = false;
		m_sePlayDelay = 0f;
		return false;
	}


	//! SEDelay系統は使ってないけど同じルーチンをいちおう記述しておく

	/// <summary>
	/// Delayをセット
	/// </summary>
	protected void setSEDelay(float delay) {
		m_seDelay = delay;
	}

	/// <summary>
	/// Update内で回す
	/// </summary>
	protected bool SEDelay() {
		if (m_seDelay > 0) {
			m_seDelay -= Time.deltaTime;
		} else {
			m_seDelay = 0;
			return true;
		}
		return false;
	}

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
