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



public class Gauge : MonoBehaviour, GaugeReciever
	{
    private float GaugePointMax=100;       //マップの長さ(距離)

	//public float[] GaugePoint = new float[4];      //プレイヤーの頑張るゲージ!の格納用の配列 プレイヤー1～4
	// 仮にGauge１：Point１で組む
	public float GaugePoint;      //プレイヤーの頑張るゲージ!の格納用 プレイヤー1～4

	int count = 0;

    // Use this for initialization
    void Start()
    {
		/*
        for (count = 0; count <= 3; count++)
        {
            GaugePoint[count] = 0;       //0で初期化
        }
		*/
		GaugePoint = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

	/// <summary>
	/// ゲージの値を受け取る
	/// </summary>
	/// <param name="incRate">比率：0～1.0f</param>
	public void ReceivePlayerGauge(float incRate) {
		// Gaugeに反映させる
		GaugePoint = incRate;
	}

}
