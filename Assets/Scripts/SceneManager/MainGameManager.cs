using UnityEngine;

//RequireComponentで指定することでそのスクリプトが必須であることを示せる
[RequireComponent(typeof(PauseManager))]
[RequireComponent(typeof(TimeManager))]
public class MainGameManager : MonoBehaviour {

	private float AllStageLength = 0f;
	private float BonusAliveCount = 0f;				// ボーナスステージのカウント
	private const float BONUS_ALIVE_TIME = 40.0f;	// ボーナスステージの生存時間
	//スクリプト群
	private SceneChange m_ScreenChange;
	private AllPlayerManager m_AllPlayerMgr;
	private AllCameraManager m_AllCameraMgr;
	// 2017年12月07日 oyama add　
	private TimeManager m_TimeMgr;
	private PauseManager m_PauseMgr;
	private CountDownSystem m_CountSys;
	private AudioList m_Audio;
	private PrintScore m_Score;
	private WorldHeritageSpawner m_WorldSpw;
	private ItemFlag m_BonusFlag;
	public AudioList.SoundList_BGM gameBGM = AudioList.SoundList_BGM.BGM_Game_Stage0;

	public enum PhaseLevel {
		Start,
		CountDown,
		CountEnd,
		Game,
		Game_Bonus_Start,
		Game_Bonus_CameraMove,
		Game_Bonus,
		Game_Bonus_End,
		Pause,
		Goal,
		Result,
		None,
	};

	[SerializeField] private PhaseLevel m_Phase = PhaseLevel.None;
	private PhaseLevel m_oldPhase = PhaseLevel.None;
	private PhaseLevel m_PrevPausePhase = PhaseLevel.Game;	// ポーズが掛かる前のフェイズ
	// Use this for initialization
	private void Start() {
		// シーンが生成されたらStart
		m_Phase = PhaseLevel.Start;
		m_ScreenChange = GetComponent<SceneChange>();
		m_AllPlayerMgr = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
		m_AllCameraMgr = GameObject.Find("AllCameraManager").GetComponent<AllCameraManager>();
		m_TimeMgr = GameObject.Find("TimeManager").GetComponent<TimeManager>();
		m_PauseMgr = GameObject.Find("PauseManager").GetComponent<PauseManager>();
		m_CountSys = GameObject.Find("CountDownSystem").GetComponent<CountDownSystem>();
		m_Audio = GameObject.Find("SoundManager").GetComponent<AudioList>();
		m_Score = GameObject.Find("ScoreManager").GetComponent<PrintScore>();
		m_WorldSpw = GameObject.Find("WorldHeritageSpawner").GetComponent<WorldHeritageSpawner>();

		// ステージ全体の長さを記録
		for (int i = 0; i < 4; i++) {
			AllStageLength += getStageLength(i);
		}
	}

