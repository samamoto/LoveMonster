using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Animator m_animator;
    private CharacterController m_characterController;
    private AnimatorStateInfo m_animatorStateInfo;
    public float work;
    enum STATUS
    {
        NONE =-1,
        INIT,
        JUMP,
        END
    }
    STATUS State=STATUS.NONE;

    private void Start()
    {
        Debug.Log("start");
        m_animator = GetComponent<Animator>();
        m_characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        switch(State)
        {

        }

        if (!m_animator.GetBool("is_jumping"))
            m_animator.SetBool("is_jumping", true);


        m_animatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);



        if (m_animatorStateInfo.IsName("Locomotion.Jump"))
        {
            if(m_animator.GetBool("is_jumping"))
            {
            }

            work = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            //アニメーション終了時の処理
            if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f/*1.0f*/)
            {
                m_animator.SetBool("is_jumping", false);
                enabled = false;
            }
        }


    }

    private void OnEnable()
    {
        Debug.Log("enable");

    }

    //int time = 0;
    //int state;
    //Vector3 velocity;
    //AllPlayerManager playerManager;
    //// Use this for initialization
    //void Start() {
    //    playerManager = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
    //    state = 0;
    //    //enabled = false;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    switch (state)
    //    {
    //        case 0:
    //            time = 0;
    //            velocity = Vector3.zero;
    //            velocity.z = 0.1f;
    //            state = 1;
    //            break;

    //        case 1:
    //            velocity.y += playerManager.m_JumpPower * Time.deltaTime;
    //            if (time > playerManager.m_JumpTime)
    //            {
    //                state = 2;
    //            }
    //            break;
    //    }
    //    time++;
    //    transform.position += velocity;
    //}

    ////private void LateUpdate()
    ////{
    ////    Debug.Log("<color=blue>b</color>");
    ////}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (enabled)
    //    {
    //        switch (collision.collider.tag)
    //        {
    //            case "Ground":
    //                if (state == 2)
    //                {
    //                    state = 0;
    //                    enabled = false;
    //                }
    //                break;
    //            case "Block":
    //                if (state == 2)
    //                {
    //                    state = 0;
    //                    enabled = false;
    //                }
    //                break;
    //            default:
    //                Debug.Log("other collision enter");
    //                break;
    //        }
    //    }
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (enabled)
    //    {
    //        switch (collision.collider.tag)
    //        {
    //            case "Block":
    //                if (state == 2)
    //                {
    //                    state = 0;
    //                    enabled = false;
    //                }
    //                break;
    //            case "Ground":
    //                break;
    //            default:
    //                Debug.Log("other collision stay");
    //                break;
    //        }
    //    }
    //}

}
