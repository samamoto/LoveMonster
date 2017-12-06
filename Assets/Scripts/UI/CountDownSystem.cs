using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
/// <summary>
/// 
/// ゲーム開始後のカウントダウンシステム
/// 3…2…1…Go!
/// 
/// 
/// </summary>
public class CountDownSystem : MonoBehaviour
{

    private bool CountDown_flag = false;  //カウントダウンのフラグ
    private float Count=0;  //カウント
    
    
    // Use this for initialization
    void Start()
    {
        CountDown_flag = true;  //カウントダウンのフラグをONに
    }

    // Update is called once per frame
    void Update()
    {
        if (CountDown_flag == true)
        //{
            Count += Time.deltaTime;
            if (Count >= 0)
            {
                GetComponent<TMPro.TextMeshProUGUI>().text = " 3";
            }
            if (Count >= 1)
            {
                GetComponent<TMPro.TextMeshProUGUI>().text = " 2";
            }
            if (Count >= 2)
            {
                GetComponent<TMPro.TextMeshProUGUI>().text = " 1";
            }
            if (Count >= 3)
            {
                GetComponent<TMPro.TextMeshProUGUI>().text = "Go!";
            }
            if (Count >= 4)
            {
                GetComponent<TMPro.TextMeshProUGUI>().text = "";
                CountDown_flag = false;
            }
    }
}
