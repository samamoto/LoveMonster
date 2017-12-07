using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 経過時間を管理するクラス
/// そのあとUIに投げる
/// </summary>

public class TimeManager : MonoBehaviour {

	//! TImer
	Timer m_Timer;

	// Use this for initialization
	void Start () {
		m_Timer = GetComponent<Timer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// カウントを開始する
	/// </summary>
	public void startTimer() {
		m_Timer.startTimer();
	}

	/// <summary>
	/// カウントを一時停止する
	/// </summary>
	public void stopTimer() {
		m_Timer.stopTimer();
	}

	/// <summary>
	/// カウントをリセット
	/// </summary>
	public void resetTimer() {
		m_Timer.resetTimer();
	}

	/// <summary>
	/// 現在時間を文字列で返す
	/// </summary>
	/// <returns>表示時間の文字列</returns>
	public string getTimeString() {
		return m_Timer.getProgressTime().minute.ToString() + ":" + m_Timer.getProgressTime().second.ToString(); 
	}

}
