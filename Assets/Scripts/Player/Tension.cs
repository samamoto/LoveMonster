using System.Collections;
using System.Collections.Generic;
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

	private float[] TensionRateMinus = new float[MAX_PHASE/2]; // 減りやすさを3段階くらいで分ける
	private float[] TensionRatePlus = new float[MAX_PHASE- MAX_PHASE / 2]; // 増えやすさを3段階くらいで分ける

	public int m_TensionPhase = 0;
	public int m_TensionCount = 0;
	private float timeCount = 0f;
	private int m_id;

	private Gauge m_Gauge;

	// Use this for initialization
	void Start () {
		m_id = GetComponent<PlayerManager>().getPlayerID();
		// GanbaruGauge_1P~4P
		m_Gauge = GameObject.Find("GanbaruGauge_" + m_id.ToString() + "P").GetComponent<Gauge>();
		for (int i = 0; i < MAX_PHASE / 2; i++) {
			TensionRateMinus[i] = 1.0f + (i * (TENSION_RATE*1.5f));    // 1.0 + 0~1*(0.5*1.5) = 1.0 ~1.75
		}
		// 増加値
		for (int i = 0; i < MAX_PHASE- MAX_PHASE/ 2; i++) {
			TensionRatePlus[i] = (TENSION_RATE*(MAX_PHASE/2+1)) - (i * TENSION_RATE);    // 0.5*(5/2+1) - 0~2*0.5 = 1.5~0.5
		}
	}

	// Update is called once per frame
	void Update () {
		countTIme();

		// ゲージに現在のコンボ比率を送る(テンション的なの)
		//DebugPrint.print(m_TensionCount);
		ExecuteEvents.Execute<GaugeReciever>(
			target: m_Gauge.gameObject,
			eventData: null,
			functor: (reciever, y) => reciever.ReceivePlayerGauge(m_id, (1.0f / (MAX_STEP)) * m_TensionCount)
		);

	}

	void countTIme() {
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
			if(timeCount >= TensionRatePlus[(m_TensionPhase) - (MAX_PHASE / 2)]){
				timeCount = 0f;
				if(m_TensionCount < MAX_STEP) {
					m_TensionCount++;
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
		if(m_TensionPhase < 0) {
			m_TensionPhase = 0;
		// 配列の範囲外エラー防止のため-1
		} else if(m_TensionPhase > MAX_PHASE-1) {
			m_TensionPhase = MAX_PHASE-1;
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

}
