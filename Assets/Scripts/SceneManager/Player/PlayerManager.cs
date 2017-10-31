using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class PlayerManager : MonoBehaviour
{
    private AllPlayerManager m_AllPlayerManager;
    private Controller m_Controller;
    private CharacterController m_CharacterController;
    private Animator m_animator;
    public float m_velocity;//加速度
    public float m_gravity;//重力
    public float m_jump;
    Vector3 m_Velocity;
    //ToDo:Test
    string word = "Cube1";
    bool lookAtFlug = false;

    // Use this for initialization
    void Start()
    {
        m_AllPlayerManager = GetComponentInParent<AllPlayerManager>();
        m_Controller = GetComponent<Controller>();
        m_CharacterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
        m_Velocity = Vector3.zero;
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            lookAtFlug = !lookAtFlug;
        }
        //デバッグ処理　仮
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //transform.position = new Vector3(0.0f, 10.0f, 0.0f);//重力テスト用
            transform.position = Vector3.zero;//基本
        }


        m_velocity = m_animator.GetFloat("Velocity");

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit raycastHit;

        //重力計算
        m_jump = m_animator.GetFloat("JumpPower");
        if (!Physics.Raycast(ray, out raycastHit, 0.1f))
        {
            m_gravity += Physics.gravity.y * m_AllPlayerManager.m_GravityPower * Time.deltaTime;
        }
        else
        {
            m_gravity = 0.0f;
        }


        //実装
        if (lookAtFlug)
        {
            Vector3 workTrans = GameObject.Find(word).GetComponent<Transform>().position;
            workTrans.y = transform.position.y;
            transform.rotation = Quaternion.LookRotation(workTrans - transform.position, Vector3.up);
        }
        else
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(new Vector3(0.0f, m_AllPlayerManager.m_RotateSpeed, 0.0f));
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(new Vector3(0.0f, -m_AllPlayerManager.m_RotateSpeed, 0.0f));
            }
        }

        m_Velocity = new Vector3(
            transform.forward.x * m_velocity * Time.deltaTime,
            m_jump + m_gravity * Time.deltaTime,
            transform.forward.z * m_velocity * Time.deltaTime);

        m_CharacterController.Move(m_Velocity);

        //コントローラの取得の仕方　仮
        //if (this.m_Controller.GetButton("A"))
        //    Debug.Log("aです");
    }
}
