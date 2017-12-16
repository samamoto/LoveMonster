using UnityEngine;

public class AllPlayerManager : MonoBehaviour {
	private static AllPlayerManager _instance;

	// 使われてない

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
		ConstAnimationStateTags.PlayerStateKongVault,
		ConstAnimationStateTags.PlayerStateLongSlider,
		ConstAnimationStateTags.PlayerStateClimbOver,
	};

	// 現存するPlayerの数
	private int m_PlayerNum = 0;
	// プレイヤーがステージを周回する回数
	private const int NEED_GOAL_NUM = 3;

	// ToDo:増えてきたらローカルクラスで管理しよう
	private string[] m_PlayerActionNames = new string[ConstPlayerParameter.PlayerMax];

	// コンポーネント
	//--------------------------------------------------------------------------------
	private PlayerManager[] m_PlayerManager = new PlayerManager[ConstPlayerParameter.PlayerMax];
	private GameObject m_Start;
	private GoalObject m_Goal;
	private MainGameManager m_GameManager;

	// Use this for initialization
	private void Awake() {
		// とりあえずFindと名前使う…
		// 名前のPlayer1~4を探す カウンタでタグの数を数える
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
		for(int i=0; i< m_PlayerNum; i++) {
			// アクションの更新(全員分)
			m_PlayerActionNames[i] = m_PlayerManager[i].getPlayerAction();
		}


		// オブジェクトに当たった

		// アクション用？

		// アイテム？

		// プレイヤーと接触した
		
		// なんか起こす


		// ゴールしているか
		if (m_Goal.getGoal()) {
			int id = m_Goal.getGoalPlayerNo();
			m_PlayerManager[id-1].plusGoalFrequency();    // ゴール回数を追加
			// 必要な回数を上回ればゴール判定
			if (m_PlayerManager[id-1].getGoalFrequency() == NEED_GOAL_NUM) {
				m_GameManager.isPlayerGoal(id, m_PlayerManager[id - 1].getPlayerPos()); // ゴールしたプレイヤーのIDを投げる
			} else {
				m_PlayerManager[id - 1].transform.position = m_Goal.getNextStagePoint();
			}
		}

		// 落下リスタート処理
		for (int i = 0; i < m_PlayerNum; i++) {
			// デバッグリスタート
			if (m_PlayerManager[i].transform.position.y < -25.0f) {
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
	/// プレイヤーのスタート位置を決定する
	/// </summary>
	/// <param name="id">PlayerID</param>
	/// <param name="pos">Playerの位置</param>
	public void startPlayerPosition(int id, Vector3 pos, Quaternion rot) {
		m_PlayerManager[id - 1].gameObject.transform.position = pos;
		m_PlayerManager[id - 1].gameObject.transform.rotation = rot;

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

	/// <summary>
	/// Playerの全ての位置を返す
	/// </summary>
	public Vector3[] getPlayerPositions(int id) {
		Vector3[] vecs = new Vector3[4];
		for(int i =0; i<4; i++) {
			vecs[i] = m_PlayerManager[i].getPlayerPos();
		}
		return vecs;
	}

	public Vector3 getPlayerPosision(int id) {
		if(id > 0 && id < 5) {
			return m_PlayerManager[id-1].getPlayerPos();
		}
		return Vector3.zero;
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