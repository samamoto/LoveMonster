using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchTarget : MonoBehaviour {

    private Animator            animator;	//　アニメーター
    private bool                jumpFlag;	//　ジャンプ中かどうか
    private CharacterController charaCon;   //　キャラクターコントローラー
    public  Transform           jumpTarget; //　到達地点
	public	AvatarTarget		avatarTarget;//	使う部位

	//private NavMeshAgent agent;			//　ナビメッシュエージェント

	private void Start()
    {
        animator = GetComponent<Animator>();
        charaCon = GetComponent<CharacterController>();
        jumpFlag = false;
		avatarTarget = AvatarTarget.RightHand;
        //agent = GameObject.Find("point").GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //　マウスの右クリックを押した時　かつ　ジャンプエリア内　かつ　ジャンプ中じゃない時
        if (Input.GetButtonDown("Fire2") && !animator.GetCurrentAnimatorStateInfo(0).IsName("TestJump"))
        {
			//　ジャンプエリア外にする
			animator.SetTrigger("TestJump");
		}

        //　ジャンプ中で一周り目の時
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("TestJump"))
        {
			//DebugPrint.print(transform.position.ToString());
            transform.rotation = Quaternion.RotateTowards(transform.rotation, jumpTarget.rotation, 360 * Time.deltaTime);
            //agent.enabled = false;
            animator.applyRootMotion = true;
            jumpFlag = true;
            charaCon.enabled = false;
         
            //　ターゲットマッチング
            animator.MatchTarget(
                jumpTarget.position,
                jumpTarget.rotation,
                avatarTarget,
                new MatchTargetWeightMask(new Vector3(1, 1, 1), 0), 0.180f, 0.302f);	// 角度処理は直さなきゃいけない
        }

        //　ジャンプ終了したら元に戻す
        if (jumpFlag)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.80f)
            {
				DebugPrint.print(transform.position.ToString());
				jumpFlag = false;
                animator.applyRootMotion = false;
                charaCon.enabled = true;
                //agent.enabled = true;
                animator.SetBool("TestJump", false);

            }
        }
    }
}
