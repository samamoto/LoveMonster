using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;


public class XFade : MonoBehaviour {

    private ScreenOverlay SCO;  //オーバーレイ

    private bool ScreenChange_flg=false;    //カメラ切り替え用のフラグ

	private float brendTime = 2.0f;			// 移行時間
	   
    // Use this for initialization
	void Start () {

        SCO = GameObject.Find("MainCamera").GetComponent<ScreenOverlay>();//オーバーレイ取得
        ScreenChange_flg = false;   //初期化
        SCO.intensity = 0.0f;          //初期化
    }
	
	// Update is called once per frame
	void Update () {


        if (Input.GetKeyDown(KeyCode.M)){
            ScreenChange_flg = true;    //カメラ切り替え開始

        }
        if (ScreenChange_flg == true)   //カメラ切り替え中
        {
			//SCO.intensity += 0.01f;      //徐々にブレンドする
			SCO.intensity += 1.0f / brendTime;
        }

        if (SCO.intensity >= 1.0f)         //画面が完全にブレンドされたら
        {
            SCO.intensity = 1.0f;
            ScreenChange_flg = false;   //カメラ切り替え終了
        }

	}
}
