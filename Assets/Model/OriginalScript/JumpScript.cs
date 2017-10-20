using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            anim.SetBool("is_jumping", true);
        }

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("Locomotion.Jump"))
        {
            anim.SetBool("is_jumping", false);
        }
    }
}