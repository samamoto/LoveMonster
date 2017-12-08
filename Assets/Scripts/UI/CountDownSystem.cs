using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
/// <summary>
/// 
/// ゲーム開始後のカウントダウンシステム
/// 3…2…1…Go!
/// 
/// 
/// </summary>
public class CountDownSystem : MonoBehaviour
{

    private bool CountDown_flag = false;  //カウントダウンのフラグ
    private float Count=0;  //カウント
    
    
    // Use this for initialization
    void Start()
    {
        //CountDown_flag = true;  //カウントダウンのフラグをONに
    }

    // Update is called once per frame
    void Update()
    {
        if (CountDown_flag == true)
        //{
            Count += Time.deltaTime;
            if (Count >= 0)
            {
                GetComponent<TMPro.TextMeshProUGUI>().text = " 3";
            }
            if (Count >= 1)
            {
                GetComponent<TMPro.TextMeshProUGUI>().text = " 2";
            }
            if (Count >= 2)
            {
                GetComponent<TMPro.TextMeshProUGUI>().text = " 1";
            }
            if (Count >= 3)
            {
                GetComponent<TMPro.TextMeshProUGUI>().text = "Go!";
            }
            if (Count >= 4)
            {
                GetComponent<TMPro.TextMeshProUGUI>().text = "";
                CountDown_flag = false;
            }
    }

	/// <summary>
	/// カウントダウンを開始する
	/// </summary>
	public void startCountDown() {
		GetComponent<TMPro.TextMeshProUGUI>().enabled = true;
		CountDown_flag = true;
	}

	/// <summary>
	/// カウントダウンを停止する
	/// </summary>
	public void stopCountDown() {
		CountDown_flag = false;
		// 状態もリセットしておく
		Count = 0;
		GetComponent<TMPro.TextMeshProUGUI>().enabled = false;
	}

	/// <summary>
	/// 現在の実行状態を返す
	/// </summary>
	/// <returns>1:カウント実行中|0:待機</returns>
	public bool getCountStatus() {
		return CountDown_flag;
	}
}