	// Update is called once per frame
	private void Update() {
		// 現在のPhaseに合わせて処理を分ける
		switch (m_Phase) {
		//================================================================================
		// CountStart-Phase
		//================================================================================
		// ステージ読み込み時の処理
		case PhaseLevel.Start:
			setPhaseState(PhaseLevel.CountDown);    // シーン読み込まれたら次のカウントへ
			m_CountSys.startCountDown();
			m_AllPlayerMgr.stopPlayerControl();     // プレイヤーのコントロールをOFF
			m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_ActionCountDown );
			m_PauseMgr.PauseRestriction(true);  // PAUSE禁止
            m_TimeMgr.stopTimer();
			m_Score.stopControll(true);
            break;

		//================================================================================
		// CountDown-Phase
		//================================================================================
		// カウントダウン中の処理
		case PhaseLevel.CountDown:
			// カウントダウンのアップデートが走っている気がする
			if (!m_CountSys.getCountStatus()) {
				setPhaseState(PhaseLevel.CountEnd);	// 次へ
			}
			break;

		//================================================================================
		// CountEnd-Phase
		//================================================================================
		// カウントが終わってゲームがスタートする1フレームだけ　リセット処理とか
		case PhaseLevel.CountEnd:
			// カウントダウンストップ(たぶん向こう側でされてるけど)
			m_CountSys.stopCountDown();
			// タイマースタートする
			m_TimeMgr.startTimer();
			m_AllPlayerMgr.returnPlayerControl();     // プレイヤーのコントロールをON
			setPhaseState(PhaseLevel.Game);
			m_Audio.Play((int)gameBGM);
			m_PauseMgr.PauseRestriction(false);
			m_Score.stopControll(false);
			break;

		//================================================================================
		// Game-Phase
		//================================================================================
		// メインのゲーム処理
		case PhaseLevel.Game:
			// ゲーム中にポーズ掛かったらPhase移行
			if (m_PauseMgr.getPauseState()) {
				setPhaseState(PhaseLevel.Pause);
				m_PrevPausePhase = m_Phase; // Pause前の状態を記録
			}
			// ボーナスにいけるようになったら
			if (m_AllPlayerMgr.getisEntryBonusStage()) {
				setPhaseState(PhaseLevel.Game_Bonus_Start);
			}

			break;

		//================================================================================
		// Game-Bonus-Phase-Start
		//================================================================================
		case PhaseLevel.Game_Bonus_Start:
			m_PauseMgr.PauseRestriction(true);
			// 指定座標までプレイヤー移動させる
			setPhaseState(PhaseLevel.Game_Bonus_CameraMove);
			m_AllPlayerMgr.stopControll(true);
			try {
				// 出てきた旗を探す(常に一個しか出ないようにして参照切り替える)
				m_BonusFlag = GameObject.Find("ItemFlag").GetComponent<ItemFlag>();
			}
			catch (System.NullReferenceException) {
				Debug.Log("まだ一回だけしか対応してない");
			}
			break;

		//================================================================================
		// Game-Bonus-Phase-CameraMove
		//================================================================================
		case PhaseLevel.Game_Bonus_CameraMove:
			// カメラの動作待ち
			if (m_WorldSpw.isControll) {
				setPhaseState(PhaseLevel.Game_Bonus);
				Transform trs = GameObject.Find("BonusStart1").transform;
				for (int i = 0; i < 4; i++) {
					trs.position = new Vector3(trs.position.x, trs.position.y, trs.position.z - 2.0f);  // 横にずらす
					m_AllPlayerMgr.startPlayerPosition(i + 1, trs.position, trs.rotation);
				}
				// 移動完了したらカメラの位置をリセットして背面に回す
				m_AllCameraMgr.resetCamera();
				m_PauseMgr.PauseRestriction(false);
				m_AllPlayerMgr.stopControll(false);
			}
			break;

		//================================================================================
		// Game-Bonus-Phase
		//================================================================================
		// メインのゲームボーナスステージ処理
		case PhaseLevel.Game_Bonus:
			// ゲーム中にポーズ掛かったらPhase移行
			if (m_PauseMgr.getPauseState()) {
				setPhaseState(PhaseLevel.Pause);
				m_PrevPausePhase = m_Phase; // Pause前の状態を記録
			}

			BonusAliveCount += Time.deltaTime;
			// 時間で終了 または取られたら終了
			if(BonusAliveCount >= BONUS_ALIVE_TIME　|| m_BonusFlag.is_Get) {
				setPhaseState(PhaseLevel.Game_Bonus_End);
			}
			// Pauseメニュー表示 //


			// 
			/* Todo:ボーナスステージに遷移したら
			 ・ボーナスステージが生えてくる
				・UI非表示
				・Pause禁止
				・キャラクターの動きを停止
				・カメラ切り替え
			　を行う

			・移動前の座標を保持
			・各ポイントにプレイヤーを配置
			・テンションゲージが下降していく？
			　・もう落ちたら終了で良いんじゃないかな…
			  ・落ちたらすぐ近くのリスタートポイントに戻す
			・ボーナスが終わったら
			 */
			break;
		//================================================================================
		// Game-Bonus-Phase-End
		//================================================================================
		case PhaseLevel.Game_Bonus_End:
			// ボーナスステージを削除
			m_WorldSpw.stageRelease = true;
			m_AllPlayerMgr.stopControll(false);
			// ボーナスステージのカウンタをリセット
			BonusAliveCount = 0f;

			// プレイヤーの位置をリセット場所まで戻す
			m_AllPlayerMgr.restartPlayer();
			m_AllPlayerMgr.resetBonusParameter();
			// ゲームに戻る
			setPhaseState(PhaseLevel.Game);
			break;

		
		//================================================================================
		// Pause-Phase
		//================================================================================
		// ゲーム中にポーズがかかる
		case PhaseLevel.Pause:
			if (m_PauseMgr.getPauseState()) {
				m_TimeMgr.stopTimer();  // タイマー停止
				m_Score.stopControll(true);
			} else {
				// 解除されたらPhaseを戻す
				setPhaseState(m_PrevPausePhase);
				m_TimeMgr.startTimer(); // タイマー戻す
				m_Score.stopControll(false);
			}

			break;

		//================================================================================
		// Goal-Phase
		//================================================================================
		// 誰かがゴールする
		case PhaseLevel.Goal:
			// 演出エフェクトとかUIとか発生させるのに使う
			// 指定時間以上経過したらシーンを遷移させる
			// とりあえず今はさっさと遷移する
			setPhaseState(m_Phase + 1); // 次のPhase
			break;

		//================================================================================
		// Result-Phase
		//================================================================================
		// リザルト画面に遷移する
		case PhaseLevel.Result:
			m_TimeMgr.resetTimer(); // タイマーリセットしておく
									// Resultで使うならGlobalParamに投げておくべき
			SceneChange.Instance._SceneLoadResult();
			m_PauseMgr.PauseRestriction(true);
			break;

		// なにもない
		default:
		case PhaseLevel.None:
			break;
		}
	}

	// Updateのあとに走る
	private void LateUpdate() {
		m_oldPhase = m_Phase;
	}

	/// <summary>
	/// 現在のフェイズを取得する
	/// </summary>
	/// <returns>現在実行中のフェイズ</returns>
	public PhaseLevel getGamePhase() {
		return m_Phase;
	}

	/// <summary>
	/// プレイヤーが死んだ、落ちたときにする処理
	/// AllPlayerManagerから落ちたプレイヤーのIDが投げられる
	/// </summary>
	public void isPlayerDead(int id, Vector3 fallPos) {
		// べつのマネージャと連携するとか
		// 誰かが落ちたら1以上
		// 次回フレームからリスタート場所
		if (id > 0) {
		}
	}

	/// <summary>
	/// ゴールしたプレイヤーに対してする処理
	/// </summary>
	public void isPlayerGoal(int id, Vector3 goalPos) {
		// 誰かがゴールしたら1以上
		if (id > 0) {
			// なにかの条件になったらつぎのステートに遷移させる
			setPhaseState(PhaseLevel.Goal + 1); // ゴールの次
		}
	}

	/// <summary>
	/// 次回のPhaseをセットする
	/// </summary>
	/// <param name="phase">遷移するPhase</param>
	private void setPhaseState(PhaseLevel phase) {
		m_Phase = phase;
	}

	//--------------------------------------------------------------------------------
	// Function
	//--------------------------------------------------------------------------------
	/// <summary>
	/// ステージ全体の距離を取得
	/// </summary>
	/// <returns>ステージ全体距離</returns>
	public float getAllStageLength() {
		return AllStageLength;
	}

	/// <summary>
	/// StartとGoalまでのステージの長さを返す
	/// </summary>
	/// <param name="n">ID</param>
	/// <returns>ステージの直線距離の長さ</returns>
	public float getStageLength(int n) {
		Vector3 start, goal;
		start = GameObject.Find("StartPoint" + (n + 1).ToString()).GetComponent<Transform>().position;
		goal = GameObject.Find("GoalPoint" + (n + 1).ToString()).GetComponent<Transform>().position;
		// エラーの場合は0
		if (start == null || goal == null) {
			return 0.0f;
		}
		//Debug.Log(Vector3.Distance(start, goal));
		return System.Math.Abs(Vector3.Distance(start, goal));
	}

	/// <summary>
	/// プレイヤーがスタートからどのくらい距離を離れているか
	/// </summary>
	/// <param name="n">ID</param>
	/// <param name="vec">プレイヤーの位置</param>
	public float getPlayerStartRange(int n, Vector3 vec) {
		Vector3 start;
		start = GameObject.Find("StartPoint" + (n + 1).ToString()).GetComponent<Transform>().position;
		return System.Math.Abs(Vector3.Distance(start, vec));
	}
	/// <summary>
	/// PlayerとGoalまでのステージの長さを返す
	/// </summary>
	/// <param name="n">ID</param>
	/// <param name="vec">プレイヤーの位置</param>
	/// <returns>直線距離の長さ</returns>
	public float getStageLengthToProcessRate(int n, Vector3 vec) {
		Vector3 start, goal;
		start = vec;
		try {
			goal = GameObject.Find("GoalPoint" + (n).ToString()).GetComponent<Transform>().position;
		}
		catch (System.NullReferenceException) {
			goal = Vector3.zero;
		}
		// エラーの場合は0
		if (start == null || goal == Vector3.zero) {
			return 0.0f;
		}
		Debug.Log(Vector3.Distance(start, goal));
		return System.Math.Abs(Vector3.Distance(start, goal));
	}

	/// <summary>
	/// PlayerとGoalまでのすべてのステージの長さとの進捗率に対する比率を返す
	/// </summary>
	/// <param name="range">現在進んだ距離 0~ステージ全体距離</param>
	/// <returns>直線距離の長さ</returns>
	public float getAllStageLengthToProcessRate(float range) {
		return range / AllStageLength;
	}
}