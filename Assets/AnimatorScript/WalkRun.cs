using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkRun : StateMachineBehaviour {
    float m_Velocity;
    AllPlayerManager PM;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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
                animator.SetBool("is_Dush", false);
            }
        }




        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetBool("is_Jump", true);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetBool("is_Slide", true);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetBool("is_Climb", true);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            animator.SetBool("is_Vault", true);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            animator.SetBool("is_WallRun", true);
        }





        animator.SetFloat("Velocity", m_Velocity);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

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
