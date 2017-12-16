using UnityEngine;
using System.Collections;

public class CollisionTrigger : MonoBehaviour
{
    private bool coll = false;      //処理をするかしないか
    public float breakBlockTime;    //ブロックの崩れる時間を指定

    void Start()
    {

    }

    //Explosionのタグに触れている間処理をする
    void OnCollisionStay(Collision other)
    {
        if (other.collider.tag.ToString() == "Explosion")
        {
            Dest(other.collider.gameObject.name);
            coll = true;
        }
    }

    //触れている場所に対しての処理
    void Dest(string Obj)
    {
        Rigidbody RigidObj = GameObject.Find(Obj).GetComponent<Rigidbody>();
        BoxCollider Box = GameObject.Find(Obj).GetComponent<BoxCollider>();
        BreakBlockStatus  status = GameObject.Find(Obj).GetComponent<BreakBlockStatus>();

        //指定の時間を超えるとブロックを落とす処理
        if (status.breakBlockTime >= breakBlockTime)
        {
            if (RigidObj && coll == true)
            {
                Box.isTrigger = true;
                RigidObj.isKinematic = false;
            }
        }   
    }

    //何もしていない間は処理できなくする
    void OnCollisionExit(Collision other)
    {
        coll = false;
    }
}
