﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : StateMachineBehaviour {

    enum STATE
    {
        NONE,
        UP,
        DOWN
    }
    STATE state;
    float speed;

    //AllPlayerManager PM;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        state = STATE.NONE;
        speed = 0.0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (state)
        {
            case STATE.NONE:
                state = STATE.UP;
                break;

            case STATE.UP:
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    speed += 0.1f;
                }
                else
                {
                    state = STATE.DOWN;
                }
                break;
            case STATE.DOWN:
                speed -= 0.1f;
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    state = STATE.UP;
                }
                if (speed < 0.0f)
                {
                    speed = 0.0f;
                    state = STATE.NONE;
                }
                break;
        }

        animator.SetFloat("Speed", speed/*PM.m_RunSpeed*/);

        if(speed>=0.5f)
        {
            animator.SetBool("is_Dush", true);
        }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        state = STATE.NONE;
        speed = 0.0f;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK(inverse kinematics) should be implemented here.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
