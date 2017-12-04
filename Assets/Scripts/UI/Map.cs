using UnityEngine;
using System.Collections;


/// <summary>
/// プレイヤーがマップのどの辺りにいるのかを一目で把握できるようにする
/// スタートからゴールまでの距離を取得
/// 取得した距離とプレイヤーの現在の距離を比較し、どの程度進んでいるのかをマップ上に表示
/// 
/// 
/// </summary>
public class Map : MonoBehaviour
{

    private float MapLength;       //マップの長さ(距離)
    public float[] PlayerGoingLength = new float[4];      //プレイヤーの進んでいる距離の格納用の配列 プレイヤー1～4

    int count=0;    //カウント

    // Use this for initialization
    void Start()
    {
        //スコアの初期化,人数分繰り返し
        for (count = 0; count <= 3; count++)
        {
            PlayerGoingLength[count] = 0;       //0で初期化
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
