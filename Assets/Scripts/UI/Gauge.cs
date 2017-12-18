using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
/// <summary>
/// プレイヤーが負けているほど溜まるゲージ?
/// 通称「頑張るゲージ!」ww
/// 
/// 
/// 
/// 
/// </summary>



public class Gauge : MonoBehaviour, GaugeReciever {
	private float GaugePointMax = 1.0f;      //ゲージの最大値

	//public float[] GaugePoint = new float[4];      //プレイヤーの頑張るゲージ!の格納用の配列 プレイヤー1～4
	// 仮にGauge１：Point１で組む
	public float GaugePoint;      //プレイヤーの頑張るゲージ!の格納用 プレイヤー1～4
	static float[] stGaugePoints = new float[4];

	int count = 0;

	GameObject Gauge_ui_1P;
	GameObject Gauge_ui_2P;
	GameObject Gauge_ui_3P;
	GameObject Gauge_ui_4P;


	// Use this for initialization
	void Start() {
		/*
        for (count = 0; count <= 3; count++)
        {
            GaugePoint[count] = 0;       //0で初期化
        }
		*/
		//初期化
		for (count = 0; count < 4; count++) {
			stGaugePoints[count] = 0.0f;
		}
		GaugePoint = 0;
		Gauge_ui_1P = GameObject.Find("Gauge_2_1P").gameObject;
		Gauge_ui_2P = GameObject.Find("Gauge_2_2P").gameObject;
		Gauge_ui_3P = GameObject.Find("Gauge_2_3P").gameObject;
		Gauge_ui_4P = GameObject.Find("Gauge_2_4P").gameObject;

	}

	// Update is called once per frame
	void Update() {
		Gauge_ui_1P.GetComponent<Image>().fillAmount = stGaugePoints[0];
		Gauge_ui_2P.GetComponent<Image>().fillAmount = stGaugePoints[1];
		Gauge_ui_3P.GetComponent<Image>().fillAmount = stGaugePoints[2];
		Gauge_ui_4P.GetComponent<Image>().fillAmount = stGaugePoints[3];
		GaugePoint = stGaugePoints[0];
		//DebugPrint.print(GaugePoint.ToString());
	}

	/// <summary>
	/// ゲージの値を受け取る
	/// </summary>
	/// <param name="id">1～4</param>
	/// <param name="incRate">比率：0～1.0f</param>
	public void ReceivePlayerGauge(int id, float incRate) {
		// Gaugeに反映させる
		if (id > 4 && id <= 0) {
			return;
		}
		stGaugePoints[id - 1] = incRate;
		//GaugePoint = incRate;
	}

}
