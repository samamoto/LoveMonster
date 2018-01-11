using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// それぞれのUIを管理するScript
/// PlayerのUI用のScriptは表示のみ(中身の構成)を
/// 行い、値の変動はこちらで管理する。
///
/// 全体で共有している表示(現在の案だと画面上部のStartからGoalまでの道のりの表示)
/// を管理する。
///
/// </summary>

public class UIManager : MonoBehaviour
{
    private MainGameManager m_Game;
    private TimeManager m_Time;
    private CountDownSystem m_Count;

    public void Start()
    {
        m_Game = GameObject.FindWithTag("GameManager").GetComponent<MainGameManager>();
        //m_Time = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        //m_Count = GameObject.Find("CountDownSystem").GetComponent<CountDownSystem>();
    }

    public void Update()
    {
    }

    public void UIenable(bool enable)
    {
    }
}