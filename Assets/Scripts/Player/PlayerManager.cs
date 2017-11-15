using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Controller;

//RequireComponentで指定することでそのスクリプトが必須であることを示せる
[RequireComponent(typeof(ThirdPersonCharacter))]
[RequireComponent(typeof(MoveState))]
[RequireComponent(typeof(Controller.Controller))]
public class PlayerManager : MonoBehaviour
{
	// PlayerのID
	public int m_PlayerID = 1; // 今は一人しかいない

	private MoveState m_MoveState;	// 移動処理を任せる
	private AllPlayerManager m_AllPlayerManager;
	private Animator m_animator;

	private Controller.Controller m_Controller;
	public bool walkByDefault = false; // toggle for walking state

	public bool lookInCameraDirection = true;// should the character be looking in the same direction that the camera is facing

	private Vector3 lookPos; // The position that the character should be looking towards
	private ThirdPersonCharacter character; // A reference to the ThirdPersonCharacter on the object
	private Transform cam; // A reference to the main camera in the scenes transform
	private Vector3 camForward; // The current forward direction of the camera

	private Vector3 move;
	private bool jump;// the world-relative desired move direction, calculated from the camForward and user input.

	// Use this for initialization
	void Start() {
		m_AllPlayerManager = GetComponentInParent<AllPlayerManager>();
		m_Controller = GetComponent<Controller.Controller>();
		m_MoveState = GetComponent<MoveState>();
		m_animator = GetComponent<Animator>();
		
		// Initialize the third person character
		//----------------------------------------------------------------------
		// get the transform of the main camera
		if (Camera.main != null) {
			cam = Camera.main.transform;
		} else {
			Debug.LogWarning(
				"Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
			// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
		}

		// get the third person character ( this should never be null due to require component )
		character = gameObject.GetComponent<ThirdPersonCharacter>();

		//character = GetComponents<ThirdPersonCharacter>();
		if (character == null) {
			character = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacter>();    // 無理矢理探す
			// なんか知らないけどGetComponentしてるのにnullを返してくる
			//character = gameObject.AddComponent<ThirdPersonCharacter>();
			if(character == null) {
				Debug.LogWarning("Third Person Character Null Reference!!");
				Debug.Break();
			}
		}

	}

	// Update is called once per frame
	void Update()
    {
        // MoveStateの状態確認
        if (!m_MoveState.isMove()){
			if (!jump) {
				if (m_Controller.GetButtonDown(Controller.Button.A) || Input.GetButtonDown("Jump")) {
					//jump = m_Controller.GetButtonDown(Controller.Button.A);
					jump = true;
				}
				m_animator.SetBool("is_Jump", jump);    // oyama add
			}
		} else {
			//m_MoveState.Update();   // 外部から操作を受け付け
			return;                 // なにもしない

		}

	}

	/// <summary>
	/// ThirdPersonFixedUpdate
	/// </summary>
	void FixedUpdate() {

		if (m_MoveState.isMove()) {
			return;
		}
		// read inputs
		bool crouch = false;
		bool slide = false;
		bool vault = false;
		bool climb = false;
		bool wallrun = false;

		float h, v;

		//get input from sticks and buttons
		if (Controller.Controller.GetConnectControllers() > 0) {
			h = m_Controller.GetAxisRaw(Axis.L_x);
			v = m_Controller.GetAxisRaw(Axis.L_y)*-1; // なんか反転しちゃう
											  //float v = Input.GetAxisRaw("Vertical");	// InputManagerのInvertがチェック入ってると反転
		} else {
			// つながってないとき
			h = Input.GetAxis("Horizontal");
			v = Input.GetAxis("Vertical");
		}
		//ToDo:鹿島
		//インプットを作ったやつに変える
		//Read in inputs and set true/false
		// -true only if the button is pressed and the character is in the ActionArea)
#if DEBUG
		crouch = Input.GetKey(KeyCode.C);
		slide = Input.GetKey(KeyCode.M) && (move.magnitude > 0);
		vault = Input.GetKey(KeyCode.V);
		climb = Input.GetKey(KeyCode.Z);
		wallrun = Input.GetKey(KeyCode.X) && (move.magnitude > 0);
#endif

		// calculate move direction to pass to character
		if (cam != null) {
			// calculate camera relative direction to move:
			camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
			move = v * camForward + h * cam.right;
		} else {
			// we use world-relative directions in the case of no main camera
			move = v * Vector3.forward + h * Vector3.right;
		}

		if (move.magnitude > 1) move.Normalize();

		// calculate the head look target position
		lookPos = lookInCameraDirection && cam != null
						? transform.position + cam.forward * 100
						: transform.position + transform.forward * 100;

		// pass all parameters to the character control script
		character.Move(move, crouch, jump, vault, slide, climb, wallrun, lookPos);
		jump = false;
		m_animator.SetBool("is_Jump", jump);	// add oyama
	}

	//============================================================
	// Getter or Setter
	//============================================================
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
	/// 一個にまとめた
	/// </summary>
	/// <param name="name">タグ名</param>
	public void PlayAction(string name) {

		//if (m_Controller.GetButtonDown(Controller.Button.A) && !m_animator.GetCurrentAnimatorStateInfo(0).IsName(name)) {
		if (m_Controller.GetButtonDown(Controller.Button.A)) {

			for (MoveState.MoveStatement m=MoveState.MoveStatement.None ; m>=MoveState.MoveStatement.None-MoveState.MoveStatement.None; m--) {
				// Dictionaryと検索してタグを検索
				if (m_MoveState.StateDictionary[m] == name) {
					// MoveStatementのenumに変換したiと検出したタグ名を投げる
					m_animator.Play(name);
					m_MoveState.changeState(m, name);
				}
			}

		}

	}

	
}

/*メモ
 *
    コントローラの取得の仕方　仮
    if (this.m_Controller.GetButton(Controller.Button.A))
 */
   