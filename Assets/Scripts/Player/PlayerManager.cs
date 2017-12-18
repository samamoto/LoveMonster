using UnityEngine;
using UnityEngine.EventSystems; // SendMessageの受信側
using System.Collections;
using System.Collections.Generic;
using System;
using Controller;

//RequireComponentで指定することでそのスクリプトが必須であることを示せる
[RequireComponent(typeof(ThirdPersonCharacter))]
[RequireComponent(typeof(MoveState))]
[RequireComponent(typeof(Controller.Controller))]
public class PlayerManager : MonoBehaviour, PlayerReciever {
	// PlayerのID
	public int m_PlayerID = 1; // 今は一人しかいない

	private MoveState m_MoveState;  // 移動処理を任せる
	private AllPlayerManager m_AllPlayerManager;
	private Animator m_animator;
	private Controller.Controller m_Controller;
	private PrintScore m_Score;     // スコア管理
	private AudioList m_Audio;      // 音声再生用
	private ComboSystem m_Combo;    // コンボシステム

	private Vector3 m_RestartPoint = Vector3.zero;  // リスタート用 2017年12月02日 oyama add
	private bool is_StopControl = false;            // コントロールの制御をするか(カウントダウン時など) 2017年12月07日 oyama add 
	private int m_GoalCount = 0;					// ゴールした回数　これが3回ほどするとゴール判定

	// Third parson character ------
	public bool walkByDefault = false; // toggle for walking state

	public bool lookInCameraDirection = true;// should the character be looking in the same direction that the camera is facing

	private Vector3 lookPos; // The position that the character should be looking towards
	private ThirdPersonCharacter character; // A reference to the ThirdPersonCharacter on the object
	private Transform cam; // A reference to the main camera in the scenes transform
	private Vector3 camForward; // The current forward direction of the camera

	private Vector3 move;
	private bool jump;// the world-relative desired move direction, calculated from the camForward and user input.
					  // ------Third parson character

	//david add
	//wallrun varaiables 
	public float wallRunSpeedFactor = 10.0f;
	public float minWallRunSpeed = 5.0f; // minimum speed player has to move in order to maintain wall run
	public float maxWallRunTime = 0.1f; // how long a player can wall run
	private bool wallRunActivated = false; // set in Update() and used to activate wallRun in FixedUpdate()
	private bool wallRunTimeUp = false; // controlling flag for handling when wall run time times up
	private float m_seDelay = 0.0f;

	//david add
	// walljump variables
	public bool isWallJumping = false;

	// Use this for initialization
	void Start() {
		m_AllPlayerManager = GetComponentInParent<AllPlayerManager>();
		m_Controller = GetComponent<Controller.Controller>();
		m_MoveState = GetComponent<MoveState>();
		m_animator = GetComponent<Animator>();
		m_Score = GameObject.Find("ScoreManager").GetComponent<PrintScore>();
		m_Audio = GameObject.Find("SoundManager").GetComponent<AudioList>();
		m_Combo = GetComponent<ComboSystem>();

		// Initialize the third person character
		//----------------------------------------------------------------------
		// get the transform of the main camera
		if (Camera.main != null) {
			//cam = Camera.main.transform;
			string camStr = "MainCamera" + m_PlayerID.ToString();
			cam = GameObject.Find(camStr).GetComponent<Transform>();
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
			if (character == null) {
				Debug.LogWarning("Third Person Character Null Reference!!");
				Debug.Break();
			}
		}

	}

	// Update is called once per frame
	void Update() {
		// MoveStateの移動制御が走っている場合、または、外部から止められている
		if (m_MoveState.isMove() || is_StopControl) {
			return;
		}

		// 状態管理
		bool Roll = m_animator.GetBool("is_Rolling");

		//david add
		if (wallRunTimeUp && character.onGround) // reset wall run if the player lands on the ground
		{
			wallRunTimeUp = false;
		}

		float v = m_Controller.GetAxisRaw(Axis.L_y);
		v = GetComponent<Rigidbody>().velocity.magnitude;
		Rigidbody rb = GetComponent<Rigidbody>();
		Quaternion charRotation = transform.localRotation;

		Vector3 adjustedDir = charRotation * character.GetComponent<Rigidbody>().velocity; // rotate velocity for rotation on character


		// 仮にコントローラーのLBに設定した
		// 2017年12月17日 oyama change
		// 壁蹴りがあるのでWallRunをなくした
		// if the player can wallrun then wall run
		if (CanWallRun(character.transform, v, adjustedDir, m_Controller.GetButtonHold(Button.LB)) && !wallRunTimeUp) {
			//wallRunActivated = true; // turn on wallRun so it can be ran in fixedUpdate(better for rigibody manipulations)
		} else {
			character.wallRunning = false; // can't wallRun so turn the flags off
			wallRunActivated = false;
		}

		// MoveStateの状態確認
		if (!jump && !Roll) {
			// キーボードのほうは全員でジャンプする（キーボードはID管理してない）
			if (m_Controller.GetButtonDown(Button.A) || Input.GetKeyDown(KeyCode.Space)) {
				jump = true;
			}
			m_animator.SetBool("is_Jump", jump);    // oyama add
		}
		// 再生関数
		ActionSE();
	}

