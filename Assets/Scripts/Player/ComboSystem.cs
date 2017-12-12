using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ・プレイヤーのスピードアップはオブジェクトのアクションを成功させたコンボ数
/// ・コンボ数は上がり続けるがスピードアップ倍率は1.0f ~ 1.5の6段階
/// ・コンボが上がらないまま5秒間経過するとそこから1秒づつ倍率は下がっていく
/// ・Multiplyの引数は移動速度が入ってくるのでそこにコンボ数に応じて積算し返す。
/// </summary>

public class ComboSystem : MonoBehaviour {

    public const float MAX_TIME = 5.0f;
    public const int   MAX_POWER = 5;
  
    private float[] multiList;

    private float downTime; //５秒経過用タイム
    private float cntTime;  //１秒経過用タイム

    public int power;
    public int cntCombo { get; private set; }
	private int m_id;

	// 参照
	private PrintScore m_Score;

    // Use this for initialization
    void Start () {
        multiList = new float[MAX_POWER];
		for(int i=0; i<MAX_POWER; i++) {
			multiList[i] = 1.0f + (i * 0.1f);
		}
		/*
        multiList[0] = 1.0f;
        multiList[1] = 1.1f;
        multiList[2] = 1.2f;
        multiList[3] = 1.3f;
        multiList[4] = 1.4f;
        multiList[5] = 1.5f;
		*/
		m_Score = GameObject.Find("ScoreManager").GetComponent<PrintScore>();
		m_id = GetComponent<PlayerManager>().getPlayerID();

		Init();
    }

    //初期化
    void Init() {
        power = 0;
        cntCombo = 0;
        cntTime = 0;
        downTime = MAX_TIME;
    }
	
	// Update is called once per frame
	void Update () {
        //コンボが０の時処理しねえ
        if (cntCombo == 0){ return; }

		// PrintScore(ScoreManager代わり)の更新

        if (downTime <= 0)
        {
            //0以下になったら
            DownPower();
        }
        else {
            //タイムを進める
            CountTime();
        }

		m_Score.setScoreRate(m_id, power);    // PrintScoreに現在のプレイヤーのスコアレートを設定

	}

	//タイムの初期化
	void TimeReset() { downTime = 0; }

    //時間を進める
    void CountTime() { downTime -= Time.deltaTime; }

    //時間がたつと倍率が減る
    void DownPower()
    {
        //１秒づつパワーを下げる
        cntTime += Time.deltaTime;
        if (cntTime >= 1)
        {
            cntTime = 0;
            
            //上限用
            if (power <= 0)
            {
                power = 0;
            }
            else{
                power--;
            }
        }
    }

    //コンボするごとに１回呼ぶ
    public void _AddCombo() {
        //タイムの初期化
        downTime = MAX_TIME;
        cntTime = 0;

        cntCombo++;                     //コンボカウントを進める
        //上限用
        if (power >= MAX_POWER)
        {
            power = MAX_POWER;
        }
        else {
            power++;
        }

	}

	//コンボのカウント初期化
	public void _ClearCombo() { Init(); }

    //入力にList[0~5] を積算して返す
    public float _Multiply(float speed) { 
        return speed * multiList[power];
    }
	/// <summary>
	///	現在のコンボ数(パワー)を返す
	/// </summary>
	public int getComboNum() {
		return power;
	}

}
