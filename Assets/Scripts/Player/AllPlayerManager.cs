﻿using UnityEngine;

public class AllPlayerManager : MonoBehaviour {
	private static AllPlayerManager _instance;

	// 使われてない
	//走る速さ
	/*
	public float m_RunSpeed = 0.2f;

	public float m_MaxRunSpeed = 5.0f;
	public float m_SideRunSpeed = 0.05f;

	//ジャンプ力
	public float m_JumpPower = 2f;

	//ジャンプ時間
	public float m_JumpTime = 2;

	//回転力
	public float m_RotatePower = 15.0f;
	*/

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
	private int m_PlayerNum = 0;

	// ToDo:増えてきたらローカルクラスで管理しよう
	private string[] m_PlayerActionNames = new string[ConstPlayerParameter.PlayerMax];

	// コンポーネント
	//--------------------------------------------------------------------------------
	private PlayerManager[] m_PlayerManager = new PlayerManager[ConstPlayerParameter.PlayerMax];
	private GameObject m_Start;
	private GoalObject m_Goal;
	private MainGameManager m_GameManager;

	// Use this for initialization
	private void Start() {
		// とりあえずFindと名前使う…
		// 名前のPlayer1~4を探す
		// カウンタでタグの数を数える
		m_PlayerNum = TagCount.CountTag("Player");
		for (int i = 0; i < m_PlayerNum; i++) {
			// Null check
			m_PlayerManager[i] = GameObject.Find("Player" + (i + 1).ToString()).GetComponent<PlayerManager>();
			m_PlayerActionNames[i] = ConstAnimationStateTags.PlayerStateIdle;
		}
		m_Goal = GameObject.FindWithTag("Goal").GetComponent<GoalObject>();
		m_Start = GameObject.FindWithTag("Start");
		m_GameManager = GameObject.FindWithTag("GameManager").GetComponent<MainGameManager>();
	}

	// Update is called once per frame
	private void Update() {
		// 各プレイヤーの状態を確認するよ！ //

		// オブジェクトに当たった

		// アクション用？

		// アイテム？

		// プレイヤーと接触した

		// なんか起こす

		// アクションの更新(全員分)

		// ゴールしているか
		if (m_Goal.getGoal()) {
			int id = m_Goal.getGoalPlayerNo();
			m_GameManager.isPlayerGoal(id, m_PlayerManager[id-1].getPlayerPos());	// ゴールしたプレイヤーのIDを投げる
		}

		// 落下リスタート処理
		for (int i = 0; i < m_PlayerNum; i++) {
			// デバッグリスタート
			if (m_PlayerManager[i].transform.position.y < -15.0f) {
				m_GameManager.isPlayerDead(i, transform.position);		// GameManagerに落ちたプレイヤーのIDを投げる
				m_PlayerManager[i].restartPlayer(); // 2017/12/01 oyama add
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

	/// <summary>
	/// 各プレイヤーのスコアを更新
	/// </summary>
	private void ScoreCheck() {
		// ToDo:ScoreManagerと連携？
	}

	/// <summary>
	/// 各プレイヤーのランクを更新
	/// </summary>
	private void RankCheck() {
		// ToDo:ScoreManagerと連携？
	}


	/// <summary>
	/// プレイヤーの操作を不可にする
	/// </summary>
	public void stopPlayerControl() {
		for (int i = 0; i < m_PlayerNum; i++) {
			m_PlayerManager[i].stopControl(true);
		}
	}

	/// <summary>
	/// プレイヤーの操作を戻す
	/// </summary>
	public void returnPlayerControl() {
		for (int i = 0; i < m_PlayerNum; i++) {
			m_PlayerManager[i].stopControl(false);
		}
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
	/// <summary>
	/// 誰かがゴールしているか
	/// </summary>
	public bool isGoal() {
		if (m_Goal.getGoal()) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// プレイヤー人数
	/// </summary>
	public int GetPlayerNum() {
		return m_PlayerNum;
	}

	/// <summary>
	/// PlayerManagerのインスタンスを返す
	/// </summary>
	public PlayerManager GetPlayerManagerInstance(int n) {
		return m_PlayerManager[n];
	}

	/// <summary>
	/// PlayerManagerのインスタンスを全て返す
	/// </summary>
	public PlayerManager[] GetPlayerManagerInstances() {
		return m_PlayerManager;
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