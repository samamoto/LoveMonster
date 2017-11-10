using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPlayerManager : MonoBehaviour {

	private static AllPlayerManager _instance;

    //走る速さ
    public float m_RunSpeed = 0.2f;
    public float m_MaxRunSpeed = 5.0f;
    public float m_SideRunSpeed = 0.05f;

    //ジャンプ力
    public float m_JumpPower = 1.5f;

    //ジャンプ時間
    public float m_JumpTime = 2;

    //回転力
    public float m_RotatePower = 5.0f;

	// プレイヤーの通常動作
	public string[] NormalAction = {
		ConstAnimationStateTags.PlayerStateIdle,
		ConstAnimationStateTags.PlayerStateWalkRun,
		ConstAnimationStateTags.PlayerStateJump,
	};

	// 移動処理を任せる特殊アクション
	public string[] SpecialAction = {
		ConstAnimationStateTags.PlayerStateVault,
		ConstAnimationStateTags.PlayerStateSlider,
		ConstAnimationStateTags.PlayerStateWallRun,
		ConstAnimationStateTags.PlayerStateClimb,
		ConstAnimationStateTags.PlayerStateClimbJump,
	};

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Singleton
	//------------------------------------------------------------
	//private AllPlayerManger() {
	//	Debug.Log("Create SampleSingleton instance.");
	//}

	/// <summary>
	/// インスタンスの入手
	/// </summary>
	public static AllPlayerManager Instance {
		get {
			if (_instance == null) _instance = new AllPlayerManager();

			return _instance;
		}
	}
}
