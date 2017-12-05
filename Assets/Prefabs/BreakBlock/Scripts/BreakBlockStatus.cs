using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FragmentのObject一つ一つにつけるScript
/// </summary>

public class BreakBlockStatus : MonoBehaviour {

    public float breakBlockTime;    //時間で落とす用
    float countTime;
    float destroyPos;
    bool onBreakTrigger;

    // Use this for initialization
    void Start () {
        countTime = 0;
        destroyPos = 50;
        onBreakTrigger = false;
    }
	
	// Update is called once per frame
	void Update () {
        Transform myTransform = this.transform;

        if (onBreakTrigger)
        {
            Dest();
        }

        if (myTransform.position.y <= -destroyPos)
        {
            deleteObject();
        }
        
    }

     /* 踏んだ合計で落とす処理
    //Explosionのタグに触れている間
    void OnCollisionStay(Collision other)
    {
        if (other.collider.tag.ToString() == "Player")
        {
            breakBlockTime += Time.deltaTime;
        }
    }
    */

    //時間が経てば自動的に落とす処理
    void Dest()
    {
        if( breakBlockTime <= countTime)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<BoxCollider>().isTrigger = true;
        }
        countTime += Time.deltaTime;
    }

    //　踏まれた時にOnにする
    public void _OnBreakTrigger(float breaktime){
        if (onBreakTrigger) { return; }
        onBreakTrigger = true;
        breakBlockTime = breaktime;
    }

    //  オブジェクトデリート
    void deleteObject() { Destroy(this.gameObject); }
}
