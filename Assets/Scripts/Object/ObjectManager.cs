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


	// プレイヤーのアクション判定パターン
	public enum ActionJudge {
		Bad = -1,   // NG
		Good = 0,   // 普通	
		Excellent,  // 良い
		Fantastic,  // すごい
		MAX,			// 判定する数
	};

	// 判定する境目を決める オブジェクトのスケール値と連動させる
	// オブジェクトごとに一つずつ設定すると混乱するのであくまで範囲は一個にする
	[SerializeField, Range(0.0f, 1.0f)]
	//private float[] JudgeZone = new float[(int)ActionJudge.MAX-1] { 0.2f, 0.65f};

	// 移動処理を任せるアニメーションはAllPlayerManagerにいる
	//

	//private Dictionary<int, string> PlayerActionDic = new Dictionary<int, string>();


	//Collider[] m_Col = new Collider[4];



	// 参照
	AllPlayerManager m_AllPlayerManager;




	// Use this for initialization
	void Start () {
		m_AllPlayerManager = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();  // Singletonをやめる
		/*
		for (int i=0; i<ConstPlayerParameter.PlayerMax; i++) {
			PlayerActionDic.Add(i, ConstAnimationStateTags.PlayerStateIdle);
		}
		*/

	}
	
	// Update is called once per frame
	void Update () {
		

		// アクション中か

		// 各オブジェクトの実行状態をPlayerManagerに反映させる
		// ObjectActionAreaから



	}

	//================================================================================
	//関数
	//================================================================================


	/// <summary>
	/// 現在どのプレイヤーがアクションをしているか
	/// </summary>
	//public static string getPlayerAction(int n) {

	//}


	/// <summary>
	/// 状態の登録
	/// </summary>
	public void setAction(int n, string action) {
		//PlayerActionDic[n] = action;
	}

	/// <summary>
	/// 状態の登録 第二引数無しでクリア
	/// </summary>
	public void setAction(int n) {
		//PlayerActionDic[n] = ConstAnimationStateTags.PlayerStateIdle;	// 戻す
	}

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
