using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderScript : MonoBehaviour {

    private Animator anim;
    private CharacterController charaCon;

    void Start()
    {
        anim = GetComponent<Animator>();
        charaCon = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 a = Vector3.zero;


        if (Input.GetKey(KeyCode.X))
        {
            anim.SetBool("is_Slider", true);
        }

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("Locomotion.Slider"))
        {
            //アニメーションしてる時
            a.y = 0.5f;
            charaCon.height = 1;
            charaCon.center = a;
            anim.SetBool("is_Slider", false);
        }
        else
        {
            //アニメーションしてない時　
            a.y = 1;
            charaCon.height = 2;
            charaCon.center = a;
         
        }

    }

}
