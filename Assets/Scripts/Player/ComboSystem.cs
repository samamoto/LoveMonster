using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ・プレイヤーのスピードアップはオブジェクトのアクションを成功させたコンボ数
/// ・コンボ数は上がり続けるがスピードアップ倍率は1.0f ~ 1.5の6段階
/// ・コンボが上がらないまま5秒間経過するとそこから1秒づつ倍率は下がっていく
/// ・Multiplyの引数は移動速度が入ってくるのでそこにコンボ数に応じて積算し返す。
/// </summary>

public class ComboSystem : MonoBehaviour {

    public int cntCombo { get; private set; }
    public float[] multiList;

    private float downTime;

    const float MAXTIME = 5.0f;

    // Use this for initialization
    void Start () {
        multiList[0] = 1.0f;
        multiList[1] = 1.1f;
        multiList[2] = 1.2f;
        multiList[3] = 1.3f;
        multiList[4] = 1.4f;
        multiList[5] = 1.5f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void CountUpTime() { downTime += Time.deltaTime; }

    void TimeReset() { downTime = 0; }

    void AddCombo() { cntCombo++; }

    void ClearCombo() { cntCombo = 0; }

    float Multiply(Vector3 input) {
        int i = 0;


        //入力にList[0~5] を積算して返す
        return multiList[i];
    }
   
}
