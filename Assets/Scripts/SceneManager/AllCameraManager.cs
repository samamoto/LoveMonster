using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// カメラ初期位置、オフセット値などの設定
/// 設定された値を各Cameraに投げる
/// </summary>


public class AllCameraManager : MonoBehaviour {

	private GameObject[] chaseObject = new GameObject[4];

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 4; i++) {
			string strPlayer = "Player" + (i + 1).ToString();
			chaseObject[i] = GameObject.Find(strPlayer);	// Player1~4を探す
		}
	 }
	
	 // Update is called once per frame
	 void Update () {
		
	 }

	public GameObject getChaseObject(int PlayerID) {
		return chaseObject[PlayerID - 1];	// IDそのまま来るので-1する
	}
}
