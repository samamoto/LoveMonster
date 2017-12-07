﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 現在は数字を加算していくだけなので、これをまずは分,秒に直す
/// 制限時間とタイムアタック両方に対応させる。
/// 
/// (バグ)
/// 60秒経過した時に00に戻らず60になる
/// 
/// 
/// </summary>

public class Timer : MonoBehaviour {
    private int minute; //分
    private float second;   //秒
    private int oldSecond;
    private bool timerFlag= false;
    private char textField;
    float countTime = 0;

    // Use this for initialization
    void Start () {
        minute = 0;
        second = 0;
        oldSecond = 0;
    }
	
	// Update is called once per frame
	void Update () {
        //countTime += Time.deltaTime; //スタートしてからの秒数を格納
        //minute = Convert.ToInt32(countTime) % 60;  //分の割り出し
        //countTime = countTime - 60 * minute;   //秒の割り出し
        //second = Convert.ToInt32(countTime);        //秒を格納



        //GetComponent<Text>().text = countTime.ToString("F2"); //小数2桁にして表示
        if (Time.timeScale > 0)
        {
			// カウントダウンが開始されていなければ止める
			if (!timerFlag)
				return;

			second += Time.deltaTime;
            if (second >= 60.0f)
            {
                minute++;   //分追加
                second = second - 60;   //秒を0に戻す
            }
            if (Convert.ToInt32(second) != oldSecond)
            {
                GetComponent<Text>().text = minute.ToString("00") + ":" + Convert.ToInt32(second).ToString("00");
            }
            oldSecond = Convert.ToInt32(second);

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
#if DEBUG
		DebugPrint.print(s);
#endif
		return s;
	}
}
