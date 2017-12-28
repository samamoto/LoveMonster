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
	private float[] ScoreRate = new float[4];     // スコアにかける倍率　コンボシステムと連動
    private float[] ScoreRate_old = new float[4];     // スコアにかける倍率　成功UI用

	// スコアの段階によって倍率が変わる	ScoreRateと倍率乗算してスコアを決める
	private float[] ScoreRateList = new float[6] {
		1.0f,1.1f,1.2f,1.5f,2f,3f
	};

	private float graduallyFlamerate = 0.5f;
	private float graduallyFlamerateCount = 0;
	private int graduallyScoreStandard = 5;
	private int count = 0;
    private bool is_Stop = true;	// 初期はストップ

    public int PrintTime;   //成功UIの表示時間

    private GameObject score_p1;
    private GameObject score_p2;
    private GameObject score_p3;
    private GameObject score_p4;

    private GameObject Judge_Success_1p;
    private GameObject Judge_Success_2p;
    private GameObject Judge_Success_3p;
    private GameObject Judge_Success_4p;
    public Sprite Nice;
    public Sprite Great;
    public Sprite Excellent;

    private int Delete_count=0;

    private bool Player_success_ui_1p;
    private bool Player_success_ui_2p;
    private bool Player_success_ui_3p;
    private bool Player_success_ui_4p;

    [SerializeField, MultilineAttribute(4)]
	public string text_field;

	// Use this for initialization
	private void Start() {
		//スコアの初期化,人数分繰り返し
		for (count = 0; count <= 3; count++) {
			PlayerScore[count] = 0;                 //0で初期化
			ScoreRate[count] = ScoreRateList[0];    // 基準
		}

        //確保
        score_p1 = GameObject.Find("Score_p1").gameObject;
        score_p2 = GameObject.Find("Score_p2").gameObject;
        score_p3 = GameObject.Find("Score_p3").gameObject;
        score_p4 = GameObject.Find("Score_p4").gameObject;

        //確保
        Judge_Success_1p = GameObject.Find("Judge_Success_1p").gameObject;
        Judge_Success_2p = GameObject.Find("Judge_Success_2p").gameObject;
        Judge_Success_3p = GameObject.Find("Judge_Success_3p").gameObject;
        Judge_Success_4p = GameObject.Find("Judge_Success_4p").gameObject;

        //削除
        Judge_Success_1p.SetActive(false);
        Judge_Success_2p.SetActive(false);
        Judge_Success_3p.SetActive(false);
        Judge_Success_4p.SetActive(false);

        Player_success_ui_1p = false;
        Player_success_ui_2p = false;
        Player_success_ui_3p = false;
        Player_success_ui_4p = false;

        Delete_count = 0;
    }

	// Update is called once per frame
	private void Update() {
		// 外部から止められていたらアップデートしない
		if (is_Stop)
			return;
		ScoreUpdate();  //スコア更新
	}

	//プレイヤースコアの受け取り
	public void ReceivePlayerScore(int id, int score) {
		//値を受け取る それぞれ(1P～4P)に用意された変数に格納 Rateを乗算する
		// idそのまま飛んでくるので-1
		PlayerScore[id - 1] += score * ScoreRate[id - 1];
	}

	//プレイヤースコア保管(他のスクリプトから値を受けとる時に使う)
	private void SendPlayerScore(int[] PlayerScore_Send) {
		//スコアの受け取り,人数分繰り返し
		for (count = 0; count <= 3; count++) {
			//値を受け取る(どこから受け取るかはまだ未定)、そしてそれぞれ(1P～4P)に用意された変数に格納
			PlayerScore[count] = PlayerScore_Send[count];
            
        }
	}

	//スコアの更新(表示)
	private void ScoreUpdate() {



		string str1 = string.Empty;
        string str2= string.Empty;
        string str3 = string.Empty;
        string str4 = string.Empty;


        //for (int i = 0; i < 4; i++) {
        //	str += (i + 1).ToString() + ":" + ScoreRate[i].ToString() + " " + PlayerScore[i].ToString() + "\n";
        //}
        //GetComponent<Text>().text = str;    // textフィールドに表示 Todo:UI表示必要
		// ScoreRateの表示はデバッグ用なので消した
        str1 += ((int)PlayerScore[0]).ToString();
        score_p1.GetComponent<TMPro.TextMeshProUGUI>().text = str1;
        str1 =null;
        str2 += ((int)PlayerScore[1]).ToString();
        score_p2.GetComponent<TMPro.TextMeshProUGUI>().text = str2;
        str2 = null;
        str3 += ((int)PlayerScore[2]).ToString();
        score_p3.GetComponent<TMPro.TextMeshProUGUI>().text = str3;
        str3 = null;
        str4 += ((int)PlayerScore[3]).ToString();
        score_p4.GetComponent<TMPro.TextMeshProUGUI>().text = str4;
        str4 = null;


        

        // スコアが徐々に増加する方式
        graduallyAddScore();

		// GlobalParamに投げて値を保持する 2017年12月17日 oyama add
		GlobalParam.GetInstance().SetHiScore(PlayerScore);
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
		if (n >= 1 && n <= 5) {
			ScoreRate[id - 1] = ScoreRateList[n];
            
        }
        Player_Success_UI();    //成功UI表示
    }


    public void Player_Success_UI()
    {

		float[] CompareRate = new float[4];

		CompareRate[0] = ScoreRateList[0];  // Default
		CompareRate[1] = ScoreRateList[2];  // Nice
		CompareRate[2] = ScoreRateList[4];  // Great
		CompareRate[3] = ScoreRateList[5];  // Excellent



		//スコアが1.0に達した時、
		for (int count = 0; count < 4; count++)
        {
            if (ScoreRate[count] == CompareRate[0])
            {
                ScoreRate_old[count] = CompareRate[0];
            }
        }

        //スコアが1.1に達した時、
        for (int count = 0; count < 4; count++)
        {
            if (ScoreRate[count] == ScoreRateList[1])
            {
                ScoreRate_old[count] = ScoreRateList[1];
            }
        }
        //スコアが1.3に達した時、
        for (int count = 0; count < 4; count++)
        {
            if (ScoreRate[count] == ScoreRateList[3])
            {
                ScoreRate_old[count] = ScoreRateList[3];
            }
        }



        /// <summary>
        /// プレイヤー1
        /// </summary>

        //スコアがCompareRate[1]に達した時、(Nice)
        if (ScoreRate[0] == CompareRate[1] && Player_success_ui_1p == false&& ScoreRate_old[0] != CompareRate[1]
            && ScoreRate_old[0] == ScoreRateList[1])
        {
            ScoreRate_old[0] = CompareRate[1];    //oldに保存
            Judge_Success_1p.SetActive(true);
            Judge_Success_1p.GetComponent<Image>().sprite = Nice;
            Player_success_ui_1p = true;    //描画中       
        }
        //スコアがCompareRate[2]に達した時、(Great)
        if (ScoreRate[0] == CompareRate[2] && Player_success_ui_1p == false && ScoreRate_old[0] != CompareRate[2]
             && ScoreRate_old[0] == ScoreRateList[3])
        {
            ScoreRate_old[0] = CompareRate[2];    //oldに保存
            Judge_Success_1p.SetActive(true);
            Judge_Success_1p.GetComponent<Image>().sprite = Great;
            Player_success_ui_1p = true;    //描画中
        }
        //スコアがCompareRate[3]に達した時、(Excellent)
        if (ScoreRate[0] == CompareRate[3] && Player_success_ui_1p == false && ScoreRate_old[0] != CompareRate[3]
             && ScoreRate_old[0] == CompareRate[2])
        {
            ScoreRate_old[0] = CompareRate[3];    //oldに保存
            Judge_Success_1p.SetActive(true);
            Judge_Success_1p.GetComponent<Image>().sprite = Excellent;
            Player_success_ui_1p = true;    //描画中
        }

        //表示中ならカウントする
        if (Player_success_ui_1p == true)
        {
            Delete_count++;
        }
        //表示時間の制限が来たら削除
        if (Delete_count >= PrintTime)
        {
            Judge_Success_1p.SetActive(false);
            Player_success_ui_1p = false;    //描画してない
            Delete_count = 0;
        }


        /// <summary>
        /// プレイヤー2
        /// </summary>

        //スコアがCompareRate[1]に達した時、(Nice)
        if (ScoreRate[1] == CompareRate[1] && Player_success_ui_2p == false && ScoreRate_old[1] != CompareRate[1]
            && ScoreRate_old[1] == ScoreRateList[1])
        {
            ScoreRate_old[1] = CompareRate[1];    //oldに保存
            Judge_Success_2p.SetActive(true);
            Judge_Success_2p.GetComponent<Image>().sprite = Nice;
            Player_success_ui_2p = true;    //描画中       
        }
        //スコアがCompareRate[2]に達した時、(Great)
        if (ScoreRate[1] == CompareRate[2] && Player_success_ui_2p == false && ScoreRate_old[1] != CompareRate[2]
             && ScoreRate_old[1] == ScoreRateList[3])
        {
            ScoreRate_old[1] = CompareRate[2];    //oldに保存
            Judge_Success_2p.SetActive(true);
            Judge_Success_2p.GetComponent<Image>().sprite = Great;
            Player_success_ui_2p = true;    //描画中
        }
        //スコアがCompareRate[3]に達した時、(Excellent)
        if (ScoreRate[1] == CompareRate[3] && Player_success_ui_2p == false && ScoreRate_old[1] != CompareRate[3]
             && ScoreRate_old[1] == CompareRate[2])
        {
            ScoreRate_old[1] = CompareRate[3];    //oldに保存
            Judge_Success_2p.SetActive(true);
            Judge_Success_2p.GetComponent<Image>().sprite = Excellent;
            Player_success_ui_2p = true;    //描画中
        }

        //表示中ならカウントする
        if (Player_success_ui_2p == true)
        {
            Delete_count++;
        }
        //表示時間の制限が来たら削除
        if (Delete_count >= PrintTime)
        {
            Judge_Success_2p.SetActive(false);
            Player_success_ui_2p = false;    //描画してない
            Delete_count = 0;
        }

        /// <summary>
        /// プレイヤー3
        /// </summary>

        //スコアがCompareRate[1]に達した時、(Nice)
        if (ScoreRate[2] == CompareRate[1] && Player_success_ui_3p == false && ScoreRate_old[2] != CompareRate[1]
            && ScoreRate_old[2] == ScoreRateList[1])
        {
            ScoreRate_old[2] = CompareRate[1];    //oldに保存
            Judge_Success_3p.SetActive(true);
            Judge_Success_3p.GetComponent<Image>().sprite = Nice;
            Player_success_ui_3p = true;    //描画中       
        }
        //スコアがCompareRate[2]に達した時、(Great)
        if (ScoreRate[2] == CompareRate[2] && Player_success_ui_3p == false && ScoreRate_old[2] != CompareRate[2]
             && ScoreRate_old[2] == ScoreRateList[3])
        {
            ScoreRate_old[2] = CompareRate[2];    //oldに保存
            Judge_Success_3p.SetActive(true);
            Judge_Success_3p.GetComponent<Image>().sprite = Great;
            Player_success_ui_3p = true;    //描画中
        }
        //スコアがCompareRate[3]に達した時、(Excellent)
        if (ScoreRate[2] == CompareRate[3] && Player_success_ui_3p == false && ScoreRate_old[2] != CompareRate[3]
             && ScoreRate_old[2] == CompareRate[2])
        {
            ScoreRate_old[2] = CompareRate[3];    //oldに保存
            Judge_Success_3p.SetActive(true);
            Judge_Success_3p.GetComponent<Image>().sprite = Excellent;
            Player_success_ui_3p = true;    //描画中
        }

        //表示中ならカウントする
        if (Player_success_ui_3p == true)
        {
            Delete_count++;
        }
        //表示時間の制限が来たら削除
        if (Delete_count >= PrintTime)
        {
            Judge_Success_3p.SetActive(false);
            Player_success_ui_3p = false;    //描画してない
            Delete_count = 0;
        }

        /// <summary>
        /// プレイヤー4
        /// </summary>

        //スコアがCompareRate[1]に達した時、(Nice)
        if (ScoreRate[3] == CompareRate[1] && Player_success_ui_4p == false && ScoreRate_old[3] != CompareRate[1]
            && ScoreRate_old[3] == ScoreRateList[1])
        {
            ScoreRate_old[3] = CompareRate[1];    //oldに保存
            Judge_Success_4p.SetActive(true);
            Judge_Success_4p.GetComponent<Image>().sprite = Nice;
            Player_success_ui_4p = true;    //描画中       
        }
        //スコアがCompareRate[2]に達した時、(Great)
        if (ScoreRate[3] == CompareRate[2] && Player_success_ui_4p == false && ScoreRate_old[3] != CompareRate[2]
             && ScoreRate_old[3] == ScoreRateList[3])
        {
            ScoreRate_old[3] = CompareRate[2];    //oldに保存
            Judge_Success_4p.SetActive(true);
            Judge_Success_4p.GetComponent<Image>().sprite = Great;
            Player_success_ui_4p = true;    //描画中
        }
        //スコアがCompareRate[3]に達した時、(Excellent)
        if (ScoreRate[3] == CompareRate[3] && Player_success_ui_4p == false && ScoreRate_old[3] != CompareRate[3]
             && ScoreRate_old[3] == CompareRate[2])
        {
            ScoreRate_old[3] = CompareRate[3];    //oldに保存
            Judge_Success_4p.SetActive(true);
            Judge_Success_4p.GetComponent<Image>().sprite = Excellent;
            Player_success_ui_4p = true;    //描画中
        }

        //表示中ならカウントする
        if (Player_success_ui_4p == true)
        {
            Delete_count++;
        }
        //表示時間の制限が来たら削除
        if (Delete_count >= PrintTime)
        {
            Judge_Success_4p.SetActive(false);
            Player_success_ui_4p = false;    //描画してない
            Delete_count = 0;
        }





    }

    /// <summary>
    /// システムをストップ
    /// </summary>
    public void stopControll(bool flag){
        is_Stop = flag;
    }
}