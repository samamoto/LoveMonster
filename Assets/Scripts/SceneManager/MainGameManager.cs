using TMPro;
using UnityEngine;
using UnityEngine.UI;

//RequireComponentで指定することでそのスクリプトが必須であることを示せる
[RequireComponent(typeof(PauseManager))]
[RequireComponent(typeof(TimeManager))]
public class MainGameManager : MonoBehaviour {
	private float AllStageLength = 0f;
	private float BonusAliveCount = 0f;             // ボーナスステージのカウント
	private const float BONUS_ALIVE_TIME = 1.0f;    // ボーナスステージの生存時間
	private float timeCount = 0f;                   // フェード処理などの汎用タイマー

	public int BonusEntryCount { get; private set; }            // ボーナスの出現回数
	public const float BONUS_ENTRY_NUM = 1;     // ボーナスステージが何回出現するか

	//スクリプト群
	private SceneChange m_ScreenChange;

	private AllPlayerManager m_AllPlayerMgr;
	private AllCameraManager m_AllCameraMgr;

	// 2017年12月07日 oyama add　
	private TimeManager m_TimeMgr;

	private PauseManager m_PauseMgr;
	private CountDownSystem m_CountSys;
	private TextMeshProUGUI[] m_PlayUser = new TextMeshProUGUI[4];
	private AudioList m_Audio;
	private PrintScore m_Score;

	// 画像群
	private Image m_FinishLogo;

	private Image m_AlertBelt;
	private Image m_AlertCenter;

	private Image m_ScreenAlert;
	private Image m_ScreenFade;

	private WorldHeritageSpawner m_WorldSpw;
	private ItemFlag m_BonusFlag;

	public AudioList.SoundList_BGM gameBGM = AudioList.SoundList_BGM.BGM_Game_Stage0;

	public enum PhaseLevel {
		Start_Fade,
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

	[SerializeField]
	private PhaseLevel m_Phase = PhaseLevel.None;
	private PhaseLevel m_oldPhase = PhaseLevel.None;
	private PhaseLevel m_PrevPausePhase = PhaseLevel.Game;  // ポーズが掛かる前のフェイズ

															// Use this for initialization
	private void Start() {
		// シーンが生成されたらStart
		m_Phase = PhaseLevel.Start_Fade;
		m_ScreenChange = GetComponent<SceneChange>();
		m_AllPlayerMgr = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
		m_AllCameraMgr = GameObject.Find("AllCameraManager").GetComponent<AllCameraManager>();
		m_PauseMgr = GameObject.Find("PauseManager").GetComponent<PauseManager>();
		m_Audio = GameObject.Find("SoundManager").GetComponent<AudioList>();
		m_Score = GameObject.Find("ScoreManager").GetComponent<PrintScore>();
		m_WorldSpw = GameObject.Find("WorldHeritageSpawner").GetComponent<WorldHeritageSpawner>();
		m_CountSys = GameObject.Find("CountDownSystem").GetComponent<CountDownSystem>();
		m_TimeMgr = GameObject.Find("TimeManager").GetComponent<TimeManager>();
		m_FinishLogo = GameObject.Find("FinishLogo").GetComponent<Image>();
		m_AlertBelt = GameObject.Find("Alert_Belt").GetComponent<Image>();
		m_AlertCenter = GameObject.Find("Alert_Center").GetComponent<Image>();
		m_ScreenFade = GameObject.Find("Fade").GetComponent<Image>();
		m_ScreenAlert = GameObject.Find("Alert").GetComponent<Image>();
		// 初期は非表示
		m_CountSys.gameObject.SetActive(false);
		m_TimeMgr.gameObject.SetActive(false);
		m_FinishLogo.enabled = false;
		m_AlertBelt.enabled = false;
		m_AlertCenter.enabled = false;

		// はじめは暗転から
		GameObject.Find("Fade").GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 1f);

		// ステージ全体の長さを記録
		for (int i = 0; i < 4; i++) {
			AllStageLength += getStageLength(i);
		}

		// PlayUser表示
		for (int i = 0; i < 4; i++) {
			m_PlayUser[i] = GameObject.Find("User-" + (i + 1).ToString()).GetComponent<TextMeshProUGUI>();
		}
	}

	// Update is called once per frame
	private void Update() {
		// 現在のPhaseに合わせて処理を分ける
		switch (m_Phase) {
		//================================================================================
		// FadeStart-Phase
		//================================================================================
		case PhaseLevel.Start_Fade:
			if (timeCount <= 0f) {
				timeCount += Time.deltaTime;
				m_AllPlayerMgr.stopPlayerControl();     // プレイヤーのコントロールをOFF
				m_PauseMgr.PauseRestriction(true);  // PAUSE禁止
				m_Score.stopControll(true);
			} else {
				timeCount += Time.deltaTime;
				SetFade(1f - (timeCount / 3));
				if (timeCount >= 3f) {
					GameObject.Find("Fade").GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 0f);
					setPhaseState(PhaseLevel.Start);
					timeCount = 0f;
				}
			}

			break;
		//================================================================================
		// CountStart-Phase
		//================================================================================
		// ステージ読み込み時の処理
		case PhaseLevel.Start:
			setPhaseState(PhaseLevel.CountDown);    // シーン読み込まれたら次のカウントへ
			m_CountSys.startCountDown();
			m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_ActionCountDown);
			// 非表示要素を復活させる
			m_CountSys.gameObject.SetActive(true);
			m_TimeMgr.gameObject.SetActive(true);
			break;

