using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingSystem : MonoBehaviour {

    private Animator animator;      //　アニメーター
    private bool rollingFlag;	    //  ローリングのフラグ
    bool flag;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        rollingFlag = false;
    }
	
	// Update is called once per frame
	void Update () {
        flag = animator.GetBool("is_Grounded");

        if (Input.GetButtonDown("Fire2")
            &&flag)
        {
            Debug.Log("Rolling");
            animator.SetBool("is_Rolling", true);
            animator.applyRootMotion = true;
            rollingFlag =true;
        }

        //　ジャンプ終了したら元に戻す
        if (rollingFlag)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            {
                rollingFlag = false;
                animator.applyRootMotion = false;
                animator.SetBool("is_Rolling", false);
            }
        }
    }
}
