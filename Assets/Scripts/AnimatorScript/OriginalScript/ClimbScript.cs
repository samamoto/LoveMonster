using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbScript : MonoBehaviour {

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            anim.SetBool("is_Climb", true);
            anim.SetBool("is_ClimbJump", true);
        }

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("Locomotion.Climb"))
        {
            anim.SetBool("is_Climb", false);
        }

        if (state.IsName("Locomotion.ClimbJump"))
        {
            anim.SetBool("is_ClimbJump", false);
        }
    }
}
