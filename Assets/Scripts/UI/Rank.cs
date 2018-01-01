using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;   //リストを使用するため必要



/// <summary>
/// 
/// ランキングUI
/// 
/// </summary>
public class Rank : MonoBehaviour {

	private MainGameManager mgm;    //メインゲームマネージャー
	private PlayerManager pm;       //プレイヤーマネージャー
	private AllPlayerManager apm;   //オールプレイヤーマネージャー

	public Vector3[] GetPlayerPosition = new Vector3[4];  //プレイヤーの現在地を受け取るための配列
	public float MapLength = 0.0f;       //マップの長さ


	//プレイヤーの進んでいる距離の格納用の配列 プレイヤー1～4
	public int[] PlayerGoingLength = new int[4];



	//ランキング用の配列を用意(距離ソート用のやつと順位用)

	//プレイヤーの進んでいる距離の格納用の配列 プレイヤー1～4 [ランク用]
	public List<int> PlayerGoingLength_rank = new List<int>();  //リスト版

	public int[] PlayerRank = new int[4];   //順位格納用

	private bool[] RankGet_flg = new bool[4];  //順位確定フラグ

	int count = 0;    //カウント


	//順位表示用オブジェクト
	GameObject Rank_1p;
	GameObject Rank_2p;
	GameObject Rank_3p;
	GameObject Rank_4p;

	// Use this for initialization
	void Start() {
		apm = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
		mgm = GameObject.Find("GameManager").GetComponent<MainGameManager>();


		Rank_1p = GameObject.Find("Rank_p1").gameObject;      //各プレイヤーの順位表示オブジェクト
		Rank_2p = GameObject.Find("Rank_p2").gameObject;      //各プレイヤーの順位表示オブジェクト
		Rank_3p = GameObject.Find("Rank_p3").gameObject;      //各プレイヤーの順位表示オブジェクト
		Rank_4p = GameObject.Find("Rank_p4").gameObject;      //各プレイヤーの順位表示オブジェクト



		//進んでいる距離の初期化,人数分繰り返し
		for (count = 0; count <= 3; count++) {
			PlayerGoingLength[count] = 0;       //0で初期化
		}

		//プレイヤー位置の初期化,人数分繰り返し
		for (count = 0; count <= 3; count++) {
			GetPlayerPosition[count] = Vector3.zero;       //0で初期化
		}

		//ランク確定フラグ初期化
		for (count = 0; count <= 3; count++) {
			RankGet_flg[count] = false;       //0で初期化
		}

		PlayerGoingLength_rank.Clear(); //リスト初期化

		MapLength = mgm.getStageLength();     //マップの長さを取得
	}

	// Update is called once per frame
	void Update() {
		PlayerGoingLength_rank.Clear(); //リスト初期化

		//まずはプレイヤーの現在地をx,y,z全て受け取る
		for (count = 0; count <= 3; count++) {
			GetPlayerPosition[count] = apm.getPlayerPosision(count + 1);
		}


		// Z方向だと動かないのでMainGameManager側でいろいろ関数作った
		// 足りなければなんか追加してもいい
		/// mgm.float getAllStageLength()
		/// mgm.float getStageLength(int n)
		/// mgm.float getPlayerStartRange(int n, Vector3 vec)
		/// mgm.float getStageLengthToProcessRate(int n, Vector3 vec)
		/// mgm.float getAllStageLengthToProcessRate(float range)

		//プレイヤーの進んでいる距離のみを格納()
		for (count = 0; count <= 3; count++) {
			PlayerGoingLength[count] = System.Convert.ToInt32(GetPlayerPosition[count].z);   //zのみを格納(Intにキャスト)

			PlayerGoingLength_rank.Add(System.Convert.ToInt32(GetPlayerPosition[count].z));  //zのみを格納(Intにキャスト)

		}


		//ランク確定フラグ初期化
		for (count = 0; count <= 3; count++) {
			RankGet_flg[count] = false;       //0で初期化
		}

		SortLength();
		Ranking();//順位更新
		PrintRank();//順位表示

	}

	//ソート関数
	public void SortLength() {
		//4人分の距離が格納された配列の中身をソート(大きい順に)
		PlayerGoingLength_rank.Sort((a, b) => b - a);    //降順
														 //PlayerGoingLength_rank.Sort();

	}

	//順位決定関数
	public void Ranking() {

		//4人分繰り返す
		for (count = 0; count <= 3; count++) {
			//ランク用配列の中の距離とプレイヤー1の距離が同じなら(順位決定フラグがfalseなら)
			if (PlayerGoingLength_rank[count] == PlayerGoingLength[0] && RankGet_flg[0] == false) {
				PlayerRank[0] = count + 1;   //順位決定　PlayerRankの配列1つ目に順位格納
				RankGet_flg[0] = true;     //順位確定フラグをONに
			}

			//ランク用配列の中の距離とプレイヤー2の距離が同じなら(順位決定フラグがfalseなら)
			if (PlayerGoingLength_rank[count] == PlayerGoingLength[1] && RankGet_flg[1] == false) {
				PlayerRank[1] = count + 1;   //順位決定　PlayerRankの配列2つ目に順位格納
				RankGet_flg[1] = true;     //順位確定フラグをONに
			}
			//ランク用配列の中の距離とプレイヤー3の距離が同じなら(順位決定フラグがfalseなら)
			if (PlayerGoingLength_rank[count] == PlayerGoingLength[2] && RankGet_flg[2] == false) {
				PlayerRank[2] = count + 1;   //順位決定　PlayerRankの配列3つ目に順位格納
				RankGet_flg[2] = true;     //順位確定フラグをONに
			}
			//ランク用配列の中の距離とプレイヤー4の距離が同じなら(順位決定フラグがfalseなら)
			if (PlayerGoingLength_rank[count] == PlayerGoingLength[3] && RankGet_flg[3] == false) {
				PlayerRank[3] = count + 1;   //順位決定　PlayerRankの配列4つ目に順位格納
				RankGet_flg[3] = true;     //順位確定フラグをONに
			}
		}

	}

	//順位表示関数
	public void PrintRank() {
		string str;
		//str4 += ScoreRate[3].ToString() + " " + PlayerScore[3].ToString();
		//score_p4.GetComponent<TMPro.TextMeshProUGUI>().text = str4;
		//str4 = null;
		//4人分繰り返す
		for (count = 1; count <= 4; count++) {
			if (PlayerRank[0] == count) {
				str = count.ToString();
				Rank_1p.GetComponent<TMPro.TextMeshProUGUI>().text = str;
			}
			if (PlayerRank[1] == count) {
				str = count.ToString();
				Rank_2p.GetComponent<TMPro.TextMeshProUGUI>().text = str;
			}
			if (PlayerRank[2] == count) {
				str = count.ToString();
				Rank_3p.GetComponent<TMPro.TextMeshProUGUI>().text = str;
			}
			if (PlayerRank[3] == count) {
				str = count.ToString();
				Rank_4p.GetComponent<TMPro.TextMeshProUGUI>().text = str;
			}
		}
	}
}