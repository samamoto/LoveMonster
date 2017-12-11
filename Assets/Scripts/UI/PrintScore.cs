using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
/// <summary>
/// スコアを表示するためのスクリプト
/// 
/// 判定(計算)などは別の部分で行う
/// ゲームシーンとリザルトシーンで使う
/// 
/// 
/// </summary>


// ExecuteEvent(SendMessageの改良版)で受け取るためにInterface実装
public class PrintScore : MonoBehaviour, ScoreReciever
{
    
    public float[] PlayerScore = new float[4];      //プレイヤーのスコアの格納用の配列 プレイヤー1～4
	public float[] ScoreRate = new float[4];	 // スコアにかける倍率　コンボシステムと連動
	// スコアの段階によって倍率が変わる	ScoreRateと倍率乗算してスコアを決める
	public float[] ScoreRateList = new float[6] {
		1f, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f,
	};

	int count = 0;
	[SerializeField, MultilineAttribute(4)]
	public string text_field;


    // Use this for initialization
    void Start()
    {
        //スコアの初期化,人数分繰り返し
        for (count = 0; count <= 3; count++)
        {
            PlayerScore[count] = 0;					//0で初期化
			ScoreRate[count] = ScoreRateList[0];	// 基準
        }

    }

    // Update is called once per frame
    void Update()
    {
        ScoreUpdate();  //スコア更新
        
    }

    //プレイヤースコアの受け取り
    public void ReceivePlayerScore(int id, int score)
    {
		//値を受け取る それぞれ(1P～4P)に用意された変数に格納 Rateを乗算する
		// idそのまま飛んでくるので-1
		PlayerScore[id-1] += score * ScoreRate[id-1];		
	}

	//プレイヤースコアを送る(他のスクリプから値を受けとる時に使う)
	void SendPlayerScore(int[] PlayerScore_Send)
    {

        //スコアの受け取り,人数分繰り返し
        for (count = 0; count <= 3; count++)
        {
            //値を受け取る(どこから受け取るかはまだ未定)、そしてそれぞれ(1P～4P)に用意された変数に格納
            PlayerScore[count] = PlayerScore_Send[count];
        }
    }

    //スコアの更新(表示)
    void ScoreUpdate()
    {
		//うごおおおおおおおおおおお
#if DEBUG
		string str = string.Empty;
		for (int i = 0; i < 4; i++) {
			str += (i + 1).ToString() + ":" + ScoreRate[i].ToString() + " " + PlayerScore[i].ToString() + "\n";
		}
		GetComponent<Text>().text = str;    // textフィールドに表示
#endif
		//DebugPrint.print(PlayerScore[0].ToString());

	}

	/// <summary>
	/// スコアレートをセットする
	/// セットするのはコンボシステム側
	/// 0：基準～5段階
	/// </summary>
	public void setScoreRate(int id, int n) {
		// 0以上
		if(n >= 0 && n <= 5) {
			ScoreRate[id] = ScoreRateList[n];
		}
	}

}
