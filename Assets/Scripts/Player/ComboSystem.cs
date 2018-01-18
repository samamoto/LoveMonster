﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ・プレイヤーのスピードアップはオブジェクトのアクションを成功させたコンボ数
/// ・コンボ数は上がり続けるがスピードアップ倍率は1.0f ~ 1.5の6段階
/// ・コンボが上がらないまま5秒間経過するとそこから1秒づつ倍率は下がっていく
/// ・Multiplyの引数は移動速度が入ってくるのでそこにコンボ数に応じて積算し返す。
/// ・テンションとコンボ数は別枠で扱う
/// ・コンボ数が上がっていくとエフェクト再生でスピードアップ感を演出する
/// ・テンションゲージは別枠で管理する
/// ・
/// </summary>

public class ComboSystem : MonoBehaviour
{
    public const float MAX_TIME = 4f;
    public const int MAX_POWER = 11;
    public const float SPD_UP_RATE = 0.03f;
    private float[] multiList;

    private float downTime; //５秒経過用タイム
    private float cntTime;  //１秒経過用タイム

    public int power;
	private int power_old;
    public int cntCombo { get; private set; }
    [SerializeField] private float mulMove; //移動速度に掛ける量を表示するだけ
    [SerializeField] private float mulAnim; //アニメーションに掛ける量を表示するだけ
    private int m_id;
	private int rollNum = 0;		// 受け身でのコンボ継続数　何度も続けると待ち時間が減る
    private float m_MoveSpeed;
    private float m_AnimSpeed;
	private bool is_StopControll = false;

    // 参照
    private PrintScore m_Score;
    private ThirdPersonCharacter m_TPerson;
	private Gauge m_Gauge;
	private ComboGaugeScript m_ComboGauge;

	private Tension m_Tension;
	private ActionTrailManager m_EffTrail;
	private EffectSpeedUp m_EffSpeed;

	private AudioList m_Audio;

	// Use this for initialization
	private void Start()
    {
        multiList = new float[MAX_POWER];
        for (int i = 0; i < MAX_POWER; i++)
        {
            multiList[i] = 1.0f + (i * SPD_UP_RATE);
        }

        m_Score = GameObject.Find("ScoreManager").GetComponent<PrintScore>();
        m_id = GetComponent<PlayerManager>().getPlayerID();
        m_TPerson = GetComponent<ThirdPersonCharacter>();
        m_MoveSpeed = m_TPerson.getMoveSpeed();
        m_AnimSpeed = m_TPerson.getAnimSpeed();
		// GanbaruGauge_1P~4P
		m_Gauge = GameObject.Find("GanbaruGauge_" + m_id.ToString() + "P").GetComponent<Gauge>();
		m_ComboGauge = GameObject.Find("ComboGauge" + m_id.ToString()).GetComponent<ComboGaugeScript>();
		m_Tension = GetComponent<Tension>();    // 2017年12月20日 oyama add
		m_EffSpeed = GetComponentInChildren<EffectSpeedUp>();		// スピードアップ系の
		m_EffTrail = GetComponentInChildren<ActionTrailManager>();  // コンポーネント
		m_Audio = GameObject.Find("SoundManager").GetComponent<AudioList>();
		Init();
    }

    //初期化
    private void Init()
    {
        power = 0;
        cntCombo = 0;
        cntTime = 0;
        downTime = MAX_TIME;
		rollNum = 0;
	}

