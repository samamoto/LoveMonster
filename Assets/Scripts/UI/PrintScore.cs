using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スコアを表示するためのスクリプト
///
/// 判定(計算)などは別の部分で行う
/// ゲームシーンとリザルトシーンで使う
/// </summary>
// 更新
// 2017/12/12 oyama ComboSystemとの連携機能をつけた
//					また、スコアを通常の加算と速度維持による徐々に増加式に

// ExecuteEvent(SendMessageの改良版)で受け取るためにInterface実装
public class PrintScore : MonoBehaviour, ScoreReciever {
	public float[] PlayerScore = new float[4];      //プレイヤーのスコアの格納用の配列 プレイヤー1～4
	public float[] ScoreRate = new float[4];     // スコアにかける倍率　コンボシステムと連動

												 // スコアの段階によって倍率が変わる	ScoreRateと倍率乗算してスコアを決める
	public float[] ScoreRateList = new float[6] {
		1f, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f,
	};

	public int graduallyFlamerate = 2;
	private float graduallyFlamerateCount = 0;
	public int graduallyScoreStandard = 5;
	private int count = 0;

	[SerializeField, MultilineAttribute(4)]
	public string text_field;

	// Use this for initialization
	private void Start() {
		//スコアの初期化,人数分繰り返し
		for (count = 0; count <= 3; count++) {
			PlayerScore[count] = 0;                 //0で初期化
			ScoreRate[count] = ScoreRateList[0];    // 基準
		}
	}

	// Update is called once per frame
	private void Update() {
		ScoreUpdate();  //スコア更新
	}

	//プレイヤースコアの受け取り
	public void ReceivePlayerScore(int id, int score) {
		//値を受け取る それぞれ(1P～4P)に用意された変数に格納 Rateを乗算する
		// idそのまま飛んでくるので-1
		PlayerScore[id - 1] += score * ScoreRate[id - 1];
	}

	//プレイヤースコアを送る(他のスクリプから値を受けとる時に使う)
	private void SendPlayerScore(int[] PlayerScore_Send) {
		//スコアの受け取り,人数分繰り返し
		for (count = 0; count <= 3; count++) {
			//値を受け取る(どこから受け取るかはまだ未定)、そしてそれぞれ(1P～4P)に用意された変数に格納
			PlayerScore[count] = PlayerScore_Send[count];
		}
	}

	//スコアの更新(表示)
	private void ScoreUpdate() {
		//うごおおおおおおおおおおお
#if DEBUG
		string str = string.Empty;
		for (int i = 0; i < 4; i++) {
			str += (i + 1).ToString() + ":" + ScoreRate[i].ToString() + " " + PlayerScore[i].ToString() + "\n";
		}
		GetComponent<Text>().text = str;    // textフィールドに表示
#endif
		// スコアが徐々に増加する方式
		graduallyAddScore();

		//DebugPrint.print(PlayerScore[0].ToString());
	}

	/// <summary>
	/// 現在のRateに合わせてスコアを徐々に加算する
	/// </summary>
	private void graduallyAddScore() {
		graduallyFlamerateCount += Time.deltaTime;
		// カウントが上回ったら
		if (graduallyFlamerate < graduallyFlamerateCount) {
			for (int i = 0; i < 4; i++) {
				PlayerScore[i] += ScoreRate[i] * (graduallyScoreStandard);
			}
		}
	}

	/// <summary>
	/// スコアレートをセットする
	/// セットするのはコンボシステム側
	/// 0：基準～5段階
	/// </summary>
	/// <param name="id">PlayerのID</param>
	/// <param name="n">スコアレート</param>
	public void setScoreRate(int id, int n) {
		// 1以上
		if (n >= 1 && n < 5) {
			ScoreRate[id - 1] = ScoreRateList[n];
		}
	}
}