using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObject : MonoBehaviour {

    public float limitTime;		    //　床が落下するまでの時間
    private float totalTime; 		//　主人公が床に乗っていたトータル時間
    public float deleteTime;	    //　床が落下後床を消す
    private float deletetotalTime;  //　主人公が床に乗っていたトータル時間
    private Rigidbody rigidBody;	//　リジッドボディ
    private bool touchFlag;         //  触られたことの検知

    void Start()
    {
        totalTime = 0.0f;
        deletetotalTime = 0.0f;
        rigidBody = GetComponent<Rigidbody>();
        touchFlag = false;
    }

    void Update()
    {
        //　床が落下する時間を超えたらリジッドボディのisKinematicをfalseに
        //　isKinematicをfalseにしたことで重力が働く
        if (totalTime >= limitTime)
        {
            rigidBody.isKinematic = false;

            //重力が働いてからデリートtimeを進めて　Objectを粉砕する
            if (deletetotalTime >= deleteTime)
            {
                deleteObject();
            }
            deletetotalTime += Time.deltaTime;
        }

        if (touchFlag)
        {
            ReceiveForce();
        }
    }

    //　主人公が床にぶつかった時呼ばれる
    void ReceiveForce()
    {
        //　床にリジッドボディがない場合は追加しisKinematicをtrueにする
        if (rigidBody == null)
        {
            gameObject.AddComponent<Rigidbody>();
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.isKinematic = true;
        }
        //　時間を足していく
        totalTime += Time.deltaTime;
    }

    //　主人公が床に触れたぞい
    void OnCollisionEnter()
    {
        touchFlag = true;
    }

    //  ただのデリート処理
    void deleteObject(){ Destroy(this.gameObject); }

}
