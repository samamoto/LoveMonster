using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
/// <summary>
/// プレイヤーがオブジェクトに近づいた際に
/// オブジェクトの上にするべきアクションの説明を表示
/// コライダーで判定
/// 
/// ガイドの位置は現在手動設定
/// Stage/Area/Act/ObjectKongVault
/// に設定してテストした、
/// そのため現在のプレファブに汎用性は無し、つまり見本
/// 
/// (´・ω・`)ちなみにガイド判定エリア内だとジャンプ出来なくなるわ
/// (´・ω・`)なぜかしら？　出荷されてくるわ
/// 
/// </summary>
public class Billboard_Check : MonoBehaviour
{
    private GameObject Guide_print;
    // Use this for initialization
    void Start()
    {
        Guide_print= GameObject.Find("GuidePrint_vault").gameObject; //情報を保存
        Guide_print.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        //if (other.collider.tag.ToString() == "GuidePrint_vault")
        if (other.tag == "Player")
        {
            //存在させる
            Guide_print.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        //if (other.collider.tag.ToString() == "GuidePrint_vault")
        if (other.tag == "Player")
        {
           
            //存在を消す
            Guide_print.SetActive(false);
        }
    }
}