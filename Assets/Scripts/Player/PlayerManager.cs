using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class PlayerManager : MonoBehaviour {
    private AllPlayerManager m_AllPlayerManager;
    private Controller m_Controller;
    private CharacterController m_CharacterController;
    private Animator m_animator;
    private float velocity;//加速度
    //ToDo:Test
    string word = "Cube1";
    bool lookAtFlug = false;

    int time = 0;
    //重力変数
   // private Vector3 vel = Vector3.zero;
    private Vector3 JumpUp = Vector3.zero;//ジャンプ力
    private Vector3 JumpDown = Vector3.zero;//ジャンプ力

    // Use this for initialization
    void Start () {
        m_AllPlayerManager = GetComponentInParent<AllPlayerManager>();
        m_Controller = GetComponent<Controller>();
        m_CharacterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();

        //重力用データ
        Physics.gravity = new Vector3(0, 9.81f, 0);
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
        
        //なにこれ？　増田Program
        //Vector3 workTrans = GameObject.Find(word).GetComponent<Transform>().position;
        //workTrans.y = transform.position.y;


        //デバッグ処理　仮
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = Vector3.zero;
        }


        if (Input.GetKeyDown(KeyCode.Z))
        {
            time = 0;
            m_CharacterController.stepOffset = 0.9f;
            //Debug.Break();
            m_animator.SetBool("is_Jump", true);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            m_animator.SetBool("is_Slide", true);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            m_animator.SetBool("is_Climb", true);
            m_animator.SetBool("is_ClimbJump", true);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            m_animator.SetBool("is_Vault", true);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            m_animator.SetBool("is_WallRun", true);
        }

        JumpUp.y = m_animator.GetFloat("JumpPower");
        //実装
        if (lookAtFlug)
        {
            //transform.rotation = Quaternion.LookRotation(workTrans - transform.position, Vector3.up);
        }
        else
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(new Vector3(0.0f, m_AllPlayerManager.m_RotatePower, 0.0f));
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(new Vector3(0.0f, -m_AllPlayerManager.m_RotatePower, 0.0f));
            }
        }

        //移動
        if (Input.GetKey(KeyCode.UpArrow))
        {
            velocity += m_AllPlayerManager.m_RunSpeed;
            if (velocity > m_AllPlayerManager.m_MaxRunSpeed)
            {
                velocity = m_AllPlayerManager.m_MaxRunSpeed;
            }
        }
        else
        {
            velocity -= m_AllPlayerManager.m_RunSpeed;
            if (velocity <= 0.0f)
            {
                velocity = 0.0f;
            }
        }
        m_animator.SetFloat("Velocity", velocity);

        //ダッシュ
        m_CharacterController.Move(new Vector3(transform.forward.x * velocity * Time.deltaTime, 0.0f, transform.forward.z * velocity * Time.deltaTime));

        //ジャンプ
        m_CharacterController.Move(new Vector3(0, JumpUp.y * Time.deltaTime,0));

        //落下させる処理の条件
        if ( m_animator.GetCurrentAnimatorStateInfo(0).IsTag("WalkRun")||
             m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
            // 落下
            JumpDown.y -= (Physics.gravity.y * Time.fixedDeltaTime) + 0.8f;
            m_CharacterController.Move(JumpDown * Time.fixedDeltaTime);

            // 着地していたら速度を0にする
            if (m_CharacterController.isGrounded)
            {
                //Debug.Log("グラウンドオン");
                //Debug.Log(m_CharacterController.stepOffset);
                JumpDown.y = 0;
            }
        }
        if(m_animator.GetFloat("JumpPower")>=1.0f)
        {
            if(time>10)
            {
                time = 0;
                m_CharacterController.stepOffset = 0.1f;
            }else
            {

                time++;
            }
        }

            //// 落下
            //JumpDown.y -= Physics.gravity.y * Time.fixedDeltaTime;
            //m_CharacterController.Move(JumpDown * Time.fixedDeltaTime);

            //// 着地していたら速度を0にする
            //if (m_CharacterController.isGrounded)
            //{
            //    Debug.Log("グラウンドオン");
            //    JumpDown.y = 0;
            //}

        }
}

/*メモ
 * 
    コントローラの取得の仕方　仮
    if (this.m_Controller.GetButton("A"))

 */