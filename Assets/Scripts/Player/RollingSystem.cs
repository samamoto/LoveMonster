using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingSystem : MonoBehaviour {

	public float possibleDistance = 1.5f;

    private Animator animator;      //　アニメーター
	private SearchCollider m_SearchCollider;
	private MoveState m_MoveState;

    private bool rollingFlag;       //  ローリングのフラグ
	private bool StopControll = false;	// 外部から停止

	[SerializeField, Range(0.01f, 1.0f)]public float normalizedTime = 0.76f;

    bool flag;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
		m_SearchCollider = GetComponentInChildren<SearchCollider>();
        rollingFlag = false;
		m_MoveState = GetComponent<MoveState>();
    }
	
	// Update is called once per frame
	void Update () {

		if (m_MoveState.isMove() || StopControll) { return; }

        flag = animator.GetBool("is_Grounded");

		// 地上から指定の距離以内なら受け身をON Jump上昇中は検知させない
		if (m_SearchCollider.GetGroundDistance() <= possibleDistance &&
			m_SearchCollider.GetPlayerJump() == -1) {
			if (Input.GetButton("Fire2")) {
				rollingFlag = true;
			}

			if (rollingFlag) {
				animator.SetBool("is_Rolling", true);
				//animator.applyRootMotion = true;
			}

		}
		//　ジャンプ終了したら元に戻す
		if (rollingFlag || m_SearchCollider.GetPlayerJump() == 0) {
			// アニメーターのパーセンテージが指定の値以上かつ、Rollingなら
			// またはRolling以外で地上にいる
			if ((animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= normalizedTime &&
				animator.GetCurrentAnimatorStateInfo(0).IsName("Rolling"))){ 
//				||	(!animator.GetCurrentAnimatorStateInfo(0).IsName("Rolling") && flag)) {
				rollingFlag = false;
				//animator.applyRootMotion = false;
				animator.SetBool("is_Rolling", false);
			}

		}
	}
	/// <summary>
	/// ローリングのフラグを外部からリセットする
	/// </summary>
	public void resetRolling() {
		rollingFlag = false;
		animator.SetBool("is_Rolling", false);
	}

	/// <summary>
	/// 外部から停止
	/// </summary>
	/// <param name="flag">解除/停止</param>
	public void stopControll(bool flag) {
		StopControll = flag;
	}
}
