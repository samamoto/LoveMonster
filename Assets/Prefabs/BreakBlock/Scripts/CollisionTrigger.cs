using UnityEngine;
using System.Collections;

public class CollisionTrigger : MonoBehaviour
{

    private bool Coll = false;

    void Start()
    {
        float weight;

        foreach (Rigidbody RigidB in GameObject.FindObjectsOfType(typeof(Rigidbody)))
        {

            BoxCollider Box = RigidB.GetComponent<BoxCollider>();
            RigidB.name = RigidB.GetInstanceID().ToString();
            RigidB.Sleep();

            Box.size = Box.size / 1.5f;
            weight = Box.size.x + Box.size.y + Box.size.z;
            if (weight == 0)
            {
                Destroy(RigidB);
            }
            else
                RigidB.GetComponent<Rigidbody>().mass = weight * 10.0f;

        }
        InvokeRepeating("checkRigidBodySpeed", 0.5f, 1.0f);
    }

    void OnCollisionEnter(Collision other)
    {

        if (other.collider.tag.ToString() == "Explosion")
        {
            Dest(other.collider.gameObject.name);
            this.GetComponent<Animation>().Play();
            Coll = true;
        }
    }

    void Dest(string Obj)
    {

        Rigidbody RigidObj = GameObject.Find(Obj).GetComponent<Rigidbody>();
        if (RigidObj && Coll == true)
        {
            RigidObj.isKinematic = false;
            RigidObj.WakeUp();
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.collider.tag.ToString() == "Explosion")
            this.GetComponent<Animation>().Play();
    }

    void OnCollisionExit(Collision other)
    {
        Coll = false;
    }


}
