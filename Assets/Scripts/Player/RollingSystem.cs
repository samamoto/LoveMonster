using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingSystem : MonoBehaviour {

	public float possibleDistance = 1.5f;

    private Animator animator;      //　アニメーター
	private SearchCollider m_SearchCollider;	

    private bool rollingFlag;       //  ローリングのフラグ
	[SerializeField, Range(0.01f, 1.0f)]public float normalizedTime = 0.76f;

    bool flag;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
		m_SearchCollider = GetComponentInChildren<SearchCollider>();
        rollingFlag = false;
    }
	
	// Update is called once per frame
	void Update () {
        flag = animator.GetBool("is_Grounded");

		// 地上から指定の距離以内なら受け身をON Jump上昇中は検知させない
		if (m_SearchCollider.GetGroundDistance() <= possibleDistance &&
			m_SearchCollider.GetPlayerJump() == -1) {
			if (Input.GetButton("Fire2")) {
				//Debug.Log("Rolling");
				rollingFlag = true;
			}

			if (rollingFlag) {
				animator.SetBool("is_Rolling", true);
				animator.applyRootMotion = true;
			}

		}
		//　ジャンプ終了したら元に戻す
		if (rollingFlag) {
			// アニメーターのパーセンテージが指定の値以上かつ、Rollingなら
			// またはRolling以外で地上にいる
			if ((animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= normalizedTime &&
				animator.GetCurrentAnimatorStateInfo(0).IsName("Rolling"))){ 
//				||	(!animator.GetCurrentAnimatorStateInfo(0).IsName("Rolling") && flag)) {
				rollingFlag = false;
				animator.applyRootMotion = false;
				animator.SetBool("is_Rolling", false);
			}
		}
	}
}
