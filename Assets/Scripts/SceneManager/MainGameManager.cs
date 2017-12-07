using UnityEngine;

//RequireComponentで指定することでそのスクリプトが必須であることを示せる
[RequireComponent(typeof(PauseManager))]
[RequireComponent(typeof(TimeManager))]
public class MainGameManager : MonoBehaviour {

	//スクリプト群
	private SceneChange m_ScreenChange;
	private AllPlayerManager m_AllPlayerMgr;

	// 2017年12月07日 oyama add　
	private TimeManager m_TimeMgr;
	private PauseManager m_PauseMgr;
	private CountDownSystem m_CountSys;
	private AudioList m_Audio;

	public enum PhaseLevel {
		Start,
		CountDown,
		CountEnd,
		Game,
		Pause,
		Goal,
		Result,
		None,
	};

	public  PhaseLevel m_Phase = PhaseLevel.None;
	private PhaseLevel m_oldPhase = PhaseLevel.None;

	// Use this for initialization
	private void Start() {
		// シーンが生成されたらStart
		m_Phase = PhaseLevel.Start;
		m_ScreenChange = GetComponent<SceneChange>();
		m_AllPlayerMgr = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
		m_TimeMgr = GameObject.Find("TimeManager").GetComponent<TimeManager>();
		m_PauseMgr = GameObject.Find("PauseManager").GetComponent<PauseManager>();
		m_CountSys = GameObject.Find("CountDownSystem").GetComponent<CountDownSystem>();
		m_Audio = GameObject.Find("SoundManager").GetComponent<AudioList>();
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
			m_Audio.PlayOneShot(AudioList.SoundList_BGM.);
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
			break;

		//================================================================================
		// Game-Phase
		//================================================================================
		// メインのゲーム処理
		case PhaseLevel.Game:
			// ゲーム中にポーズ掛かったらPhase移行
			if (m_PauseMgr.getPauseState()) {
				setPhaseState(PhaseLevel.Pause);
			}
			// Pauseメニュー表示 //

			break;

		//================================================================================
		// Pause-Phase
		//================================================================================
		// ゲーム中にポーズがかかる
		case PhaseLevel.Pause:
			if (m_PauseMgr.getPauseState()) {
				m_TimeMgr.stopTimer();  // タイマー停止
			} else {
				// 解除されたらPhaseを戻す
				setPhaseState(PhaseLevel.Game);
				m_TimeMgr.startTimer(); // タイマー戻す
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
	/// StartとGoalまでのステージの長さを返す
	/// </summary>
	/// <returns>ステージの直線距離の長さ</returns>
	public float getStageLength() {
		Vector3 start, goal;
		start = GameObject.Find("StartPoint").GetComponent<Transform>().position;
		goal = GameObject.Find("GoalPoint").GetComponent<Transform>().position;
		Debug.Log(Vector3.Distance(start, goal));
		return Vector3.Distance(start, goal);
	}
}