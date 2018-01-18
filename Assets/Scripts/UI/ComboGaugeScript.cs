using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ComboGaugeScript : MonoBehaviour {
    
    public int initComboLevel = 0;
    public int comboLevels = 5; //合計のコンボの段階数
	public int nowCombo = 0;
    public float comboTime = 5.0f; //コンボが一段階落ちるまでの許す時間
    private float[] gaugeFillAmounts = { 0.0f, 0.154f, 0.31f, 0.455f, 0.6f, 0.8f, 1.0f, 1.0f}; //コンボ段階に合わせたそれぞれのゲージの貯まる量
    public Image fill;
    public Image fillGlow;

	public float fillAmout = 0;
	public float nowFill = 0;
	public float MaxFillAmout = 0;
	float timeCount = 0f;

    // Use this for initialization
    void Start () {
        UpdateComboLevel(initComboLevel, comboTime);
    }
	
	// Update is called once per frame
	void Update () {

		timeCount += Time.deltaTime;

        if (fill.fillAmount > 0f && nowCombo > 0)
        {

			//     nowf 
			// ||||----|f
			nowFill = gaugeFillAmounts[nowCombo] - gaugeFillAmounts[nowCombo-1];
			float f = gaugeFillAmounts[nowCombo-1];
			float t = timeCount / comboTime;

			// f + nowFill = 現在の比率
			fillAmout = f + (nowFill - t*nowFill);
			// 
			//fillAmout -= 1.0f / (comboTime*comboLevels) * Time.deltaTime;

			fill.fillAmount = fillAmout;
			fillGlow.fillAmount = fillAmout;

		}

		if (nowCombo <= 0) {
			fill.fillAmount = fillGlow.fillAmount = 0f;
		}

	}

	//comboLevel に対してゲージの貯まる量を調整する。comboLevel　はコンボの何段階目（0-5まで）。
	public void UpdateComboLevel(int comboLevel, float time) {

		comboTime = time;
		timeCount = 0;

		int level = comboLevel;
		if (level > gaugeFillAmounts.Length-1) {
			level = gaugeFillAmounts.Length - 1;
			comboTime = (comboLevel - comboLevels) * time;
			timeCount = 0;
		}
		nowCombo = level;

		fillAmout = gaugeFillAmounts[nowCombo];
		fill.fillAmount = gaugeFillAmounts[nowCombo];
		fillGlow.fillAmount = gaugeFillAmounts[nowCombo];
	}

	public void UpdateComboLevel(int comboLevel) {
		// 現在最大量
		for (int i = 0; i < nowCombo; i++) {
			MaxFillAmout += gaugeFillAmounts[i + 1];
		}
		fill.fillAmount = gaugeFillAmounts[comboLevel];
		fillGlow.fillAmount = gaugeFillAmounts[comboLevel];
	}

	public void setComboLevel(int level) {
		nowCombo = level;
	}


}
