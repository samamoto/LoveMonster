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

	// 外部移動処理が必要な物のリスト
	public enum MoveStatement {
		Vault,
		Slider,
		Climb,
		None,
	}

	// Enumと文字列を管理する場所
	public Dictionary<MoveStatement, string> StateDictionary = new Dictionary<MoveStatement, string>();

	// UnityEditor内で表示 //
	[SerializeField, MultilineAttribute(2)]
	string smoothTimeMessage;

	[SerializeField, Range(0.1f, 5)]
	private float[] smoothTime = new float[(int)MoveStatement.None] { 0.65f, 0.65f, 1.65f};

	//private AnimationCurve[] m_Curve = new AnimationCurve[(int)MoveStatement.None];	// ToDo:カーブつかって個別制御用

	[SerializeField] MoveStatement m_NowState;    // 現在ステート
	[SerializeField] Vector3 m_MovePos;
	[SerializeField] string m_AnimName;   // 再生されている（はず）のアニメーションの名前を受け取る
	[SerializeField] float ActOffsetY = 1.0f;

	Vector3 startPosition;              // 開始点
	Vector3 m_PlayerPos;
	private Vector3 velocity = Vector3.zero;


	private bool is_Move = false;       // MoveStateが動きを受け持っているかの判定

	private Animator m_Animator;
	private AllPlayerManager m_AllPlayerMgr;

	private float startTime;	// 開始時間
	private float deltaCount;   // 開始時間からどれだけ経過したか

	private Quaternion m_PrevRot;
	private bool is_LookRot;	// LookRotationを使って回すか決める


	// Use this for initialization
	void Start () {
		m_NowState = MoveStatement.None;
		m_Animator = GetComponent<Animator>();
		m_AllPlayerMgr = AllPlayerManager.Instance;

		// Dictionaryの更新
		StateDictionary.Add(MoveStatement.Vault, ConstAnimationStateTags.PlayerStateVault);
		StateDictionary.Add(MoveStatement.Slider, ConstAnimationStateTags.PlayerStateSlider);
		StateDictionary.Add(MoveStatement.Climb, ConstAnimationStateTags.PlayerStateClimb);
		StateDictionary.Add(MoveStatement.None, "None");


	}


	// Updateの前に行う処理
	private void FixedUpdate() {

		// 座標関係の更新
		m_PlayerPos = this.transform.position;
		deltaCount += Time.deltaTime;

		// 状態取得/切り替え部分
		//DebugPrint.print("Animator.StateInfo = " + m_Animator.IsInTransition(0).ToString());
		// ChangeStateから変更されたら都度アニメーターの状態を監視してプレイ中か判定する
		if (is_Move) {

			//is_Move = m_Animator.GetBool(m_AnimName);   // 再生されているはずのアニメーションの名前からひったくる
			//if(!m_Animator.GetCurrentAnimatorStateInfo(0).IsName(m_AnimName)) {
			if (m_Animator.IsInTransition(0)) {
				is_Move = false;
			}
			// アニメーションの再生が終了した瞬間
			if (!is_Move) {
				//DebugPrint.print("MoveState Exit", 0.5f);
				resetState();
				return;
			}
		}

	}

	/// <summary>
	/// 外部から操作
	/// </summary>
	public void Update() {
		// アニメーションの再生をしていなければなにもしない
		if (!is_Move) {
			return;
		}

		// ステートに移行してからの差分の時間を取ってLerp関係処理に投げる
		// 座標更新
		float diff = deltaCount - startTime;
		if (diff > smoothTime[(int)m_NowState]) {
			transform.position = m_MovePos;
		}

		float rate = diff / smoothTime[(int)m_NowState];
		//var pos = curve.Evaluate(rate);


		// 状態によって操作を分ける
		switch (m_NowState) {
		case MoveStatement.Vault:
			ActionLerp(rate);
			break;

		case MoveStatement.Slider:
			ActionSlerp(rate);
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

		// 角度の更新
		if(is_LookRot)
			transform.rotation = Quaternion.LookRotation(m_MovePos, Vector3.up);    // 指定の位置を向く

	}


	//============================================================
	// アクション用
	//============================================================
	/// <summary>
	/// アクションの移動制御(SmoothDamp)
	/// </summary>
	void Action() {
		// 指定位置まで指定時間で移動する
		m_PlayerPos = Vector3.SmoothDamp(m_PlayerPos, m_MovePos, ref velocity, smoothTime[(int)m_NowState]);
		this.transform.position = m_PlayerPos;
	}

	/// <summary>
	/// Lerpを使った移動制御
	/// </summary>
	void ActionLerp(float rate) {
		transform.position = Vector3.Lerp(startPosition, m_MovePos, rate);
	}

	/// <summary>
	/// Slerpを使った移動制御
	/// </summary>
	void ActionSlerp(float rate) {
		transform.position = Vector3.Slerp(startPosition, m_MovePos, rate);
	}

	// UnityEditorのときのみ
	void OnDrawGizmosSelected() {
#if UNITY_EDITOR

		if (!UnityEditor.EditorApplication.isPlaying || enabled == false) {
			startPosition = transform.position;
		}

		UnityEditor.Handles.Label(m_MovePos, m_MovePos.ToString());
		UnityEditor.Handles.Label(startPosition, startPosition.ToString());
#endif
		Gizmos.DrawSphere(m_MovePos, 0.1f);
		Gizmos.DrawSphere(startPosition, 0.1f);

		Gizmos.DrawLine(startPosition, m_MovePos);
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
		startTime = Time.deltaTime;
		deltaCount = startTime;
		startPosition = transform.position;
		// LookRotで回す場合とそうでない場合
		if (is_LookRot) {
			transform.rotation = Quaternion.LookRotation(m_MovePos, Vector3.up);    // 指定の位置を向く
		} else {
			// 回さない場合は移動前のY座標を上にして余計な回転をさせないようにする
			//m_PrevRot = transform.rotation;
			m_PrevRot = new Quaternion(0, m_PrevRot.y, 0, m_PrevRot.w);
			transform.rotation = m_PrevRot;
		}
		GetComponent<Rigidbody>().isKinematic = true;	// 制御をONにして外部操作を無効
	}

	/// <summary>
	/// 状態をリセットする
	/// </summary>
	public void resetState() {
		m_NowState = MoveStatement.None;
		m_AnimName = "";
		transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
		GetComponent<Rigidbody>().isKinematic = false;   // 制御をOFFにして操作を有効化
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

	/// <summary>
	///	ブロックと当たったときに判定する
	/// </summary>
	public void OnTriggerEnter(Collider other) {

		// 指定したアクションがあるかを調べる
		if (AllPlayerManager.TagCheck(other.tag)) {
			// 指定のタグなら
			// このときオブジェクトはプレイヤー（正面）に対して背面に作られている必要がある
			// 同じ向きでもともと作られている場合反転処理が個別に必要になるため気をつける
			if (other.tag == ConstAnimationStateTags.PlayerStateClimb || other.tag == ConstAnimationStateTags.PlayerStateClimbJump) {
				is_LookRot = false;
				//m_PrevRotY = other.root.transform.rotation.y;    // 見つかったら座標を確保
				//m_PrevRotY = other.transform.root.rotation.y;	    // 見つかったら（親の）座標を確保
				m_PrevRot = transform.rotation;
				//is_LookRot = true;	// ToDo:↑が上手くいかないのであきらめ(´・ω・｀)
			} else {
				// それ以外はオブジェクトの向き見て
				is_LookRot = true;
			}
		}

	}

}