    // Update is called once per frame
    private void Update()
    {
		if (is_StopControll) return;

        //コンボが０の時処理しねえ
        if (cntCombo == 0) {
			m_Score.setScoreRate(m_id, power);    // PrintScoreに現在のプレイヤーのスコアレートを設定
			m_TPerson.setMoveSpeed(m_MoveSpeed * multiList[power]);
			mulMove = m_MoveSpeed * multiList[power];
			m_TPerson.setAnimSpeed(m_AnimSpeed * ((multiList[power] - 1) * 0.5f + 1.0f));// ちょっとだけモーションも速くする
			mulAnim = m_AnimSpeed * ((multiList[power] - 1) * 0.5f + 1.0f);
			m_Tension.updateTensionPhase(power);
			m_EffTrail.setActive(false, gameObject);
			m_EffSpeed.setActive(false, gameObject);
			return;
		}


        if (downTime <= 0)
        {
            //0以下になったら
            DownPower();
		} else {
            //タイムを進める
            CountTime();
        }

        // PrintScore(ScoreManager代わり)の更新
        m_Score.setScoreRate(m_id, power);    // PrintScoreに現在のプレイヤーのスコアレートを設定
                                              // スピードアップ処理
        if (power < MAX_POWER)
        {
            m_TPerson.setMoveSpeed(m_MoveSpeed * multiList[power]);
            mulMove = m_MoveSpeed * multiList[power];
            m_TPerson.setAnimSpeed(m_AnimSpeed * ((multiList[power] - 1) * 0.5f + 1.0f));// ちょっとだけモーションも速くする
            mulAnim = m_AnimSpeed * ((multiList[power] - 1) * 0.5f + 1.0f);
        }

		// コンボが上がってきているほど演出を派手にする
		// 1:スピードアップエフェクト
		// 2:軌跡を出す
		// powerの状態が変わったら
		if (power_old != power) {
			if (power >= MAX_POWER) {
				// MAX
				m_EffTrail.setActive(true, gameObject);
				m_EffSpeed.setActive(true, gameObject);
				// 音声
				m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_ActionPowup);

			} else if (power > 5) {
				m_EffTrail.setActive(false, gameObject);
				m_EffSpeed.setActive(true, gameObject);
				// 段階が切り替わるときだけ音を入れる
				if(power_old < power && power == 6) {
					// 音声
					m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_ActionComboGood);
				} else if (power_old > power && power == MAX_POWER -1) {
					m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_ActionComboBad);
				}
			} else {
				//　戻す
				m_EffTrail.setActive(false, gameObject);
				m_EffSpeed.setActive(false, gameObject);
				if ((power_old > power && power == 5) || power == 0) {
					m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_ActionComboBad);
				}
			}
		}
		// テンションに現在のコンボ数を送る
		// 向こう側でコンボが多いほど溜まる
		m_Tension.updateTensionPhase(power);

		//	Tensionから送るようにした　2017年12月21日 oyama add
		/*
        // ゲージに現在のコンボ比率を送る(テンション的なの)
        ExecuteEvents.Execute<GaugeReciever>(
            target: m_Gauge.gameObject,
            eventData: null,
            functor: (reciever, y) => reciever.ReceivePlayerGauge(m_id,(1.0f / (MAX_POWER-1)) * power)
        );
		*/
		// 以前のpowerを記録
		power_old = power;
	}

	//タイムの初期化
	private void TimeReset()
    { downTime = 0; }

    //時間を進める
    private void CountTime()
    { downTime -= Time.deltaTime; }

    //時間がたつと倍率が減る
    private void DownPower()
    {
        //１秒づつパワーを下げる
        cntTime += Time.deltaTime;
		rollNum = 0;
		if (cntTime >= 1f)
        {
			cntTime = 0;
			//上限用
			if (power <= 0)
            {
                power = 0;
			} else
            {
				power--;
			}
			m_ComboGauge.UpdateComboLevel(power, 1.0f);
		}
	}

    //コンボするごとに１回呼ぶ
    public void _AddCombo()
    {
        //タイムの初期化
        downTime = MAX_TIME;
        cntTime = 0;
        cntCombo++;                     //コンボカウントを進める
		rollNum = 0;
		//上限用
		if (power >= MAX_POWER)
        {
            power = MAX_POWER;
		} else
        {
            power++;
		}
		m_ComboGauge.UpdateComboLevel(power, downTime);
	}

	/// <summary>
	/// コンボ継続
	/// </summary>
	public void _ContinueCombo() {
		//タイムの初期化
		rollNum++;
		if(rollNum >= MAX_TIME - 1) {
			rollNum = (int)MAX_TIME - 1;
		}
		downTime = MAX_TIME- rollNum;
		cntTime = 0;

		m_ComboGauge.UpdateComboLevel(power, downTime);
	}

	//コンボのカウント初期化
	public void _ClearCombo() {
		Init();
	}

	//入力にList[0~5] を積算して返す
	public float _Multiply(float speed)
    {
        return speed * multiList[power];
    }

    /// <summary>
    ///	現在のコンボ数(パワー)を返す
    /// </summary>
    public int getComboNum()
    {
        return power;
    }

	/// <summary>
	/// 外部から動作を停止させる
	/// </summary>
	/// <param name="flag">解除/停止</param>
	public void stopControll(bool flag) {
		is_StopControll = flag;
	}

}