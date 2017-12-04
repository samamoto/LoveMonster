using UnityEngine;
using System.Collections;
/// <summary>
/// スコアを表示するためのスクリプト
/// 
/// 判定(計算)などは別の部分で行う
/// ゲームシーンとリザルトシーンで使う
/// 
/// 
/// 
/// </summary>



public class PrintScore : MonoBehaviour
{
    
    private float[] PlayerScore = new float[4];      //プレイヤーのスコアの格納用の配列 プレイヤー1～4

    int count = 0;
    // Use this for initialization
    void Start()
    {
        //スコアの初期化,人数分繰り返し
        for (count = 0; count <= 3; count++)
        {
            PlayerScore[count] = 0;       //0で初期化
        }


    }

    // Update is called once per frame
    void Update()
    {
        ScoreUpdate();  //スコア更新
        
    }
    //プレイヤースコアの受け取り
    void ReceivePlayerScore()
    {
       

    }

    //プレイヤースコアを送る(他のスクリプから値を受けとる時に使う)
    void SendPlayerScore(float[] PlayerScore_Send)
    {

        //スコアの受け取り,人数分繰り返し
        for (count = 0; count <= 3; count++)
        {
            //値を受け取る(どこから受け取るかはまだ未定)、そしてそれぞれ(1P～4P)に用意された変数に格納
            PlayerScore[count] = PlayerScore_Send[count];
        }
    }

    //スコアの更新(表示)
    void ScoreUpdate()
    {
        //うごおおおおおおおおおおお
    }
}
