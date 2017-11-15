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
/// Todo　:　2017/11/09 oyama add
/// ・各アクションごとに細かな移動制御が必要（高さも入ってない）
/// ・複数MovePointの設定に対応したら壁を登る動作なども設定できるかも
/// 　その際アニメーションのパーセンテージ（Normalize…なんとか）で0～1.0fで取れる
/// ・MovePointに向けてキャラクターの回転動作が必要
/// ・オブジェクトへの突っ込み方に応じて向きがばらついてしまうので
/// ・
/// Todo ： 2017/11/15 oyama add
/// ・とりあえずリファクタリングしよう
/// ・各アクションごとにクラス分ける？
/// ・MovePointを複数処理化　リスト使うかな
/// ・MoveStateはもともと管理用に使おうと思ったから、具体的な処理関係は別のスクリプトに任せるか
/// ・いったんクラス内でローカルクラス使ってTestしてできそうだったら分けていくかんじー…？
/// ・enumとかDictionaryはここに置いてていっか



public class MoveState : MonoBehaviour {

	//--------------------------------------------------------------------------------
	// 管理してるところ
	//--------------------------------------------------------------------------------
	// 外部移動処理が必要な物のリスト
	public enum MoveStatement {
		Vault,
		Slider,
		Climb,
		//ClimbJump,
		//WallRun,
		None,
	}

	// Enumと文字列を管理する場所↑と連携してStart()からAddすること
	public Dictionary<MoveStatement, string> StateDictionary = new Dictionary<MoveStatement, string>();
	//--------------------------------------------------------------------------------

	//--------------------------------------------------------------------------------
	// UnityEditor内で表示 //
	//--------------------------------------------------------------------------------
	[SerializeField, MultilineAttribute(2)]
	string smoothTimeMessage;

	[SerializeField, Range(0.1f, 10)]
	private float[] smoothTime = new float[(int)MoveStatement.None] { 0.65f, 0.65f, 1.65f};

	//private AnimationCurve[] m_Curve = new AnimationCurve[(int)MoveStatement.None];	// ToDo:カーブつかって個別制御用

	[SerializeField] MoveStatement m_NowState;			// 現在ステート
	private int m_LerpItr = 0;							// 今の移動場所
	List<Vector3> m_MoveList = new List<Vector3>();	// 移動場所の格納リスト
	[SerializeField] string m_AnimName;                 // 再生されている（はず）のアニメーションの名前を受け取る

	// ボタンの入力処理関係
	float m_HoldButtonTIme = 0f;    // ToDo：ボタン系はまだ
	//--------------------------------------------------------------------------------
	Vector3 startPosition;              // 開始点
	Vector3 m_PlayerPos;                // Playerの

	private Vector3 velocity = Vector3.zero;	// smoothDampで使う

	public bool is_Move = false;   // MoveStateが動きを受け持っているかの判定

	public float startTime;		// 開始時間
	public float deltaCount;		// 開始時間からどれだけ経過したか

	private Quaternion m_PrevRot;	// 移動前の角度を保持	
	private bool is_LookRot;        // LookRotationを使って回すか決める
	public bool is_Arrival;		// 目的地に到着したか
	//--------------------------------------------------------------------------------
	// 参照いろいろ //
	//--------------------------------------------------------------------------------
	private Animator m_Animator;
	private AllPlayerManager m_AllPlayerMgr;

	//================================================================================
	// 関数
	//================================================================================

	// Use this for initialization
	void Start () {
		m_NowState = MoveStatement.None;
		m_Animator = GetComponent<Animator>();
		m_AllPlayerMgr = AllPlayerManager.Instance;

		// --Dictionaryの更新 --//
		StateDictionary.Add(MoveStatement.Vault, ConstAnimationStateTags.PlayerStateVault);
		StateDictionary.Add(MoveStatement.Slider, ConstAnimationStateTags.PlayerStateSlider);
		StateDictionary.Add(MoveStatement.Climb, ConstAnimationStateTags.PlayerStateClimb);
		//StateDictionary.Add(MoveStatementClimbJump, ConstAnimationStateTags.PlayerStateClimbJump);
		//StateDictionary.Add(MoveStatement.WallRun, ConstAnimationStateTags.PlayerStateWallRun);
		StateDictionary.Add(MoveStatement.None, "None");
		//--　　　ここまで　　　--//

	}