		//================================================================================
		// CountDown-Phase
		//================================================================================
		// カウントダウン中の処理
		case PhaseLevel.CountDown:
			// カウントダウンのアップデートが走っている気がする
			if (!m_CountSys.getCountStatus()) {
				setPhaseState(PhaseLevel.CountEnd); // 次へ
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
			// PlayUserの表示を切る
			for (int i = 0; i < 4; i++) {
				m_PlayUser[i].enabled = false;
			}
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

			// ボーナスにいけるようになったら(移行待機)
			if (m_AllPlayerMgr.getisEntryBonus() && BonusEntryCount < BONUS_ENTRY_NUM && timeCount <= 0f) {
				timeCount += Time.deltaTime;
				m_AlertBelt.enabled = true;
				m_AlertCenter.enabled = true;
				//
				iTween.FadeTo(m_AlertBelt.gameObject, 1f, 0.5f);
				iTween.FadeTo(m_AlertCenter.gameObject, 1f, 0.5f);
				System.Collections.Hashtable hash = new System.Collections.Hashtable(){
					{"from", new Color(0,0,0,0)},
					{"to", new Color(1f,0f,0f,0.8f)},
					{"time", 1f},
					{"speed", 1f},
					{"delay", 0f},
					{"easeType",iTween.EaseType.linear},
					{"loopType",iTween.LoopType.loop},
					{"onupdate", "SetAlertScreen"},
					{"onupdatetarget", gameObject},
				};
				iTween.ValueTo(gameObject, hash);
			}

			if (timeCount > 0) {
				timeCount += Time.deltaTime;
			}

			// ここから実際の移行処理
			if (m_AllPlayerMgr.getisEntryBonusStage() && BonusEntryCount < BONUS_ENTRY_NUM) {
				// Alert停止
				iTween.Stop(gameObject, "value");
				SetAlertScreen(new Color(0, 0, 0, 0));	// 元に戻す

				timeCount = 0f;
				BonusEntryCount++;  // カウンタをプラス
				setPhaseState(PhaseLevel.Game_Bonus_Start);
				m_AlertBelt.enabled = false;
				m_AlertCenter.enabled = false;
				// XFade
				GameObject wldCamera = GameObject.Find("WorldHeritageCamera");
				wldCamera.GetComponent<XFade>().CrossFade(wldCamera, 1.0f);
			}

			break;

		//================================================================================
		// Game-Bonus-Phase-Start
		//================================================================================
		case PhaseLevel.Game_Bonus_Start:

			//BGMを変える　SE　ｽﾞｺﾞｺﾞｺﾞｺﾞ予定
			m_Audio.AllStop();
			m_Audio.Stop((int)gameBGM);
			m_Audio.Play((int)AudioList.SoundList_BGM.BGM_Game_Bonus0);
			m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_Bonus);
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
				//ｺﾞｺﾞｺﾞｺﾞｺﾞｺﾞSEをSTOP
				m_Audio.Stop((int)AudioList.SoundList_SE.SE_Bonus);
				// XFade
				GameObject.Find("MainCamera").GetComponent<XFade>().CrossFade(1.0f);
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
			if (BonusAliveCount >= BONUS_ALIVE_TIME || m_BonusFlag.is_Get) {
				setPhaseState(PhaseLevel.Game_Bonus_End);
			}

			break;
		//================================================================================
		// Game-Bonus-Phase-End
		//================================================================================
		case PhaseLevel.Game_Bonus_End:
			// XFade
			GameObject.Find("MainCamera").GetComponent<XFade>().CrossFade(1.0f);

			// ボーナスステージを削除
			m_WorldSpw.stageRelease = true;
			m_AllPlayerMgr.stopControll(false);
			// ボーナスステージのカウンタをリセット
			BonusAliveCount = 0f;

			// プレイヤーの位置をリセット場所まで戻す
			m_AllPlayerMgr.restartPlayer();
			m_AllPlayerMgr.resetBonusParameter();
			//BGMを元に戻す
			m_Audio.Stop((int)AudioList.SoundList_BGM.BGM_Game_Bonus0);
			m_Audio.Play((int)gameBGM);
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
				m_AllPlayerMgr.stopPlayerControl();
				m_Score.stopControll(true);
			} else {
				// 解除されたらPhaseを戻す
				setPhaseState(m_PrevPausePhase);
				m_TimeMgr.startTimer(); // タイマー戻す
				m_AllPlayerMgr.returnPlayerControl();
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
			timeCount += Time.deltaTime;
			if (timeCount <= 1.5f) {
				m_FinishLogo.enabled = true;
				iTween.ScaleTo(m_FinishLogo.gameObject, new Vector3(0.65f, 0.65f, 0.65f), 1.5f);
				iTween.RotateTo(m_FinishLogo.gameObject, new Vector3(0f, 0f, 720f), 1.5f);
			}

			if (timeCount > 2f && timeCount <= 3f) {
				iTween.ScaleTo(m_FinishLogo.gameObject, new Vector3(0.6f, 0.6f, 0.6f), 1.0f);
			}

			if (timeCount > 3f) {
				SetFade((timeCount - 3f) / 3);
			}

			if (timeCount >= 6f) {
				timeCount = 0f;
				setPhaseState(m_Phase + 1); // 次のPhase
			}
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
			setPhaseState(PhaseLevel.Goal); // ゴールの次
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

	/// <summary>
	/// フェード処理
	/// </summary>
	/// <param name="value"></param>
	private void SetFade(float value) {
		float v = value;
		if (v >= 1) {
			v = 1f;
		}
		m_ScreenFade.color = new Color(0, 0, 0, v);
	}

	/// <summary>
	/// アラート用の赤いフラッシュ
	/// </summary>
	/// <param name="value"></param>
	private void SetAlertScreen(Color value) {
		m_ScreenAlert.color = value;
	}
}