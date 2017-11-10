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
/// Todo:
/// ・おおざっぱな移動処理は完成
/// ・各アクションごとに細かな移動制御が必要（高さも入ってない）
/// ・複数MovePointの設定に対応したら壁を登る動作なども設定できるかも
/// 　その際アニメーションのパーセンテージ（Normalize…なんとか）で0～1.0fで取れる
/// ・MovePointに向けてキャラクターの回転動作が必要
/// ・オブジェクトへの突っ込み方に応じて向きがばらついてしまうので
/// ・2017/11/09 oyama add

public class MoveState : MonoBehaviour {

	public float smoothTime = 0.65f;
	public Vector3 m_PlayerPos;
	public Vector3 m_MovePos;

	private Vector3 velocity = Vector3.zero;

	// 外部移動処理が必要な物のリスト
	public enum MoveStatement {
		None,
		Vault,
		Slider,
		Climb,
	}

	public MoveStatement m_NowState;	// 現在ステート
	private bool is_Move = false;       // MoveStateが動きを受け持っているかの判定

	private Animator m_Animator;
	private AllPlayerManager m_AllPlayerMgr;
	public string m_AnimName;	// 再生されている（はず）のアニメーションの名前を受け取る


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
			Action();
			break;

		case MoveStatement.Slider:
			Action();
			break;

		case MoveStatement.Climb:
			Action();
			break;

		default:
			Action();	// Default動作
			break;
		case MoveStatement.None:
			break;
		}
	}

	// Updateの前に行う処理
	private void FixedUpdate() {

		// 座標関係の更新
		m_PlayerPos = this.transform.position;
		

		// 状態取得/切り替え部分
		//DebugPrint.print("Animator.StateInfo = " + m_Animator.IsInTransition(0).ToString());
		// ChangeStateから変更されたら都度アニメーターの状態を監視してプレイ中か判定する
		if (is_Move) {

			//is_Move = m_Animator.GetBool(m_AnimName);   // 再生されているはずのアニメーションの名前からひったくる
			//if(!m_Animator.GetCurrentAnimatorStateInfo(0).IsName(m_AnimName)) {
			if(m_Animator.IsInTransition(0)) {
				is_Move = false;
			}
			// アニメーションの再生が終了した瞬間
			if (!is_Move) {
				//DebugPrint.print("MoveState Exit", 0.5f);
				resetState();
			}
		}
	
	}

	//============================================================
	// アクション用
	//============================================================
	/// <summary>
	/// アクションの移動制御
	/// </summary>
	void Action() {
		// 指定位置まで指定時間で移動する
		m_PlayerPos = Vector3.SmoothDamp(m_PlayerPos, m_MovePos, ref velocity, smoothTime);
		this.transform.position = m_PlayerPos;
	}


	//============================================================
	// 状態取得/変更
	//============================================================
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
	/// <summary>
	/// 外部から移動予定ポイントを設定
	/// </summary>
	public void setMovePosition(Vector3 MovePos) {
		m_MovePos = MovePos;
	}
}
