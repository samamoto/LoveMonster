using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;

public partial class PlayerManager : MonoBehaviour
{
	// PlayerのID
	public int m_PlayerID = 1; // 今は一人しかいない

	private MoveState m_MoveState;	// 移動処理を任せる
	private AllPlayerManager m_AllPlayerManager;
	private Controller.Controller m_Controller;
	private CharacterController m_CharacterController;
	private Animator m_animator;
	private float velocity;//加速度
						   //ToDo:Test
						   //string word = "Cube1";
	bool lookAtFlag = false;

	int time = 0;
	//重力変数
	// private Vector3 vel = Vector3.zero;
	[SerializeField] private Vector3 JumpUp = Vector3.zero;//ジャンプ力
	[SerializeField] private Vector3 JumpDown = Vector3.zero;//ジャンプ力

	// Use this for initialization
	void Start() {
		m_AllPlayerManager = GetComponentInParent<AllPlayerManager>();
		m_Controller = GetComponent<Controller.Controller>();
		m_CharacterController = GetComponent<CharacterController>();
		m_animator = GetComponent<Animator>();
		m_MoveState = GetComponent<MoveState>();

        //重力用データ
        Physics.gravity = new Vector3(0, 9.81f, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        // MoveStateの状態確認
        if (m_MoveState.isMove())
        {
            m_MoveState.Update();   // 外部から操作を受け付け
            return;                 // なにもしない
        }

		//TLookAtのテスト
		/* Debug実装をコメントアウト 2017/11/11 oyama add
		if (Input.GetKeyDown(KeyCode.U)) {
			//word = "Cube1";
		}
		if (Input.GetKeyDown(KeyCode.I)) {
			// word = "Cube2";
		}
		if (Input.GetKeyDown(KeyCode.O)) {
			//word = "Cube3";
		}
		if (Input.GetKeyDown(KeyCode.P)) {
			lookAtFlag = !lookAtFlag;
		}
		
		//デバッグ処理　仮
		if (Input.GetKeyDown(KeyCode.Space)) {
			transform.position = Vector3.zero;
		}

		*/
		// キャラクターが地上にいる状態で指定のキーが押された場合 oyama add
		if (m_CharacterController.isGrounded) {
			if (Input.GetKeyDown(KeyCode.Space) || this.m_Controller.GetButtonDown(Controller.Button.A)) {  // 一番操作しやすいキーに 2017/11/11 oyama add
				time = 0;
				m_CharacterController.stepOffset = 0.9f;
				//Debug.Break();
				m_animator.SetBool("is_Jump", true);
			}
		}
		//Debug実装をコメントアウト 2017/11/11 oyama add
		if (Input.GetKeyDown(KeyCode.X)) {
			m_animator.SetBool("is_Slide", true);
		}
		/*
        if (Input.GetKeyDown(KeyCode.C))
        {
            m_animator.SetBool("is_Climb", true);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            m_animator.SetBool("is_Vault", true);
        }

		if (Input.GetKeyDown(KeyCode.B)) {
			m_animator.SetBool("is_WallRun", true);
		}
		*/
		JumpUp.y = m_animator.GetFloat("JumpPower");

		//実装
		if (lookAtFlag) {
			//transform.rotation = Quaternion.LookRotation(workTrans - transform.position, Vector3.up);
		} else {
			// 左右の回転処理
			if (Input.GetKey(KeyCode.RightArrow) || m_Controller.GetAxisDown(Axis.L_x) == -1) {
				transform.Rotate(new Vector3(0.0f, m_AllPlayerManager.m_RotatePower, 0.0f));
			}
			if (Input.GetKey(KeyCode.LeftArrow) || m_Controller.GetAxisDown(Axis.L_x) == 1) {
				transform.Rotate(new Vector3(0.0f, -m_AllPlayerManager.m_RotatePower, 0.0f));
			}

		}
		//移動

		if (Input.GetKey(KeyCode.UpArrow)) {
			velocity += m_AllPlayerManager.m_RunSpeed;
			if (velocity > m_AllPlayerManager.m_MaxRunSpeed) {
				velocity = m_AllPlayerManager.m_MaxRunSpeed;
			}
		} else {
			velocity -= m_AllPlayerManager.m_RunSpeed;
			if (velocity <= 0.0f) {
				velocity = 0.0f;
			}
		}
		m_animator.SetFloat("Velocity", velocity);

		//ダッシュ
		m_CharacterController.Move(new Vector3(transform.forward.x * velocity * Time.deltaTime, JumpUp.y * Time.deltaTime, transform.forward.z * velocity * Time.deltaTime));

		//ジャンプ
		//m_CharacterController.Move(new Vector3(0, JumpUp.y * Time.deltaTime, 0));	// ↑の一個で良いんじゃね

		//落下させる処理の条件
		if (!m_CharacterController.isGrounded) {
			if (m_animator.GetCurrentAnimatorStateInfo(0).IsTag("WalkRun") ||
				m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle")) {
				m_animator.SetBool("is_Fall", true);    // 落下状態に				
			}
		}

		// キャラクターが浮いていたら
		if (!m_CharacterController.isGrounded) {
			// 落下
			JumpDown.y -= (Physics.gravity.y * Time.deltaTime);
			m_CharacterController.Move(JumpDown * Time.deltaTime);

			// 着地していたら速度を0にする
			if (m_CharacterController.isGrounded) {
				m_animator.SetBool("is_Fall", false);
				m_animator.SetBool("is_LongFall", false);
				//Debug.Log("グラウンドオン");
				//Debug.Log(m_CharacterController.stepOffset);
				JumpDown.y = 0;
			}
		}
		
		// ジャンプ落下中に段差を登るための防止
		if (m_animator.GetFloat("JumpPower") >= 1.0f) {
			if (time > 10) {
				time = 0;
				m_CharacterController.stepOffset = 0.1f;
			} else {

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

    // updateの前に走る
    private void FixedUpdate()
    {
		// CharacterControllerのis_Groundedを引き渡す
		m_animator.SetBool("is_Grounded" , m_CharacterController.isGrounded);
    }

	//============================================================
	// Getter or Setter
	//============================================================

	/// <summary>
	/// プレイヤーの移動予定のポジションをセットする
	/// </summary>
	/// <param name="MovePos">移動予定場所</param>
	public void setMovePosition(Vector3 MovePos) {
		m_MoveState.setMovePosition(MovePos);
	}
	/// <summary>
	/// プレイヤーのポジションを返す
	/// </summary>
	/// <returns>プレイヤーの位置</returns>
	public Vector3 getPlayerPos() {
		return transform.position;
	}

	/// <summary>
	/// プレイヤーのIDを返す
	/// </summary>
	/// <returns>プレイヤーID</returns>
	public int getPlayerID() {
		return m_PlayerID;
	}


	//============================================================
	// ==2017/10/31 Oyama Add
	// ObjectActionAreaスクリプトから直接実行される部分
	//
	// オブジェクトに当たったらそのオブジェクトに応じたアクションを実行
	// そのあとは各スクリプトで状態を管理させる
	// アニメーターのアニメーション名と関数を一致させること
	//============================================================

    /// <summary>
    /// ボルトアクション用
    /// </summary>
    /// <param name="name">タグ名</param>
    public void Vault(string name)
    {
        // ボタンが押されていたらステート切り替え かつ 現在再生されているアニメーションがVaultではない
        if ((Input.GetKey(KeyCode.S) || this.m_Controller.GetButtonDown(Controller.Button.A)) && !m_animator.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            m_animator.Play(name);
            m_MoveState.changeState(MoveState.MoveStatement.Vault, name);
        }
    }

    // クライム
    public void Climb(string name)
    {
        // ボタンが押されていたらステート切り替え かつ 現在再生されているアニメーションがVaultではない
        if ((Input.GetKey(KeyCode.S) || this.m_Controller.GetButtonDown(Controller.Button.A)) && !m_animator.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            m_animator.Play(name);
            m_MoveState.changeState(MoveState.MoveStatement.Climb, name);
        }
    }

    // スライダー
    public void Slide(string name)
    {
        // ボタンが押されていたらステート切り替え かつ 現在再生されているアニメーションがVaultではない
        if ((Input.GetKey(KeyCode.S) || this.m_Controller.GetButtonDown(Controller.Button.A)) && !m_animator.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            m_animator.Play(name);
            m_MoveState.changeState(MoveState.MoveStatement.Slider, name);
        }
    }
	
}

/*メモ
 *
    コントローラの取得の仕方　仮
    if (this.m_Controller.GetButton(Controller.Button.A))
 */
   