using UnityEngine;
using System.Collections;

public class CollisionTrigger : MonoBehaviour
{

    private bool Coll = false;

    void Start()
    {
       
    }

    //Explosionのタグに当たった時
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag.ToString() == "Explosion")
        {
            Dest(other.collider.gameObject.name);
            Coll = true;
        }
    }

    //触れられた場所を崩す
    void Dest(string Obj)
    {
        Rigidbody RigidObj = GameObject.Find(Obj).GetComponent<Rigidbody>();
        BoxCollider Box = GameObject.Find(Obj).GetComponent<BoxCollider>();
        if (RigidObj && Coll == true)
        {
            Box.isTrigger = true;
            RigidObj.isKinematic = false;
        }
    }

    //何もしていない間は処理できなくする
    void OnCollisionExit(Collision other)
    {
        Coll = false;
    }


}
