using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldHeritageName : MonoBehaviour
{
    //開始位置
    private Vector3 m_start = new Vector3(1365, -380, 0);

    //終了位置
    private Vector3 m_end = new Vector3(615, -380, 0);

    //待機時間
    private float m_delayTime = 0.0f;

    //移動時間
    private float m_MoveTime = 1.0f;

    private RectTransform m_RectTrans;

    private float waitingTime;
    public float normalizeTime { get; private set; }
    public bool endflg { get; private set; }

    ///スプライトデータ(ここに直接入れ込めば変わるはず...)
    private Sprite m_ImageSprite
    {
        get { return this.GetComponent<Image>().sprite; }
        set { this.GetComponent<Image>().sprite = value; }
    }

    private void Start()
    {
        this.m_RectTrans = this.GetComponent<RectTransform>();
        this.endflg = true;
    }

    private void Update()
    {
        //移動開始待機
        if (!endflg && this.waitingTime >= this.m_delayTime)
        {
            //移動
            if (normalizeTime < 1.0f)
            {
                this.normalizeTime += Mathf.Clamp01(Time.deltaTime / this.m_MoveTime);
                this.m_RectTrans.localPosition = Vector3.Lerp(this.m_start, this.m_end, this.normalizeTime);
            }
            else
            {
                endflg = true;
            }
        }
        else
        {
            this.waitingTime += Time.deltaTime;
        }
    }

    public void Init(float delayTime, float moveTime, Sprite nameSprite)
    {
        this.m_RectTrans.position = this.m_start;
        this.normalizeTime = 0.0f;
        this.waitingTime = 0.0f;
        this.m_MoveTime = moveTime;
        this.m_delayTime = delayTime;
        this.m_ImageSprite = nameSprite;
        this.endflg = false;
    }
}