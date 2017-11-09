using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------
// プレイヤーがアクションを切り替えて移動をするときに使用する
// PlayerManagerから使用する
// MoveState側で移動管理しているときはPlayerManagerの
// 操作を受け付けないようにする
// 移動処理中に外部から接触などの判定があった場合は別途相談
//------------------------------------------------------------

public class MoveState : MonoBehaviour {

	// 外部移動処理が必要な物のリスト
	public enum MoveStatement {
		Vault,
		None,
	}

	private MoveStatement m_NowState;	// 現在ステート
	private bool is_Move = false;       // MoveStateが動きを受け持っているかの判定

	private Animator m_Animator;
	private AllPlayerManager m_AllPlayerMgr;
	private string m_AnimName;	// 再生されている（はず）のアニメーションの名前を受け取る


	// Use this for initialization
	void Start () {
		m_NowState = MoveStatement.None;
		m_Animator = GetComponent<Animator>();
		m_AllPlayerMgr = AllPlayerManager.Instance;
	}

	/// <summary>
	/// 外部から操作
	/// </summary>

	public void Update() {
		// アニメーションの再生をしていなければなにもしない
		if (!is_Move) {
			return;
		}

		// 状態によって操作を分ける
		switch (m_NowState) {
		case MoveStatement.Vault:
			this.transform.Translate(0, 0, 0.02f);	// まだ仮
			break;
		default:
		case MoveStatement.None:
			break;
		}
	}

	// Updateの前に行う処理
	private void FixedUpdate() {

		// ChangeStateから変更されたら都度アニメーターの状態を監視してプレイ中か判定する
		if (is_Move) {
			//is_Move = m_Animator.GetBool(m_AnimName);   // 再生されているはずのアニメーションの名前からひったくる
			if(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
				is_Move = false;
			}
			// アニメーションの再生が終了した瞬間
			if (!is_Move) {
				DebugPrint.print("MoveState Exit", 0.5f);
				resetState();
			}
		}
		

		// お試し
		/*
		if (m_Animator.GetBool(0))

			// Animatorの状態を確認してMoveStateの状態を更新する
			// 特殊アクションかを判定して該当すれば
			bool is_Hit = false;

		for (int i = 0; i < m_AllPlayerMgr.SpecialAction.Length; i++) {
			// アニメーターのステートと特殊アクションを比較する
			if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName(m_AllPlayerMgr.SpecialAction[i])) {
				is_Hit = true;
				break;
			}
		}

		// 特殊アクションの場合
		if (is_Hit) {

			// 現在の再生中のアニメーションを見て同じなら
			m_Animator.


		} else {


		}
		*/

	}

	// ============================================================
	// 状態取得/変更
	// ============================================================
	/// <summary>
	/// 状態を取得する
	/// </summary>
	public MoveStatement getState() {
		return m_NowState;
	}

	/// <summary>
	/// 状態を変更する
	/// 外部(PlayerManager)から変更される
	/// </summary>
	/// <param name="mvState">切り替えるステート</param>
	/// <param name="AnimName"></param>
	public void changeState(MoveStatement mvState, string AnimName) {
		// 再生情報を保存
		m_NowState = mvState;
		is_Move = true;
		m_AnimName = AnimName;
		//DebugPrint.print("MoveState Start",1.0f);
	}

	/// <summary>
	/// 状態をリセットする
	/// </summary>
	public void resetState() {
		m_NowState = MoveStatement.None;
		m_AnimName = "";
	}

	/// <summary>
	/// MoveStateの移動処理が実行中か
	/// </summary>
	public bool isMove() {
		return is_Move;
	}

}
