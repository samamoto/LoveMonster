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
	CountDownTimer m_cdTimer;
	float saveTime = 0f;
	bool isChange = true;

	// Use this for initialization
	void Start () {
		m_cdTimer = GetComponent<CountDownTimer>();
		m_cdTimer.enabled = false;
		m_Timer = GetComponent<Timer>();
		m_Timer.startTimer();
		changeTimer(true);
		m_Timer.stopTimer();
	}
	
	// Update is called once per frame
	void Update () {
		if (isChange)
			m_Timer.getTimeString();
		else
			m_cdTimer.getTimeString();
	}

	/// <summary>
	/// カウントを開始する
	/// </summary>
	public void startTimer() {
		if (isChange)
			m_Timer.startTimer();
		else
			m_cdTimer.startTimer();
	}

	/// <summary>
	/// カウントを一時停止する
	/// </summary>
	public void stopTimer() {
		if (isChange)
			m_Timer.stopTimer();
		else
			m_cdTimer.stopTimer();
	}

	/// <summary>
	/// カウントをリセット
	/// </summary>
	public void resetTimer() {
		if (isChange)
			m_Timer.resetTimer();
		else
			m_cdTimer.resetTimer();
	}

	public void setTimer(int time) {
		if (!isChange) {
			m_cdTimer.setTimer(time);
		}
	}

	/// <summary>
	/// カウントアップかダウンか
	/// </summary>
	/// <param name="flag"></param>
	public void changeTimer(bool flag) {
		isChange = flag;
		if (flag) {
			m_Timer.enabled = true;
			m_Timer.setTimer((int)saveTime);
			m_cdTimer.enabled = false;
		} else {
			saveTime = m_Timer.getTime();
			m_Timer.enabled = false;
			m_cdTimer.enabled = true;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public bool isCountEnd() {
		if (!isChange) {
			return m_cdTimer.isCountDownEnd();
		}
		return false;
	}

}
