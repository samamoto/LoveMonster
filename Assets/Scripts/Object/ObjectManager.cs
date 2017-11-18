using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// オブジェクトの全体を管理する
/// 〔管理するもの〕
/// ・オブジェクトの種類とどんな動作か（一回しか使えないとか）
/// ・どのプレイヤーが何のアクションオブジェクトを実行しているか
/// ・いずれかのプレイヤーに一番近いオブジェクトはなにか（場所はどこか）
/// 
/// </summary>


public class ObjectManager : MonoBehaviour {

	private static ObjectManager _instance; // インスタンス

	// 一度しか使えないオブジェクト
	public readonly string[] OnceObject = {
		"CollapseBridge",		// 崩れる橋
	};

	// 移動処理を任せるアニメーションはAllPlayerManagerにいる
	//

	

	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//================================================================================
	//関数
	//================================================================================


	/// <summary>
	/// 現在どのプレイヤーがアクションをしているか
	/// </summary>
	//public static string getPlayerAction(int n) {

	//}


	// Singleton
	//------------------------------------------------------------
	/// <summary>
	/// インスタンスの入手
	/// </summary>
	public static ObjectManager Instance {
		get {
			if (_instance == null) _instance = new ObjectManager();

			return _instance;
		}
	}
}
