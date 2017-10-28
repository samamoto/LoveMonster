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
        Vector3 workTrans = GameObject.Find(word).GetComponent<Transform>().position;
        transform.rotation = Quaternion.LookRotation(workTrans - transform.position, Vector3.up);

        //デバッグ処理　仮
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = Vector3.zero;
        }

        velocity = m_animator.GetFloat("Speed");
        m_CharacterController.Move(new Vector3(transform.forward.x * velocity * Time.deltaTime, 0.0f, transform.forward.z * velocity * Time.deltaTime));

        //コントローラの取得の仕方　仮
        //if (this.m_Controller.GetButton("A"))
        //    Debug.Log("aです");
    }
}
