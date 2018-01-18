using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class CountDownTimer : MonoBehaviour {
	private int minute; //分
	private float second;   //秒
	private float second_2;   //秒　下2桁
	private int oldSecond;
	private bool timerFlag = false;
	private char textField;
	private bool countUpDownSwitch = true;
	public float CountDownLimit = 90;

	// Use this for initialization
	void Start() {

		if (CountDownLimit <= 0) {
			CountDownLimit = 45;
		}
		minute = 0;
		second = 0;
		oldSecond = 0;
		int cnt = 0;
		while ((int)CountDownLimit / 60 > 0) {
			minute += (int)CountDownLimit / 60;
			CountDownLimit -= 60f;
			cnt++;
		}
		second = CountDownLimit;
	}

	private void restart() {
		if (CountDownLimit <= 0) {
			CountDownLimit = 45;
		}
		minute = 0;
		second = 0;
		oldSecond = 0;
		int cnt = 0;
		while ((int)CountDownLimit / 60 > 0) {
			minute += (int)CountDownLimit / 60;
			CountDownLimit -= 60f;
			cnt++;
		}
		second = CountDownLimit;
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

			if (countUpDownSwitch) { 
				second -= Time.deltaTime;
				if (second <= 0.0f) {
					if (minute > 0) {
						minute--;
						second = second + 60.0f;   //秒を0に戻す
					}
				}
			}
			GetComponent<TextMeshProUGUI>().outlineColor = new Color(0f, 1f, 0.2f);
			GetComponent<TextMeshProUGUI>().text = minute.ToString("00") + "." + second.ToString("F2");

		}


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

	/// <summary>
	/// 現在時間を文字列で返す
	/// </summary>
	/// <returns>表示時間の文字列</returns>
	public string getTimeString() {
		string s = minute.ToString("00") + ":" + Convert.ToInt32(second).ToString("00");
		return s;
	}

	public void setTimer(float time) {
		CountDownLimit = time;
		restart();
	}

	public bool isCountDownEnd() {
		if (second <= 0 && minute <= 0) {
			return true;
		}
		return false;
	}

}
