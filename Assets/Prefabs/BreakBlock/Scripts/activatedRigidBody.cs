using UnityEngine;
using System.Collections;

/// <summary>
/// 最初は非アクティブからスタート
/// lodChangeが起動してから処理を開始する
/// </summary>

public class activatedRigidBody : MonoBehaviour
{
    private float countTime;            //カウント用のタイム
    public float allDestroyTime;        //全てを消す時間
    public float boxColliderSize_Up;    //コライダーのサイズ変更用

    void Start()
    {
        countTime = 0;
    }

    void Update()
    {
        if (countTime >= allDestroyTime)
        {       
            AllDestroyObject();
        }
        countTime += Time.deltaTime;
    }

    void OnEnable()
    {
        foreach (Rigidbody RigidB in this.transform.GetComponentsInChildren(typeof(Rigidbody)))
        {
            //コライダー取得
            BoxCollider Box = RigidB.GetComponent<BoxCollider>();

            //ID取得
            RigidB.name = RigidB.GetInstanceID().ToString();
            RigidB.Sleep();

            //コライダーのサイズ変更
            Box.size = Box.size * boxColliderSize_Up;

            //重さつけ
            float weight = Box.size.x + Box.size.y + Box.size.z;
            RigidB.GetComponent<Rigidbody>().mass = weight * 50.0f;

            //スクリプトを追加する
            GameObject.Find(RigidB.GetInstanceID().ToString()).AddComponent<BreakBlockStatus>();
        }
    }

    // BreakBlock自体を全て消す
    void AllDestroyObject()
    {
        GameObject.Find("BlockControl").GetComponent<BlockControl>()._BlockRevivalOn();
        GameObject parentobj = transform.parent.gameObject;
        transform.parent = null;//親から切り離す
        Destroy(parentobj);     //親削除
        Destroy(gameObject);    //自分削除
    }

}