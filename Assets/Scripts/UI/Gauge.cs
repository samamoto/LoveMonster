using UnityEngine;
using System.Collections;
/// <summary>
/// プレイヤーが負けているほど溜まるゲージ?
/// 通称「頑張るゲージ!」ww
/// 
/// 
/// 
/// 
/// </summary>



public class Gauge : MonoBehaviour
{
    private float GaugePointMax=100;       //マップの長さ(距離)

    public float[] GaugePoint = new float[4];      //プレイヤーの頑張るゲージ!の格納用の配列 プレイヤー1～4

    int count = 0;

    // Use this for initialization
    void Start()
    {
        for (count = 0; count <= 3; count++)
        {
            GaugePoint[count] = 0;       //0で初期化
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