	// Updateの前に行う処理
	private void FixedUpdate() {

		// 座標関係の更新
		m_PlayerPos = this.transform.position;
		deltaCount += Time.deltaTime;

		// 状態取得/切り替え部分
		// ChangeStateから変更されたら都度アニメーターの状態を監視してプレイ中か判定する
		if (is_Move) {

			//if (!m_Animator.IsInTransition(0) && MovePosList.Count - 1 == m_NowMoveNum) {
			if (is_Arrival) {
				is_Move = false;
			}

			// アニメーションの再生が終了、または移動が完了
			if (!is_Move) {
				resetState();
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
		// 経過時間diff
		float diff = deltaCount - startTime;
		// 全体再生時間を配列数と割り算して二点間の経過時間を計算する

		// 最終ポイントに到着してなければ
		if (!is_Arrival) {
			// Iterator / n 時間で区切る
			if (diff > (smoothTime[(int)m_NowState] / m_MoveList.Count)) {

				// 最終ポイントに到着
				if (m_LerpItr == m_MoveList.Count-1) {
					transform.position = m_MoveList[m_MoveList.Count-1];
					is_Arrival = true;
				} else {
					m_LerpItr++;    // まだならカウントを増やして次へ
					startTime = Time.deltaTime;
					deltaCount = startTime;
					diff = deltaCount - startTime;
				}
			}
		}

		// rate経過時間 / 
		float rate = diff / (smoothTime[(int)m_NowState] / m_MoveList.Count);

		// 状態によって操作を分ける
		//--------------------------------------------------------------------------------
		if (!is_Arrival) {
			switch (m_NowState) {
			case MoveStatement.Vault:
				ActionLerp(rate);
				break;

			case MoveStatement.Slider:
				ActionLerp(rate);
				break;

			case MoveStatement.Climb:
				ActionSlerp(rate);
				break;

			default:
				Action();   // Default動作
				break;

			case MoveStatement.None:
				break;
			}

			// 角度の更新
			if (is_LookRot) {
				//transform.rotation = Quaternion.LookRotation(m_MoveList[m_MoveList.Count - 1], Vector3.up);    // 指定の位置を向く
				//transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
			}
		}

	}


	//============================================================
	// アクション用
	//============================================================
	
	///--------------------------------------------------------------------------------
	/// <summary>
	/// アクションの移動制御(SmoothDamp)
	/// </summary>
	///--------------------------------------------------------------------------------
	void Action() {
		// 指定位置まで指定時間で移動する
		transform.position = Vector3.SmoothDamp(m_PlayerPos, m_MoveList[m_LerpItr], ref velocity, smoothTime[(int)m_NowState]);
	}
	///--------------------------------------------------------------------------------
	/// <summary>
	/// Lerpを使った移動制御
	/// </summary>
	///--------------------------------------------------------------------------------
	void ActionLerp(float rate) {
		if (m_LerpItr == 0) {
			transform.position = Vector3.Lerp(startPosition, m_MoveList[m_LerpItr], rate);
		} else {
			transform.position = Vector3.Lerp(m_MoveList[m_LerpItr - 1], m_MoveList[m_LerpItr], rate);
		}
	}

	///--------------------------------------------------------------------------------
	/// <summary>
	/// Slerpを使った移動制御
	/// </summary>
	///--------------------------------------------------------------------------------
	void ActionSlerp(float rate) {
		if (m_LerpItr == 0) {
			transform.position = Vector3.Slerp(startPosition, m_MoveList[m_LerpItr], rate);
		} else {
			transform.position = Vector3.Slerp(m_MoveList[m_LerpItr - 1], m_MoveList[m_LerpItr], rate);
		}
	}

	///--------------------------------------------------------------------------------
	// UnityEditorのときのみ
	///--------------------------------------------------------------------------------
	/// ゲームシーン上に経路を描画する
	void OnDrawGizmosSelected() {
#if UNITY_EDITOR

		if (!UnityEditor.EditorApplication.isPlaying || enabled == false) {
			startPosition = transform.position;
		}

		for (int i = 0; i < m_MoveList.Count; i++) {
			if(i == 0) {
				UnityEditor.Handles.Label(transform.position, "Start:\n"+transform.position.ToString());
				Gizmos.DrawWireSphere(transform.position, 0.1f);
			} else {
				UnityEditor.Handles.Label(m_MoveList[i - 1], m_MoveList[i - 1].ToString());
				Gizmos.DrawSphere(m_MoveList[i], 0.1f);
				Gizmos.DrawLine(m_MoveList[i-1], m_MoveList[i]);
			}
#endif
		}
	}

	//================================================================================
	// 状態取得/変更(public)
	//================================================================================

	///--------------------------------------------------------------------------------
	/// <summary>
	/// 状態を取得する
	/// </summary>
	///--------------------------------------------------------------------------------
	public MoveStatement getState() {
		return m_NowState;
	}


	///--------------------------------------------------------------------------------
	/// <summary>
	/// 状態を変更する
	/// 外部(PlayerManager)から変更される
	/// </summary>
	/// <param name="mvState">切り替えるステート</param>
	/// <param name="AnimName"></param>
	///--------------------------------------------------------------------------------
	public void changeState(MoveStatement mvState, string AnimName) {
		// 移動するための初期値をセット
		m_NowState = mvState;
		is_Move = true;
		m_AnimName = AnimName;
		startTime = Time.deltaTime;
		deltaCount = startTime;
		startPosition = transform.position;
		m_LerpItr = 0;
		is_Arrival = false;
		Vector3 aim = m_MoveList[m_MoveList.Count - 1] - this.transform.position;
		GetComponent<Rigidbody>().isKinematic = true;   // 制御をONにして外部操作を無効

		// LookRotで回す場合とそうでない場合
		/// Todo:LookRotationで指定の位置に向かない Rigidbodyとか入れたせいかもしれないなんか切ってできるかどうか 現状スティックの向きで回転角度が変わる
		if (is_LookRot) {

			Quaternion look = Quaternion.LookRotation(aim);
			transform.rotation = look;    // 指定の位置を向く
		} else {
			// 回さない場合は移動前のY座標を上にして余計な回転をさせないようにする
			//m_PrevRot = transform.rotation;
			m_PrevRot = new Quaternion(0, m_PrevRot.y, 0, m_PrevRot.w);
			transform.rotation = m_PrevRot;
		}
	}

	///--------------------------------------------------------------------------------
	/// <summary>
	/// 状態をリセットする
	/// </summary>
	///--------------------------------------------------------------------------------
	public void resetState() {
		m_NowState = MoveStatement.None;
		m_AnimName = "";
		transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
		GetComponent<Rigidbody>().isKinematic = false;   // 制御をOFFにして操作を有効化
		m_LerpItr = 0;
		m_MoveList.Clear(); // ポジションリストをクリア
		is_Arrival = false;
	}

	///--------------------------------------------------------------------------------
	/// <summary>
	/// MoveStateの移動処理が実行中か
	/// </summary>
	///--------------------------------------------------------------------------------
	public bool isMove() {
		return is_Move;
	}

	///--------------------------------------------------------------------------------
	/// <summary>
	/// プレイヤーの移動予定のポジションをセットする
	/// </summary>
	/// <param name="MovePos">移動予定場所の配列</param>
	public void setMovePosition(Vector3[] MovePos) {
		m_MoveList = new List<Vector3>();
		for (int i = 0; i < MovePos.Length; i++) {
			m_MoveList.Add(MovePos[i]);
		}
	}
	/// <summary>
	/// プレイヤーの移動予定のポジションをセットする
	/// </summary>
	/// <param name="MovePos">移動予定場所</param>
	public void addMovePosition(Vector3 MovePos) {
		m_MoveList.Add(MovePos);
	}
	///--------------------------------------------------------------------------------
	///--------------------------------------------------------------------------------
	/// <summary>
	///	ブロックと当たったときに判定する
	/// </summary>
	///--------------------------------------------------------------------------------
	public void OnTriggerEnter(Collider other) {

		// 指定したアクションがあるかを調べる
		if (AllPlayerManager.TagCheck(other.tag)) {
			// 指定のタグなら
			// このときオブジェクトはプレイヤー（正面）に対して背面に作られている必要がある
			// 同じ向きでもともと作られている場合反転処理が個別に必要になるため気をつける
			if (other.tag == ConstAnimationStateTags.PlayerStateClimb || other.tag == ConstAnimationStateTags.PlayerStateClimbJump) {
				is_LookRot = false;
				m_PrevRot = transform.rotation;
			} else {
				// それ以外はオブジェクトの向き見て
				is_LookRot = true;
			}
		}

	}

}
