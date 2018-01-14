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

// 2017年12月29日 oyama add
// 音声機能付加


public class PrintScore : MonoBehaviour, ScoreReciever {
	static public float[] PlayerScore = new float[4];      //プレイヤーのスコアの格納用の配列 プレイヤー1～4
	private float[] ScoreRate = new float[4];     // スコアにかける倍率　コンボシステムと連動
    private float[] ScoreRate_old = new float[4];     // スコアにかける倍率　成功UI用

	// スコアの段階によって倍率が変わる	ScoreRateと倍率乗算してスコアを決める
	private float[] ScoreRateList = new float[6] {
		0.0f,0.5f,1.0f,2f,3f,4f
	};

	private float graduallyFlamerate = 0.5f;
	private float graduallyFlamerateCount = 0;
	private int graduallyScoreStandard = 5;
	private int count = 0;
    private bool is_Stop = true;    // 初期はストップ
	private AudioList m_Audio;      // Sound 2017年12月29日 oyama add

	public int PrintTime;   //成功UIの表示時間

    private GameObject score_p1;
    private GameObject score_p2;
    private GameObject score_p3;
    private GameObject score_p4;

    private GameObject Judge_Success_1p;
    private GameObject Judge_Success_2p;
    private GameObject Judge_Success_3p;
    private GameObject Judge_Success_4p;

	private GameObject[] scoreUp = new GameObject[4];
	private Vector3[] scoreUpPos = new Vector3[4];

    public Sprite Nice;
    public Sprite Great;
    public Sprite Excellent;

	// 複数人数に対応してなさそうだから配列にする　2017年12月29日 oyama add
	private int[] Delete_count = new int[4] {
		0,0,0,0
	};

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

		for (int i = 0; i < 4; i++) {
			scoreUp[i] = GameObject.Find("ScoreUp_p" + (i + 1).ToString());
			scoreUp[i].GetComponent<TMPro.TextMeshProUGUI>().font.material.color = new Color(0,0,0,0);
			scoreUpPos[i] = scoreUp[i].transform.position;
			scoreUp[i].SetActive(false);
		}

		//Delete_count = 0;

		// 確保 2017年12月29日 oyama add
		m_Audio = GameObject.Find("SoundManager").GetComponent<AudioList>();
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
		if(ScoreRate[id-1] <= 0) {
			PlayerScore[id - 1] += score * ScoreRateList[1];
		} else {
			PlayerScore[id - 1] += score * ScoreRate[id - 1];
		}
		// 0より上なら表示する
		if (score > 0) {
			ScoreUp((int)(score * ScoreRate[id - 1]), id-1);
		}
	}

	//プレイヤースコア保管(他のスクリプトから値を受けとる時に使う)
	private void SendPlayerScore(int[] PlayerScore_Send) {
		//スコアの受け取り,人数分繰り返し
		for (count = 0; count <= 3; count++) {
			//値を受け取る(どこから受け取るかはまだ未定)、そしてそれぞれ(1P～4P)に用意された変数に格納
			PlayerScore[count] = PlayerScore_Send[count];
            
        }
	}

	// スコアアップ
	private void ScoreUp(int score, int n) {

		// 一度非アクティブにしてからアクティブにするとAnimatorが巻き戻る(と思う)
		scoreUp[n].SetActive(false);
		scoreUp[n].SetActive(true);
		scoreUp[n].GetComponent<TMPro.TextMeshProUGUI>().text = "+" + score.ToString();
		//scoreUp[n].GetComponent<Animator>().StopPlayback();
		scoreUp[n].GetComponent<Animator>().Play("ScoreCountUp", 0, 0.0f);
		scoreUp[n].transform.position = scoreUpPos[n];
		iTween.MoveBy(scoreUp[n], new Vector3(0, 8), 1.0f);
	}

	//スコアの更新(表示)
	private void ScoreUpdate() {



		string str1 = string.Empty;
        string str2= string.Empty;
        string str3 = string.Empty;
        string str4 = string.Empty;

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

		// 
		for (int n = 0; n < 4; n++) {
			if (scoreUp[n].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
				scoreUp[n].SetActive(false);

			}
		}
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
		if (n >= 0 && n <= 5) {
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

		int PlyProcess = 0;		// めんどくさいからプロセスごとにこれをインクリメントする


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
            Delete_count[PlyProcess]++;
        }
        //表示時間の制限が来たら削除
        if (Delete_count[PlyProcess] >= PrintTime)
        {
            Judge_Success_1p.SetActive(false);
            Player_success_ui_1p = false;    //描画してない
            Delete_count[PlyProcess] = 0;
        }

		// つぎのプレイヤープロセス
		PlyProcess++;	// 1

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
            Delete_count[PlyProcess]++;
        }
        //表示時間の制限が来たら削除
        if (Delete_count[PlyProcess] >= PrintTime)
        {
            Judge_Success_2p.SetActive(false);
            Player_success_ui_2p = false;    //描画してない
            Delete_count[PlyProcess] = 0;
        }

		// つぎのプロセス
		PlyProcess++;

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
            Delete_count[PlyProcess]++;
        }
        //表示時間の制限が来たら削除
        if (Delete_count[PlyProcess] >= PrintTime)
        {
            Judge_Success_3p.SetActive(false);
            Player_success_ui_3p = false;    //描画してない
            Delete_count[PlyProcess] = 0;
        }

		// つぎのプロセス
		PlyProcess++;

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
            Delete_count[PlyProcess]++;
        }
        //表示時間の制限が来たら削除
        if (Delete_count[PlyProcess] >= PrintTime)
        {
            Judge_Success_4p.SetActive(false);
            Player_success_ui_4p = false;    //描画してない
            Delete_count[PlyProcess] = 0;
        }


		// 2017年12月29日 oyama add
		// 表示されたら音を鳴らす
		// めんどくさいのでDeleteCountが1(表示し始めてから次のフレーム)のとき
		// ところでこのカウンタ配列じゃないから複数人でできない気がする
		// ...ので直した　PlyProcessとか面倒くさくて使ってます
		for(int i=0; i<4; i++) {
			if(Delete_count[i] == 1) {
				m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_ActionComboGood);
			}
		}


	}

	/// <summary>
	/// システムをストップ
	/// </summary>
	public void stopControll(bool flag){
        is_Stop = flag;
    }
}