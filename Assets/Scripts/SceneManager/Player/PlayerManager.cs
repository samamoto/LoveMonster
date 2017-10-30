using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class PlayerManager : MonoBehaviour {
    private Controller m_Controller;
    private CharacterController m_CharacterController;
    private Animator m_animator;
    private float velocity;//加速度
    //ToDo:Test
    string word = "Cube1";
    bool lookAtFlug = false;

    // Use this for initialization
    void Start () {
        m_Controller = GetComponent<Controller>();
        m_CharacterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //TLookAtのテスト
        if (Input.GetKeyDown(KeyCode.U))
        {
            word = "Cube1";
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            word = "Cube2";
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            word = "Cube3";
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            lookAtFlug = !lookAtFlug;
        }
        Vector3 workTrans = GameObject.Find(word).GetComponent<Transform>().position;
        workTrans.y = transform.position.y;
        //デバッグ処理　仮
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = Vector3.zero;
        }

        velocity = m_animator.GetFloat("Velocity");

        //実装
        if (lookAtFlug)
        {
            transform.rotation = Quaternion.LookRotation(workTrans - transform.position, Vector3.up);
        }
        else
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f));
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(new Vector3(0.0f, -1.0f, 0.0f));
            }
        }
        m_CharacterController.Move(new Vector3(transform.forward.x * velocity * Time.deltaTime, 0.0f, transform.forward.z * velocity * Time.deltaTime));

        //コントローラの取得の仕方　仮
        //if (this.m_Controller.GetButton("A"))
        //    Debug.Log("aです");
    }
}
