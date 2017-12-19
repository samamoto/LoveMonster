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

	[SerializeField]
	private float TensionRate;  // 時間経過の下降値
	const int MAX_STEP = 30;

	private float count = 0f;
	private int m_id;

	float m_Tension = 0f;
	private Gauge m_Gauge;

	// Use this for initialization
	void Start () {
		m_id = GetComponent<PlayerManager>().getPlayerID();
		// GanbaruGauge_1P~4P
		m_Gauge = GameObject.Find("GanbaruGauge_" + m_id.ToString() + "P").GetComponent<Gauge>();
		m_Tension = 0f;
	}

	// Update is called once per frame
	void Update () {
		countTIme();

		// ゲージに現在のコンボ比率を送る(テンション的なの)
		for (int i = 0; i < 4; i++) {
			ExecuteEvents.Execute<GaugeReciever>(
				target: m_Gauge.gameObject,
				eventData: null,
				functor: (reciever, y) => reciever.ReceivePlayerGauge(m_id, (1.0f / (MAX_STEP - 1)) * count)
			);
		}
	}

	void countTIme() {
		count += Time.deltaTime;
		// テンション下げる
		if(count >= TensionRate) {

		}
	}

	
	/// <summary>
	///テンションのクリア
	/// </summary>
	public void resetTension() {
		for (int i = 0; i < 4; i++) {
			m_Tension = 0f;
		}
		count = 0f;
	}


}
