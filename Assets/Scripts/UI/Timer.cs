using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
/// <summary>
/// 現在は数字を加算していくだけなので、これをまずは分,秒に直す
/// 制限時間とタイムアタック両方に対応させる。
/// 
/// 
/// タイマーのテキストを00.00.00に直す
/// (バグ)
/// 60秒経過した時に00に戻らず60になる
/// 
/// 
/// </summary>

public class Timer : MonoBehaviour {
	private int minute; //分
	private float second;   //秒
	private float second_2;   //秒　下2桁
	private int oldSecond;
	private bool timerFlag = false;
	private char textField;

	// Use this for initialization
	void Start() {

		minute = 0;
		second = 0;
		oldSecond = 0;
		
	}

	// Update is called once per frame
	void Update() {
		//countTime += Time.deltaTime; //スタートしてからの秒数を格納
		//minute = Convert.ToInt32(countTime) % 60;  //分の割り出し
		//countTime = countTime - 60 * minute;   //秒の割り出し
		//second = Convert.ToInt32(countTime);        //秒を格納



		//GetComponent<Text>().text = countTime.ToString("F2"); //小数2桁にして表示
		if (Time.timeScale > 0) {
			// カウントダウンが開始されていなければ止める
			if (!timerFlag)
				return;

			second += Time.deltaTime;
			if (second >= 60.0f) {
				minute++;   //分追加
				second = second - 60.0f;   //秒を0に戻す
										   //second_2 = second_2 - second;
			}
		}
		GetComponent<TextMeshProUGUI>().outlineColor = new Color(1f, 1f, 0f);
		GetComponent<TextMeshProUGUI>().text = minute.ToString("00") + "." + second.ToString("F2");

	}

	/// <summary>
	/// カウントを開始する
	/// </summary>
	public void startTimer() {
		timerFlag = true;
	}

	/// <summary>
	/// カウントを一時停止する
	/// </summary>
	public void stopTimer() {
		timerFlag = false;
	}

	/// <summary>
	/// カウントをリセットする
	/// </summary>
	public void resetTimer() {
		timerFlag = false;
		minute = 0;
		second = 0;
		oldSecond = 0;
	}

	public void setTimer(int time) {
		second = (float)time;
	}

	/// <summary>
	/// 現在時間を返す
	/// </summary>
	/// <returns>表示時間の文字列</returns>
	public int getTime() {
		int s = Convert.ToInt32(second) + minute*60;
		return s;
	}

	/// <summary>
	/// 現在時間を文字列で返す
	/// </summary>
	/// <returns>表示時間の文字列</returns>
	public string getTimeString() {
		string s = minute.ToString("00") + ":" + Convert.ToInt32(second).ToString("00");
#if DEBUG
		//DebugPrint.print(s);
#endif
		return s;
	}

}
