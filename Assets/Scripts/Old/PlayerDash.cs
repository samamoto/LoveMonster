using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private Animator m_animator;
    private CharacterController m_characterController;
    private AnimatorStateInfo m_animatorStateInfo;
    private AllPlayerManager m_AllPlayerManager;
    public Camera m_Camera;

    public Vector3 velocity;
    public Vector3 velocity2;
    public Vector3 velocity3;
    public float rotate;
    public float rotate2;

   public float m_HorizontalAcceleration;
   public float m_VerticalAcceleration;

    private void Start()
    {
        velocity3 = Vector3.zero;
        velocity2 = Vector3.zero;
        rotate = 0.0f;
        m_AllPlayerManager = GetComponentInParent<AllPlayerManager>();
        m_animator = GetComponent<Animator>();
        m_characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //ToDo:スティックで移動したときの移動計算に変える
        m_HorizontalAcceleration = Input.GetAxis("Horizontal");
        m_VerticalAcceleration = Input.GetAxis("Vertical");

        /*
        y=1x=0  0

        y=0x=1  90

        y=-1x=0 180

        y=0x=-1 -90
         */



        velocity = new Vector3(m_HorizontalAcceleration, 0.0f, m_VerticalAcceleration);

        m_characterController.Move(velocity * m_AllPlayerManager.m_RunSpeed * Time.deltaTime);
        transform.Rotate(new Vector3(0.0f,rotate,0.0f));



        /*
            moveDirection = Quaternion.Euler(0, camobj.transform.localEulerAngles.y, 0) * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //移動方向をローカルからワールド空間に変換する
            moveDirection = transform.TransformDirection(moveDirection);
         */






        //停止
        if (m_animator.GetFloat("Speed") <= 0.0f)
            enabled = false;


        ////移動
        ////ToDo:test

        ////↑
        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    velocity.z = 1.0f;
        //}
        //else
        //{
        //    velocity.z = 0.0f;
        //}

        ////←→
        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    rotate = 1.0f;
        //}
        //else if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    rotate = -1.0f;
        //}
        //else
        //{
        //    rotate = 0.0f;
        //}
        //rotate2 += rotate;
        //if (rotate2 < 0)
        //{
        //    rotate2 += 360;
        //}
        //if (rotate2 > 360)
        //{
        //    rotate2 -= 360;
        //}

        ////if (Input.GetKey(KeyCode.RightArrow))
        ////{
        ////    rotate += 1.0f;
        ////}
        ////else if (Input.GetKey(KeyCode.LeftArrow))
        ////{
        ////    rotate += -1.0f;
        ////}
        ////if(rotate<0)
        ////{
        ////    rotate += 360;
        ////}
        ////if(rotate>360)
        ////{
        ////    rotate -= 360;
        ////}

        //velocity.x = Mathf.Sin(rotate2 * (Mathf.PI / 180.0f));
        //velocity.z = Mathf.Cos(rotate2 * (Mathf.PI / 180.0f));

        //m_characterController.Move(velocity * m_AllPlayerManager.m_RunSpeed * Time.deltaTime);
        //transform.Rotate(new Vector3(0.0f,rotate,0.0f));
        
    }


    //Vector3 velocity;
    //Vector3 transformOld;
    ////new Rigidbody rigidbody;
    //AllPlayerManager playerManager;
    //// Use this for initialization
    //void Start()
    //{
    //    velocity = Vector3.zero;
    //    playerManager = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
    //    //playerManager = GetComponentInParent<AllPlayerManager>();
    //    //GameObject GM;
    //    //GM = gameObject.transform.parent.parent.parent.Find("AllPlayerManager").gameObject;
    //    //playerManager = GM.GetComponent<AllPlayerManager>();
    //    //rigidbody = GetComponent<Rigidbody>();
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //    if(Input.GetKey(KeyCode.LeftArrow))
    //    {
    //        velocity.x=-playerManager.m_SideRunSpeed;
    //    }
    //    else if (Input.GetKey(KeyCode.RightArrow))
    //    {
    //        velocity.x = playerManager.m_SideRunSpeed;
    //    }
    //    else
    //    {
    //        velocity.x = 0.0f;
    //    }
    //    velocity.z = playerManager.m_RunSpeed;



    //    //rigidbody.velocity += velocity * Time.fixedDeltaTime / rigidbody.mass;


    //    //transformOld = transform.position;
    //    //velocity.z = GetComponentInParent<PlayerManager>().m_RunSpeed;


    //    //if (Input.GetKey(KeyCode.LeftArrow))
    //    //{
    //    //    velocity.x = -GetComponentInParent<PlayerManager>().m_SideRunSpeed;
    //    //}
    //    //else if (Input.GetKey(KeyCode.RightArrow))
    //    //{
    //    //    velocity.x = GetComponentInParent<PlayerManager>().m_SideRunSpeed;
    //    //}else
    //    //{
    //    //    velocity.x = 0.0f;
    //    //}

    //    //transform.position += velocity;

    //    //壁に体当たりするのを止めようとしたけどできない
    //    //    Debug.Log(transform.position.z - transformOld.z);
    //    //    //Debug.Log(GetComponentInParent<PlayerManager>().m_RunSpeed * 99 / 100);
    //    //if (Mathf.Abs(transform.position.z - transformOld.z) < GetComponentInParent<PlayerManager>().m_RunSpeed * 99 / 100)
    //    //{
    //    //    transform.position = new Vector3(transform.position.x, transform.position.y, transformOld.z);
    //    //}

    //    //終了
    //    //this.enabled = false;
    //}
    //private void OnCollisionStay(Collision collision)
    //{
    //    if(enabled)
    //    {
    //        switch(collision.collider.tag)
    //        {
    //            case "Block":

    //                break;
    //        }
    //    }
    //}
}
