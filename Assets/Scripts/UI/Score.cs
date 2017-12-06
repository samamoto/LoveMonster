using UnityEngine;
using System.Collections;

/// <summary>
/// スコアを管理する
/// プレイヤーのそれぞれのスコアを保持(管理)
/// 
/// 
/// 
/// ToDo
/// 
/// プレイヤーの番号(ID)を引数として渡すと、そのプレイヤーのスコアが返される関数
/// 
/// 
/// 
/// /// </summary>

public class Score : MonoBehaviour
{
    
    public int[] score=new int[4];      //スコア格納用の配列 プレイヤー1～4


    // Use this for initialization
    int count = 0;

    void Start()
    {
        //スコアの初期化,人数分繰り返し
        for (count=0; count <= 3; count++)
        {
            score[count] = 0;   //0で初期化
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    //プレイヤーの番号(ID)を引数として渡すと、そのプレイヤーのスコアが返される関数
    public int ReturnScore(int PlayerID)
    { 
        return score[PlayerID];
    }
}
