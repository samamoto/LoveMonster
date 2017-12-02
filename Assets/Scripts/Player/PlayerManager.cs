using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
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

	private Vector3 m_RestartPoint = Vector3.zero;  // リスタート用 2017年12月02日 oyama add

	//david add
	//wallrun varaiables 
	public float wallRunSpeedFactor = 10.0f;
    public float minWallRunSpeed = 5.0f; // minimum speed player has to move in order to maintain wall run
    public float maxWallRunTime = 0.1f; // how long a player can wall run
    private bool wallRunActivated = false; // set in Update() and used to activate wallRun in FixedUpdate()
    private bool wallRunTimeUp = false; // controlling flag for handling when wall run time times up

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

		// 状態管理
		bool Roll = m_animator.GetBool("is_Rolling");

		if (m_MoveState.isMove()) {
			return;
		}
	
		//david add
		if (wallRunTimeUp && character.onGround) // reset wall run if the player lands on the ground
        { 
            wallRunTimeUp = false;
        }

        float v = m_Controller.GetAxisRaw(Axis.L_y) ;
        v = GetComponent<Rigidbody>().velocity.magnitude;
        Rigidbody rb = GetComponent<Rigidbody>();
        Quaternion charRotation = transform.localRotation;

        Vector3 adjustedDir = charRotation * character.GetComponent<Rigidbody>().velocity; // rotate velocity for rotation on character


		// 仮にコントローラーのLBに設定した
		// if the player can wallrun then wall run
		if (CanWallRun(character.transform, v, adjustedDir, m_Controller.GetButtonHold(Button.LB)) && !wallRunTimeUp)
        {
            wallRunActivated = true; // turn on wallRun so it can be ran in fixedUpdate(better for rigibody manipulations)
        }
        else
        {
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
			v = m_Controller.GetAxisRaw(Axis.L_y);
		} else {
			// つながってないとき
			h = Input.GetAxis("Horizontal");
			v = Input.GetAxis("Vertical");
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
            
            if(Physics.Raycast(playerPosition, character.transform.right, out hitR, rayCastDistance))
            {
                Vector2 tmp = new Vector2(v, h); // get a value influenced by both axes

                wallrun = wallRunActivated;
                WallRun(hitR, v, h, Time.deltaTime, 1);
                StartCoroutine(afterWallRun());
            }
            else if(Physics.Raycast(playerPosition, -character.transform.right, out hitL, rayCastDistance))
            {
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

		if ((m_Controller.GetButtonDown(Controller.Button.A) &&
			!m_animator.GetCurrentAnimatorStateInfo(0).IsName(name)) || Input.GetKey(KeyCode.Z)) {
		//if (m_Controller.GetButtonDown(Controller.Button.X) || Input.GetKey(KeyCode.Z)) {

			for (MoveState.MoveStatement m=MoveState.MoveStatement.None ; m>=MoveState.MoveStatement.None-MoveState.MoveStatement.None; m--) {
				// Dictionaryと検索してタグを検索
				if (m_MoveState.StateDictionary[m] == name) {
					// MoveStatementのenumに変換したiと検出したタグ名を投げる
					m_animator.SetBool("is_" + name, true);
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
		eff.createItemHit(m_RestartPoint);	// 仮にエフェクト再生
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
    private bool CanWallRun(Transform trans, float speed, Vector3 moveDir, bool jumpButtonHold)
    {

        bool canWallRun = false;
        
        // only wallrun if player is mid-air can he wall run and
        // if the jump button is held down and the speed at which the player is moving is fast enough
        if (!character.onGround && jumpButtonHold && (speed > minWallRunSpeed))
        {
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
    private void WallRun(RaycastHit hitInfo, float v, float h, float time, int direction)
    {
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
    IEnumerator afterWallRun()
    {
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
