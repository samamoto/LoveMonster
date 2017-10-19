using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallRun : MonoBehaviour {

    //壁歩きの状態
    int m_WallRunState = -1;

    //現在位置に加算する数値
    Vector3 velocity;

	// Use this for initialization
	void Start () {
        velocity = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		switch(m_WallRunState)
        {
            ///アクションボタンが押されたが条件を満たしていない
            ///＝壁際にいない
            case -1:

                //左右移動の状態を維持
                //ToDo:コントローラに変える
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    velocity.x = -GetComponentInParent<PlayerManager>().m_SideRunSpeed;
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    velocity.x = GetComponentInParent<PlayerManager>().m_SideRunSpeed;
                }
                else
                {
                    velocity.x = 0.0f;
                }

                //壁歩き終了
                this.enabled = false;
                break;

            ///ここ以降壁歩きの処理
            ///caseの数は自由
            case 0:
                ///ここから
                Debug.Log("壁歩き開始");

                m_WallRunState = 1;
                break;
            case 1:

                break;




                ///ここまで
        }
	}


    /// <summary>
    /// enableがfalseでも呼ばれるので範囲に入ったかどうかの当たり判定として使う
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "WallRun":
                //壁歩き可能状態にする
                m_WallRunState = 0;
                break;
        }
    }

    /// <summary>
    /// 範囲から出たかどうかの当たり判定
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "WallRun":
                //壁歩き不可能状態にする
                m_WallRunState = -1;
                break;
        }
    }
}
