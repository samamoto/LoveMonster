using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPlayerManager : MonoBehaviour
{

    //走る速さ
    public float m_RunSpeed = 0.2f;
    public float m_MaxRunSpeed = 5.0f;
    public float m_SideRunSpeed = 0.05f;

    //ジャンプ力
    public float m_JumpPower = 1.0f;
    //ジャンプ時間
    public float m_JumpTime = 2;

    //回転速度
    public float m_RotateSpeed=1.0f;

    //落下速度
    public float m_GravityPower = 1.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