	/// <summary>
	/// アクションが再生される時にSEを鳴らす
	/// </summary>
	void ActionSE() {
		// Jump
		if (m_animator.GetBool("is_Jump") && m_animator.GetBool("is_Grounded")　&& !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Crouching")) {
			if(m_seDelay == 0) {
				m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_ActionJump);
				//m_seDelay = 0.4f;
			}
		}
		//着地
		if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Crouching")) {
			if (m_seDelay == 0) {
				m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_ActionLanding);
				m_seDelay = 0.4f;
			}
		}
		
		//スライド 音声が遅いのでStateMacineBehavior側でやってる
		if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Slide")) {
			if (m_seDelay == 0) {
				//m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_ActionSlide);
				//m_seDelay = 0.82f;
				GetComponentInChildren<EffectTrailManager>().setActive(true, gameObject);
			}
		}
		
		//ロングスライド
		if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("LongSlider")) {
			if (m_seDelay == 0) {
				m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_ActionSlide);
				m_seDelay = 1;
			}
		}
		//ヴォルト
		if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Vault")) {
			if (m_seDelay == 0) {
				m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_ActionVault);
				m_seDelay = 0.45f;
			}
		}
		//キングヴォルト
		if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("KongVault")) {
			if (m_seDelay == 0) {
				m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_ActionVault);
				m_seDelay = 0.6f;
			}
		}
		//クライム
		if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Climb")) {
			if (m_seDelay == 0) {
				m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_ActionGrab);
				m_seDelay = 0.8f;
			}
		}
		//クライムオーバー
		if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("ClimbOver")) {
			if (m_seDelay == 0) {
				m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_ActionGrab);
				m_seDelay = 0.5f;
			}
		}

		//多重再生防止のディレイ処理
		if (m_seDelay > 0) {
			m_seDelay -= Time.deltaTime;
		} else {
			m_seDelay = 0;
		}
		/////////////////////////////////////////////////////////////////////////
	}
	/// <summary>
	/// ThirdPersonFixedUpdate
	/// </summary>
	void FixedUpdate() {

		// MoveStateの移動制御が走っている場合、または、外部から止められている
		if (m_MoveState.isMove() || is_StopControl) {
			return;
		}
		// read inputs
		bool crouch = false;
		bool slide = false;
		bool vault = false;
		bool climb = false;
		bool wallrun = false;

		float h, v;
		// if player is wall jumping then lock up the stick inputs;
		// by setting them to 0, it will effectively seem like they were locked up
		if (isWallJumping) {
			h = 0;
			v = 0;
		} else {
			//get input from sticks and buttons
			if (Controller.Controller.GetConnectControllers() > 0) {
				h = m_Controller.GetAxisRawThreshold(Axis.L_x, 0.05f);  // 2017/12/11 oyama add 閾値設定して小さな誤入力を消す
				v = m_Controller.GetAxisRawThreshold(Axis.L_y, 0.05f);
			} else {
				// つながってないとき
				h = Input.GetAxis("Horizontal");
				v = Input.GetAxis("Vertical");
			}
		}

		if (cam != null) {
			// calculate camera relative direction to move:
			camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
			move = v * camForward + h * cam.right;
		} else {
			// we use world-relative directions in the case of no main camera
			move = v * Vector3.forward + h * Vector3.right;
		}

		if (move.magnitude > 1) move.Normalize();

		//david add
		// wallRun action
		if (wallRunActivated) // wallRun is called and the rigidbody processing is done here
		{
			Vector3 playerPosition = character.transform.position;
			//RaycastHit hitInfo;
			RaycastHit hitR;
			RaycastHit hitL;
			float rayCastDistance = 1.0f;

			if (Physics.Raycast(playerPosition, character.transform.right, out hitR, rayCastDistance)) {
				Vector2 tmp = new Vector2(v, h); // get a value influenced by both axes

				wallrun = wallRunActivated;
				WallRun(hitR, v, h, Time.deltaTime, 1);
				StartCoroutine(afterWallRun());
			} else if (Physics.Raycast(playerPosition, -character.transform.right, out hitL, rayCastDistance)) {
				Vector2 tmp = new Vector2(v, h);

				wallrun = wallRunActivated;
				WallRun(hitL, v, h, Time.deltaTime, -1);
				StartCoroutine(afterWallRun());
			}
		}

		// calculate the head look target position
		lookPos = lookInCameraDirection && cam != null
						? transform.position + cam.forward * 100
						: transform.position + transform.forward * 100;

		// pass all parameters to the character control script
		character.Move(move, crouch, jump, vault, slide, climb, wallrun, lookPos);
		jump = false;
		m_animator.SetBool("is_Jump", jump);    // add oyama

	}

	//============================================================
	// Getter or Setter
	//============================================================
	/// <summary>
	/// 現在プレイヤーがどのアクションをしているか
	/// </summary>
	public string getPlayerAction() {
		return m_MoveState.getPlayerAction();
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

	/// <summary>
	/// プレイヤーのリスタート位置をセットする.上書き
	/// </summary>
	/// <returns>プレイヤーID</returns>
	public void setRestartPosition(Vector3 vec) {
		m_RestartPoint = vec;
	}

	/// <summary>
	/// ゴールした回数を追加
	/// </summary>
	public void plusGoalFrequency() {
		m_GoalCount++;
	}

	public int getGoalFrequency() {
		return m_GoalCount;
	}
	
	public void startPlayerPostion(Vector3 pos, Quaternion rot) {
		gameObject.transform.position = pos;
		gameObject.transform.rotation = rot;
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
	/// アクションを再生する
	/// </summary>
	/// <param name="name">タグ名</param>
	public void PlayAction(string name) {
		PlayAction(name, Button.A);
	}

	/// <summary>
	/// アクションを再生する
	/// </summary>
	/// <param name="name">タグ名</param>
	/// <param name="button">実行するときに使うボタン</param>
	public void PlayAction(string name, Controller.Button button) {

		if ((m_Controller.GetButtonDown(button) || Input.GetKey(KeyCode.Z)) &&
			!m_animator.GetCurrentAnimatorStateInfo(0).IsName(name)) {

			for (MoveState.MoveStatement m = MoveState.MoveStatement.None; m >= MoveState.MoveStatement.None - MoveState.MoveStatement.None; m--) {
				// Dictionaryと検索してタグを検索
				if (m_MoveState.StateDictionary[m] == name) {
					// MoveStatementのenumに変換したiと検出したタグ名を投げる
					m_animator.SetBool("is_" + name, true);
					m_animator.Play(name);
					m_MoveState.changeState(m, name);
				}
			}
		}
	}

	/// <summary>
	/// アクションを再生する
	/// </summary>
	/// <param name="name">タグ名</param>
	/// <param name="button">実行するときに使うボタン</param>
	public void PlayAction(string name, Controller.Button button, Vector3[] move, int score) {
		// 指定されたボタンが押され、現在の再生アニメーションがアクション予定と違う
		if ((m_Controller.GetButtonDown(button) || Input.GetKey(KeyCode.Z)) &&
			!m_animator.GetCurrentAnimatorStateInfo(0).IsName(name)) {
			m_MoveState.setMovePosition(move);
			for (MoveState.MoveStatement m = MoveState.MoveStatement.None; m >= MoveState.MoveStatement.None - MoveState.MoveStatement.None; m--) {
				// Dictionaryと検索してタグを検索
				if (m_MoveState.StateDictionary[m] == name) {
					// MoveStatementのenumに変換したiと検出したタグ名を投げる
					m_animator.SetBool("is_" + name, true);
					m_animator.Play(name);
					// PrintScoreにスコアの基準値を投げる
					// スコアマネージャに送信
					ExecuteEvents.Execute<ScoreReciever>(
						target: m_Score.gameObject,
						eventData: null,
						functor: (reciever, y) => reciever.ReceivePlayerScore(this.m_PlayerID, score)
					);
					m_Combo._AddCombo();    // アクションが実行されるたびにコンボを＋１
					m_MoveState.changeState(m, name);
				}
			}
		}
	}

	// 2017年12月01日 oyama add
	/// <summary>
	/// プレイヤーがリスタートする時の処理
	/// Todo:エフェクトとかもいる
	/// </summary>
	public void restartPlayer() {
		EffectControl eff = EffectControl.get();
		transform.position = m_RestartPoint;
		eff.createItemHit(m_RestartPoint);  // 仮にエフェクト再生
	}

	/// <summary>
	/// プレイヤーのコントロールを停止させる
	/// </summary>
	/// <param name="flag">false:通常|true:停止</param>
	public void stopControl(bool flag) {
		is_StopControl = flag;
	}

	//============================================================
	/// <summary>
	/// プレイヤー同士が接触したときの処理
	/// </summary>
	/// <param name="name">タグ名</param>
	void OnTriggerStay(Collider other) {
		if (other.tag == "Player") {
			// 衝突したときの処理
			// 上になんか送る
		}
	}

	// david add
	/// <summary>
	/// Can the player wall run?
	/// Requirements to wall run:
	///     1. Wall in direction of movement(either to the left or right)
	///     2. Moving fast enough( > minWallRunSpeed)
	///     3. Holding "jump" button
	///     4. Player is mid-air
	/// </summary>
	/// <param location="location of the player"> location of the player </param>
	/// <param speed="speed of the player's movement(will be the value of the directional stick being held down)"></param>
	/// <param moveDir="direction in which the player is moving"></param>
	/// <param jumpButtonHold="whether or not the jump button is held down"></param>
	private bool CanWallRun(Transform trans, float speed, Vector3 moveDir, bool jumpButtonHold) {

		bool canWallRun = false;

		// only wallrun if player is mid-air can he wall run and
		// if the jump button is held down and the speed at which the player is moving is fast enough
		if (!character.onGround && jumpButtonHold && (speed > minWallRunSpeed)) {
			canWallRun = true;
		}
		return canWallRun;
	}

	// david add
	/// <summary>
	/// WallRun action
	/// attaches player to the point on the wall and allows them to move in a straight line against factored by the designated speed
	/// direction: -1 for going left, 1 for going right
	/// </summary>
	private void WallRun(RaycastHit hitInfo, float v, float h, float time, int direction) {
		character.wallRunning = true;

		Rigidbody rb = character.GetComponent<Rigidbody>();
		Vector3 wallRunPoint = hitInfo.point;
		Quaternion rotation = transform.rotation;
		Vector3 charForward = new Vector3();


		charForward = Vector3.Cross(character.transform.up, hitInfo.normal);
		// rotate the foward vector with respect to the direction the character is facing to get "true" forward vector cuz the original doesn't rotate
		charForward = rotation * charForward * direction;

		Vector3.Normalize(charForward);

		// determine which direction player is facing relative to wall
		float leftRightMatch = Vector3.Dot(Vector3.right, charForward);
		float frontBackMatch = Vector3.Dot(Vector3.forward, charForward);

		if (direction == 1) // wall is on the players right side
		{
			GetComponent<Animator>().SetBool("WallOnRight", true);
		} else // wall is on the players left side
		  {
			GetComponent<Animator>().SetBool("WallOnRight", false);
		}

		// TODO: Lots of black magic hard coding to get right numbers for horizontal and frontal wall running, FIX PLZ!
		//       theres got to be a more elegant method
		/*
		if (frontBackMatch < leftRightMatch) // player is facing on the x axis, for horizontal walls
        {
            charForward = Vector3.Cross(character.transform.up, hitInfo.normal);
            float speed = Math.Abs(h);

            // TODO: black magic hacking together the right values, think of better solution. This method has no versatility guarantees
            if (h > 0)
                charForward.x *= -1;

            if (v < 0)
            {
                charForward.z *= -1;
                speed *= -1;
            }

            rb.velocity = charForward * speed * wallRunSpeedFactor; // use the h value of stick to affect horizontal wall run

        }
        else // player is facing on the x axis, for horizontal walls
        {
            float speed = v;

            rb.velocity = charForward * speed * wallRunSpeedFactor; // use the v value of stick to affect frontal wall run
        }
		*/
		// attach player to the wall
		character.transform.position = Vector3.Lerp(character.transform.position, wallRunPoint, time);
		rb.useGravity = false; // turn off gravity so the player doesn't fall during wall run
		StartCoroutine(afterWallRun());
	}

	// coroutine for ending wallrun after a certain amount of time
	// TODO: make it work correctly, right now not detecting time up
	IEnumerator afterWallRun() {
		yield return new WaitForSeconds(maxWallRunTime);

		Rigidbody rb = character.GetComponent<Rigidbody>();
		rb.useGravity = true;

		wallRunTimeUp = true;
	}
}

/*メモ
 *
    コントローラの取得の仕方　仮
    if (this.m_Controller.GetButton(Controller.Button.A))
 */
