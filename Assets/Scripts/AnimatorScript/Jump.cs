using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : StateMachineBehaviour {

    AllPlayerManager PM;
    //CharacterController CC;
    public float m_JumpUpSpeed;

    private float m_Jump;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PM = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
        //CC = GameObject.Find("Player1").GetComponent<CharacterController>();
        m_Jump = m_JumpUpSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //CC.stepOffset = 0.9f;
        animator.SetBool("is_Jump", false);
        animator.SetFloat("JumpPower", m_Jump);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //CC.stepOffset = 0.1f;
        m_Jump = 0;
        animator.SetFloat("JumpPower", m_Jump);
        animator.SetBool("is_Fall", true);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
