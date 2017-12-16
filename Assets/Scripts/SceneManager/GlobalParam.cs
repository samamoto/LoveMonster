using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----------------------------------------------------------------------------
// ゲーム内情報保持(GlobalParamはUnity実行開始後に保持し続ける領域).
//  - 保持する内容(HiScoreのサンプル)
//  - 保持する内容(未定)
// ----------------------------------------------------------------------------
public class GlobalParam : MonoBehaviour {

	// 保持する変数 //
	private int hiScore = 0;    // Sample
	private int[] ranking = new int[ConstPlayerParameter.PlayerMax];	// ランキングを保持 0~3->1~4
	// ---------- //

	private static GlobalParam instance = null; // インスタンス

	// GlobalParamは一度だけ作成してインスタンスを返す.
	// (作成後は作成済みのインスタンスを返す).
	public static GlobalParam GetInstance() {
		if (instance == null) {
			GameObject globalParam = new GameObject("GlobalParam");
			instance = globalParam.AddComponent<GlobalParam>();
			DontDestroyOnLoad(globalParam);
		}
		return instance;
	}

	// 保持する内容
	//------------------------------------------------------------------------
	// 以下にゲッターとセッターの両方を書いて取得できるようにする

	//-- Sample --//
	// --HISCORE-- //
	// HISCOREを返す.
	public int GetHiScore() {
		return hiScore;
	}

	// HISCOREを保持する.
	public void SetHiScore(int hiScore) {
		this.hiScore = hiScore;
	}
	// --HISCORE-- //

	// --RANKING-- //
	/// <summary>
	/// 1PをPlayerID=0としてランキングをセットする
	/// </summary>
	/// <param name="id">PlayerのID(0から)</param>
	/// <param name="rank">ランキング番号1~4</param>
	public void SetRanking(int id, int rank) {
		int n = id;
		// エラーチェック
		if(n > 3) {
			n = 3;
		}
		if(0 > n) {
			n = 0;
		}

		ranking[id] = rank;	// ランキングをセット
	}

	/// <summary>
	/// パラメータをすべてリセットする
	/// </summary>
	public void resetParameter() {
		hiScore = 0;
		for(int i=0; i<ConstPlayerParameter.PlayerMax; i++) {
			ranking[i] = 0;
		}
	}

}
