using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : StateMachineBehaviour
{

    AllPlayerManager PM;

    public float m_JumpUpSpeed;
    float m_Jump;
    float m_Velocity;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Jump = m_JumpUpSpeed;
        PM = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        m_Velocity = animator.GetFloat("Velocity");
        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_Velocity += PM.m_RunSpeed;
            if (m_Velocity > PM.m_MaxRunSpeed)
            {
                m_Velocity = PM.m_MaxRunSpeed;
            }
        }
        else
        {
            m_Velocity -= PM.m_RunSpeed;
            if (m_Velocity <= 0.0f)
            {
                m_Velocity = 0.0f;
            }
        }
        animator.SetFloat("Velocity", m_Velocity);

        animator.SetBool("is_Jump", false);
        animator.SetBool("is_Land", true);
        animator.SetFloat("JumpPower", m_Jump);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Jump = 0.0f;
        animator.SetFloat("JumpPower", m_Jump);
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
