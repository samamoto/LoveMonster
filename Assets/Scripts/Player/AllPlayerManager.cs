using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPlayerManager : MonoBehaviour {

	private static AllPlayerManager _instance;

    //走る速さ
    public float m_RunSpeed = 0.2f;
    public float m_MaxRunSpeed = 5.0f;
    public float m_SideRunSpeed = 0.05f;

    //ジャンプ力
    public float m_JumpPower = 2f;

    //ジャンプ時間
    public float m_JumpTime = 2;

    //回転力
    public float m_RotatePower = 15.0f;

	// プレイヤーの通常動作
	public readonly string[] NormalAction = {
		ConstAnimationStateTags.PlayerStateIdle,
		ConstAnimationStateTags.PlayerStateWalkRun,
		ConstAnimationStateTags.PlayerStateJump,
	};

	// 移動処理を任せる特殊アクション
	public readonly string[] SpecialAction = {
		ConstAnimationStateTags.PlayerStateVault,
		ConstAnimationStateTags.PlayerStateSlider,
		ConstAnimationStateTags.PlayerStateWallRun,
		ConstAnimationStateTags.PlayerStateClimb,
		ConstAnimationStateTags.PlayerStateClimbJump,
	};

	// 現存するPlayerの数
	int m_PlayerNum = 0;

	// ToDo:増えてきたらローカルクラスで管理しよう
	string[] m_PlayerActionNames = new string[ConstPlayerParameter.PlayerMax];

	// コンポーネント
	//--------------------------------------------------------------------------------
	PlayerManager[] m_PlayerManager = new PlayerManager[ConstPlayerParameter.PlayerMax];

	// Use this for initialization
	void Start () {
		// とりあえずFindと名前使う…
		// 名前のPlayer1~4を探す
		// カウンタでタグの数を数える
		m_PlayerNum = TagCount.CountTag("Player");
		for (int i=0; i<m_PlayerNum; i++) {
			// Null check
			m_PlayerManager[i] = GameObject.Find("Player" + (i + 1).ToString()).GetComponent<PlayerManager>();
			m_PlayerActionNames[i] = ConstAnimationStateTags.PlayerStateIdle;
		}
	}

	// Update is called once per frame
	void Update () {

		  // 各プレイヤーの状態を確認するよ！ //

		  // オブジェクトに当たった

		  // アクション用？

		  // アイテム？

		  // プレイヤーと接触した

		  // なんか起こす


		  // アクションの更新(全員分)

		  for (int i = 0; i < m_PlayerNum; i++) {
				// デバッグリスタート
				if(m_PlayerManager[i].transform.position.y < -15.0f) {

					m_PlayerManager[i].restartPlayer();	// 2017/12/01 oyama add
					 //SceneChange.Instance._SceneLoadString("GameSceneProto");  //2017年11月22日 oyama add
				}
			//m_PlayerActionNames[i] = m_PlayerManager[i].getPlayerAction();
		}
		// スコアが更新
		ScoreCheck();

		// ランキングが更新
		RankCheck();

		// など

	}

	// Updateのあとに来る
	private void LateUpdate() {
		  // Updateで確認したフラグ関係はこっちでリセットする //


	 }

	//============================================================
	// Function
	//============================================================

	// reset
	void resetAllPlayerManager() {
		  // 2017年11月22日 oyama add
		  // シングルトンしていると読み込みの時にStartに行かないらしいのでテスト
		  for (int i = 0; i < m_PlayerNum; i++) {
				m_PlayerManager[i] = GameObject.Find("Player" + (i + 1).ToString()).GetComponent<PlayerManager>();
				m_PlayerActionNames[i] = ConstAnimationStateTags.PlayerStateIdle;
		  }
	 }
	 /// <summary>
	 /// 各プレイヤーのスコアを更新
	 /// </summary>
	 void ScoreCheck() {
		// ToDo:ScoreManagerと連携？
	}

	/// <summary>
	/// 各プレイヤーのランクを更新
	/// </summary>
	void RankCheck() {
		// ToDo:ScoreManagerと連携？
	}


	/// <summary>
	/// 指定したタグがAllPlayerManagerの特別アクションに指定されているかを見る
	/// </summary>
	/// <param name="tag">タグ名</param>
	/// <returns>ある/なし</returns>
	public bool TagCheck(string tag) {
		for (int i = 0; i < SpecialAction.Length; i++) {
			if (SpecialAction[i] == tag) {
				return true;
			}
		}
		return false;
	}

	//============================================================
	// getter/setter
	//============================================================
	public int GetPlayerNum() {
		return m_PlayerNum;
	}

	// Singleton
	//------------------------------------------------------------
	//private AllPlayerManger() {
	//	Debug.Log("Create SampleSingleton instance.");
	//}

	/// <summary>
	/// インスタンスの入手
	/// </summary>
	///
	/* //シングルトンやめる
	public static AllPlayerManager Instance {
		get {
			if (_instance == null) _instance = new AllPlayerManager();

			return _instance;
		}
	}
	*/
}
