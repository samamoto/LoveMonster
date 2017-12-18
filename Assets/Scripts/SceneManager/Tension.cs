using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

	float timeCount = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// カウントを進める
	/// </summary>
	void countTime() { timeCount += Time.deltaTime; }
}
