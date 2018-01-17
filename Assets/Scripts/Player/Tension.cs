using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// プレイヤーのテンションを管理する
/// 1.コンボシステムのコンボ数に応じて溜まり方が変わる
/// 2.時間経過で減っていく
/// 　コンボをするたびにテンションが上がる
/// 　下降する条件は
/// 　・落ちる
/// 　・
/// 3.テンションが規定値全員溜まったらステージに移行する(テレポ式)
/// </summary>

public class Tension : MonoBehaviour {
	private const int MAX_STEP = 50;    // ステップ数
	private const int MAX_PHASE = 6;
	private const float TENSION_RATE = 0.55f;

	private float[] TensionRateMinus = new float[MAX_PHASE / 2]; // 減りやすさを3段階くらいで分ける
	private float[] TensionRatePlus = new float[MAX_PHASE - MAX_PHASE / 2]; // 増えやすさを3段階くらいで分ける

	public int m_TensionPhase = 0;
	public int m_TensionCount = 0;
	private float timeCount = 0f;
	private int m_id;
	private bool is_StopControll = false;   // 外部からの停止

	[SerializeField]
	private float TensionRatio = 0.0f;

	private Gauge m_Gauge;
	private ComboGaugeScript m_ComboGauge;
	private MainGameManager m_GameManager;
	private AudioList m_Audio;
	private bool is_EntryBonusTension_Sound;    // ボーナス移行時の時のサウンドフラグ

	// Use this for initialization
	private void Start() {
		m_id = GetComponent<PlayerManager>().getPlayerID();
		// GanbaruGauge_1P~4P
		m_Gauge = GameObject.Find("GanbaruGauge_" + m_id.ToString() + "P").GetComponent<Gauge>();
		for (int i = 0; i < MAX_PHASE / 2; i++) {
			TensionRateMinus[i] = 1.0f + (i * (TENSION_RATE * 1.5f));    // 1.0 + 0~1*(0.5*1.5) = 1.0 ~1.75
		}
		// 増加値
		for (int i = 0; i < MAX_PHASE - MAX_PHASE / 2; i++) {
			TensionRatePlus[i] = (TENSION_RATE * (MAX_PHASE / 2 + 1)) - (i * TENSION_RATE);    // 0.5*(5/2+1) - 0~2*0.5 = 1.5~0.5
		}
		m_Audio = GameObject.Find("SoundManager").GetComponent<AudioList>();
		m_GameManager = GameObject.Find("GameManager").GetComponent<MainGameManager>();
		m_ComboGauge = GameObject.Find("ComboGauge" + m_id.ToString()).GetComponent<ComboGaugeScript>();
	}

	// Update is called once per frame
	private void Update() {
		if (is_StopControll) return;
		countTIme();

		// ゲージに現在のコンボ比率を送る(テンション的なの)
		//DebugPrint.print(m_TensionCount);
		ExecuteEvents.Execute<GaugeReciever>(
			target: m_Gauge.gameObject,
			eventData: null,
			functor: (reciever, y) => reciever.ReceivePlayerGauge(m_id, (1.0f / (MAX_STEP)) * m_TensionCount)
		);
		this.TensionRatio = getTensionRatio();

		// コンボゲージに現在のPhaseを送る
		//m_ComboGauge.setComboLevel(m_TensionPhase);

	}

	private void countTIme() {
		timeCount += Time.deltaTime;
		// 下降状況か、上昇か処理を分ける
		if (m_TensionPhase < MAX_PHASE / 2) {
			// 各Phaseごとに定められた規定時間を越えたらテンション下げる
			if (timeCount >= TensionRateMinus[m_TensionPhase]) {
				timeCount = 0f;
				if (m_TensionCount > 0) {
					m_TensionCount--;
				}
			}
		} else {
			// 増加
			if (timeCount >= TensionRatePlus[(m_TensionPhase) - (MAX_PHASE / 2)]) {
				timeCount = 0f;
				if (m_TensionCount < MAX_STEP) {
					m_TensionCount++;
					// テンションのカウンタがAllPlayerManagerで指定されているパーセンテージを超えたら一度だけ鳴らす
					if (getTensionRatio() > m_GameManager.ENTRY_BONUS_TENSION && !is_EntryBonusTension_Sound) {
						is_EntryBonusTension_Sound = true;
						m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_TensionCharge);
						EffectControl.get().createBuff(gameObject, m_id);
					}
				}
			}
		}
	}

	/// <summary>
	/// 外部(コンボシステム側)からPhaseを切り替えるときに使用する
	/// </summary>
	public void updateTensionPhase(int cnt) {
		int count = cnt / 2;

		m_TensionPhase = count; // 段階分けて格納

		// countがこちらの最大値より大きい場合は修正する
		if (count > MAX_PHASE) {
			count = MAX_PHASE;
		}
		// 範囲チェック
		if (m_TensionPhase < 0) {
			m_TensionPhase = 0;
			// 配列の範囲外エラー防止のため-1
		} else if (m_TensionPhase > MAX_PHASE - 1) {
			m_TensionPhase = MAX_PHASE - 1;
		}
	}

	/// <summary>
	///テンションのクリア
	/// </summary>
	public void resetTension() {
		m_TensionPhase = 0;
		m_TensionCount = 0;
		timeCount = 0f;
	}

	/// <summary>
	/// 落下したときなど、テンションを下降させる
	/// </summary>
	public void downTension() {
		m_TensionPhase = 0;
		timeCount = 0;
	}

	/// <summary>
	/// 現在のテンションの割合を返す
	/// </summary>
	public float getTensionRatio() {
		return ((float)m_TensionCount / MAX_STEP);
	}

	/// <summary>
	/// 外部から動作を停止させる
	/// </summary>
	/// <param name="flag">解除/停止</param>
	public void stopControll(bool flag) {
		is_StopControll = flag;
	}
}