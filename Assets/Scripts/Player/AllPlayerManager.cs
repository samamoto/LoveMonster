using UnityEngine;
using System.Collections.Generic;

public class AllPlayerManager : MonoBehaviour {

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
	private const int NEED_GOAL_NUM = 4;

	// ToDo:増えてきたらローカルクラスで管理しよう
	private string[] m_PlayerActionNames = new string[ConstPlayerParameter.PlayerMax];

	// テンションが何％でボーナスに遷移するか
	public const float ENTRY_BONUS_TENSION = 0.7f;
	public const int ENTRY_BONUS_PLAYER = 1;

	private bool StopControll = false;

	// コンポーネント
	//--------------------------------------------------------------------------------
	private PlayerManager[] m_PlayerManager = new PlayerManager[ConstPlayerParameter.PlayerMax];
	private List<StartObject> m_StartList = new List<StartObject>();
	private List<GoalObject> m_GoalList = new List<GoalObject>();
	private MainGameManager m_GameManager;

	// Use this for initialization
	private void Awake() {
		m_GameManager = GameObject.FindWithTag("GameManager").GetComponent<MainGameManager>();
		// とりあえずFindと名前使う…
		// 名前のPlayer1~4を探す カウンタでタグの数を数える
		m_PlayerNum = TagCount.CountTag("Player");
		for (int i = 0; i < m_PlayerNum; i++) {
			// Null check
			m_PlayerManager[i] = GameObject.Find("Player" + (i + 1).ToString()).GetComponent<PlayerManager>();
			m_PlayerActionNames[i] = ConstAnimationStateTags.PlayerStateIdle;
		}
		// ゴールとスタートの取得
		GameObject[] game;
		game = GameObject.FindGameObjectsWithTag("Goal");
		m_GoalList = new List<GoalObject>();
		for(int i=0; i<game.Length; i++) {
			m_GoalList.Add(game[i].GetComponent<GoalObject>());
		}
		game.Initialize();
		// Start
		game = GameObject.FindGameObjectsWithTag("Start");
		m_StartList = new List<StartObject>();
		for (int i = 0; i < game.Length; i++) {
			m_StartList.Add(game[i].GetComponent<StartObject>());
		}
		// 順不同なのでもうループ
		for (int i = 0; i < game.Length; i++) {
			// プレイヤーのポジションをセット
			int id = m_StartList[i].StartPlayerID-1;
			m_PlayerManager[id].startPlayerPostion(m_StartList[i].transform.position, m_StartList[i].transform.rotation);
		}
	}

	// Update is called once per frame
	private void Update() {

		if (StopControll) return;

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

		// テンションが規定値を上回れば移動させる
		// getEntryBonusStage()から判定を返し、上位システムから

		// ゴールしているか
		for (int i=0; i < m_GoalList.Capacity; i++) {
			if (m_GoalList[i].getGoal()) {
				int id = m_GoalList[i].getGoalPlayerNo();
				m_PlayerManager[id - 1].plusGoalFrequency();    // ゴール回数を追加

				// 必要な回数を上回ればゴール判定
				if (m_PlayerManager[id - 1].getGoalFrequency() >= NEED_GOAL_NUM) {
					m_GameManager.isPlayerGoal(id, m_PlayerManager[id - 1].getPlayerPos()); // ゴールしたプレイヤーのIDを投げる
				} else {
					// 0 ~ 3をぐるぐる
					m_PlayerManager[(id - 1)].startPlayerPostion(
						m_GoalList[i].getNextStagePoint(), 
						m_GoalList[i].transform.rotation);
					// カメラ
					GameObject.Find("MainCamera" + id.ToString()).GetComponent<ChaseCamera>().resetCamera();
				}
			}
		}

		// 落下リスタート処理
		PlayerFall();

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
	/// 落下処理
	/// </summary>
	private void PlayerFall() {
		for (int i = 0; i < m_PlayerNum; i++) {
			if (m_PlayerManager[i].transform.position.y < -20.0f) {
				m_GameManager.isPlayerDead(i, transform.position);      // GameManagerに落ちたプレイヤーのIDを投げる
				// 落下したプレイヤーの下降処理
				m_PlayerManager[i].gameObject.GetComponent<ComboSystem>()._ClearCombo();
				m_PlayerManager[i].gameObject.GetComponent<Tension>().downTension();

				m_PlayerManager[i].restartPlayer(); // 2017/12/01 oyama add
			}
		}
	}
	/// <summary>
	/// プレイヤーをリスタートさせる
	/// </summary>
	public void restartPlayer() {
		for (int i = 0; i < m_PlayerNum; i++) {
			m_PlayerManager[i].restartPlayer();
		}
	}

	/// <summary>
	/// ボーナスから戻ったときのパラメータ関係をリセットする
	/// </summary>
	public void resetBonusParameter() {
		for (int i = 0; i < m_PlayerNum; i++) {
			m_PlayerManager[i].resetBonusParameter();
		}
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
	/// <summary>
	/// プレイヤーが現在いるステージのIDを返す
	/// </summary>
	/// <param name="n">PlayerのID 1~4</param>
	/// <returns>StageID 1~4</returns>
	public int getPlayerThereStageID(int n) {
		return m_PlayerManager[n - 1].getPlayerStageID();
	}

	//============================================================
	// getter/setter
	//============================================================
	/// <summary>
	/// 誰かがゴールしているか
	/// </summary>
	public bool isGoal() {
		for (int i = 0; i < m_GoalList.Capacity; i++) {
			if (m_GoalList[i].getGoal()) {
				return true;
			}
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

	// 2017年12月22日 oyama add
	/// <summary>
	/// プレイヤーがボーナスステージに入れるかどうかを検出
	/// 全員のテンションが一定値以上になれば
	/// </summary>
	public bool getisEntryBonusStage() {
		int count = 0;
		for (int i = 0; i < m_PlayerNum; i++) {
			if (ENTRY_BONUS_TENSION <= m_PlayerManager[i].gameObject.GetComponent<Tension>().getTensionRatio()) {
			//if (ENTRY_BONUS_TENSION <= m_PlayerManager[0].gameObject.GetComponent<Tension>().getTensionRatio()) {
				count++;
			}
		}
		if (count >= ENTRY_BONUS_PLAYER) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// 外部から停止
	/// </summary>
	/// <param name="flag"></param>
	public void stopControll(bool flag) {
		StopControll = flag;
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