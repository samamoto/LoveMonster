using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkRun : StateMachineBehaviour {
    enum STATE
    {
        NONE,
        UP,
        DOWN
    }
    STATE state;
    float speed;
    AllPlayerManager PM;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        state = STATE.UP;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        speed = animator.GetFloat("Speed");


        switch (state)
        {
            case STATE.UP://加速
                if (Input.GetKey(KeyCode.UpArrow))
                {

                    speed += 0.1f;// PM.m_RunSpeed;
                }
                else
                {
                    state = STATE.DOWN;
                }
                break;
            case STATE.DOWN:
                if (Input.GetKey(KeyCode.UpArrow))
                {

                    state = STATE.UP;
                }
                else
                {
                    speed -= 0.1f;// PM.m_RunSpeed;
                }


                break;
        }


        if (speed <= 0.0f)
        {
            animator.SetBool("is_Dush", false);
        }
        if (speed > 5.0f)
        {
            speed = 5.0f;
        }

        animator.SetFloat("Speed", speed);
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
