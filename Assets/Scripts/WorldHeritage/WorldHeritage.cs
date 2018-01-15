using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldHeritage : MonoBehaviour
{
    [Tooltip("開始座標")] private Vector3 m_Start;
    [Tooltip("終了座標")] private Vector3 m_End;
    [Tooltip("移動時間")] private float m_MoveTime = 0.0f;

    /// <summary>
    /// 正規化時間(0.0f ~ 1.0f)
    /// </summary>
    public float normalizedTime { get; private set; }

    // Update is called once per frame
    private void Update()
    {
        //移動
        if (normalizedTime < 1.0f)
        {
            this.normalizedTime += Mathf.Clamp01(Time.deltaTime / this.m_MoveTime);
            this.transform.position = Vector3.Lerp(this.m_Start, this.m_End, this.normalizedTime);
        }
    }

    public void Init(Vector3 startPoint, Vector3 endPoint, float moveTime)
    {
        this.GetComponent<Transform>().position = this.m_Start = startPoint;
        this.m_End = endPoint;
        this.m_MoveTime = moveTime;
        this.normalizedTime = 0.0f;
    }
}