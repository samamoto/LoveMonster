using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    Vector3 velocity;
    Vector3 transformOld;
    new Rigidbody rigidbody;
    AllPlayerManager playerManager;
    // Use this for initialization
    void Start()
    {
        velocity = Vector3.zero;
        playerManager = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
        //playerManager = GetComponentInParent<AllPlayerManager>();
        //GameObject GM;
        //GM = gameObject.transform.parent.parent.parent.Find("AllPlayerManager").gameObject;
        //playerManager = GM.GetComponent<AllPlayerManager>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            velocity.x=-playerManager.m_SideRunSpeed;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity.x = playerManager.m_SideRunSpeed;
        }
        else
        {
            velocity.x = 0.0f;
        }
        velocity.z = playerManager.m_RunSpeed;



        rigidbody.velocity += velocity * Time.fixedDeltaTime / rigidbody.mass;


        //transformOld = transform.position;
        //velocity.z = GetComponentInParent<PlayerManager>().m_RunSpeed;


        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    velocity.x = -GetComponentInParent<PlayerManager>().m_SideRunSpeed;
        //}
        //else if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    velocity.x = GetComponentInParent<PlayerManager>().m_SideRunSpeed;
        //}else
        //{
        //    velocity.x = 0.0f;
        //}

        //transform.position += velocity;

        //壁に体当たりするのを止めようとしたけどできない
        //    Debug.Log(transform.position.z - transformOld.z);
        //    //Debug.Log(GetComponentInParent<PlayerManager>().m_RunSpeed * 99 / 100);
        //if (Mathf.Abs(transform.position.z - transformOld.z) < GetComponentInParent<PlayerManager>().m_RunSpeed * 99 / 100)
        //{
        //    transform.position = new Vector3(transform.position.x, transform.position.y, transformOld.z);
        //}

        //終了
        //this.enabled = false;
    }
    private void OnCollisionStay(Collision collision)
    {
        if(enabled)
        {
            switch(collision.collider.tag)
            {
                case "Block":

                    break;
            }
        }
    }
}
