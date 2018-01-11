using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;


public class XFade : MonoBehaviour {

    private ScreenOverlay SCO;  //オーバーレイ

    private bool ScreenChange_flg=false;    //カメラ切り替え用のフラグ

	private float brendTime = 2.0f;         // 移行時間
	private float timeCount = 0f;
    // Use this for initialization
	void Start () {

        SCO = GameObject.Find("MainCamera").GetComponent<ScreenOverlay>();//オーバーレイ取得
        ScreenChange_flg = false;   //初期化
        SCO.intensity = 0.0f;          //初期化
    }
	
	// Update is called once per frame
	void Update () {


		//if (Input.GetKeyDown(KeyCode.M)){
		//    ScreenChange_flg = true;    //カメラ切り替え開始

		//}
		SCO.enabled = ScreenChange_flg;
        if (ScreenChange_flg == true)   //カメラ切り替え中
        {
			timeCount += Time.deltaTime;
			//SCO.intensity += 0.01f;      //徐々にブレンドする
			if(brendTime/2 > timeCount) {
				SCO.intensity += 1.0f / brendTime;
			} else {
				SCO.intensity -= 1.0f / brendTime;
			}
		}

        if (timeCount > brendTime)         //画面が完全にブレンドされたら
        {
            SCO.intensity = 0.0f;
			timeCount = 0f;
            ScreenChange_flg = false;   //カメラ切り替え終了
			SCO = GameObject.Find("MainCamera").GetComponent<ScreenOverlay>();//オーバーレイ取得
		}

	}

	public void CrossFade() {
		ScreenChange_flg = true;
	}

	public void CrossFade(float time) {
		brendTime = time;
		ScreenChange_flg = true;
	}

	public void CrossFade(GameObject obj, float time) {
		SCO = obj.GetComponent<ScreenOverlay>();
		brendTime = time;
		ScreenChange_flg = true;
	}

}
