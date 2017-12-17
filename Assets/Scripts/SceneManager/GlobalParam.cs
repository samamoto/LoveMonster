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
	private float[] hiScore = new float[ConstPlayerParameter.PlayerMax];    // Sample
	private int[] ranking = new int[ConstPlayerParameter.PlayerMax];	// ランキングを保持 0~3->1~4
	private int[] tension = new int[ConstPlayerParameter.PlayerMax];		// がんば…テンションゲージ
	private float[] mapProgress = new float[ConstPlayerParameter.PlayerMax];	// マップ進捗率
	private int[] getFlagNum = new int[ConstPlayerParameter.PlayerMax];			// ボーナスステージでゲットしたフラグの数


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
	public float[] GetHiScore() {
		return hiScore;
	}

	// HISCOREを保持する.
	public void SetHiScore(float[] hiScore) {
		this.hiScore = hiScore;
	}
	// --HISCORE-- //

	// --RANKING-- //
	/// <summary>
	/// 1PをPlayerID=1としてランキングをセットする
	/// </summary>
	/// <param name="id">PlayerのID(1から)</param>
	/// <param name="rank">ランキング番号1~4</param>
	public void SetRanking(int id, int rank) {
		int n = id;
		// エラーチェック
		if(n > 4) {
			n = 4;
		}
		if(1 > n) {
			n = 1;
		}

		ranking[n-1] = rank;	// ランキングをセット
	}

	public void SetRankings(int[] rank) {
		ranking = rank;
	}
	public int[] GetRankings() {
		return ranking;
	}

	// --Tension-- //
	/// <summary>
	/// テンションをセットする
	/// </summary>
	/// <param name="id">PlayerのID(1から)</param>
	/// <param name="rate">0以上の数値</param>
	public void SetTension(int id, int rate) {
		int n = id;
		// エラーチェック
		if (n > 4) {
			n = 4;
		}
		if (1 > n) {
			n = 1;
		}
		tension[n - 1] = rate;  // 値をセット
	}

	public void SetTensions(int[] rate) {
		tension = rate;
	}
	public int[] GetTensions() { return tension; }

	// --mapProgress-- //
	/// <summary>
	/// マップの進捗率をセットする
	/// </summary>
	/// <param name="id">PlayerのID(1から)</param>
	/// <param name="rate">0以上の数値</param>
	public void SetMapProgress(int id, float rate) {
		int n = id;
		// エラーチェック
		if (n > 4) {
			n = 4;
		}
		if (1 > n) {
			n = 1;
		}
		mapProgress[n - 1] = rate;  // 値をセット
	}

	public void SetMapProgresses(float[] rate) {
		mapProgress = rate;
	}
	public float[] GetMapProgresses() { return mapProgress; }

	// --Flag数-- //
	/// <summary>
	/// フラグ数をセットする
	/// </summary>
	/// <param name="id">PlayerのID(1から)</param>
	/// <param name="rate">0以上の数値</param>
	public void SetFlagNum(int id, int num) {
		int n = id;
		// エラーチェック
		if (n > 4) {
			n = 4;
		}
		if (1 > n) {
			n = 1;
		}
		getFlagNum[n - 1] = num;  // 値をセット
	}
	public void SetFlagNums(int[] num) {
		getFlagNum = num;
	}
	public int[] GetFlagNums() { return getFlagNum; }

	/// <summary>
	/// パラメータをすべてリセットする
	/// </summary>
	public void resetParameter() {
		for(int i=0; i<ConstPlayerParameter.PlayerMax; i++) {
			hiScore[i] = 0;
			ranking[i] = 0;
			tension[i] = 0;
			mapProgress[i] = 0f;
			getFlagNum[i] = 0;
		}
	}

}
