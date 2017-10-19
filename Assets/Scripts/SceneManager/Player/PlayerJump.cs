using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {
    int time = 0;
    int state;
    Vector3 aaa;
    // Use this for initialization
    void Start() {
        state = 0;
        //enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 0:
                time = 0;
                aaa = Vector3.zero;
                aaa.z = 0.1f;
                state = 1;
                break;

            case 1:
                aaa.y += GetComponentInParent<PlayerManager>().m_JumpPower * Time.deltaTime;
                if (time > GetComponentInParent<PlayerManager>().m_JumpTime)
                {
                    state = 2;
                }
                break;
        }
        time++;
        transform.position += aaa;
    }

    //private void LateUpdate()
    //{
    //    Debug.Log("<color=blue>b</color>");
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (enabled)
        {
            switch (collision.collider.tag)
            {
                case "Ground":
                    if (state == 2)
                    {
                        state = 0;
                        enabled = false;
                    }
                    break;
                case "Block":
                    if (state == 2)
                    {
                        state = 0;
                        enabled = false;
                    }
                    break;
                default:
                    Debug.Log("other collision enter");
                    break;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (enabled)
        {
            switch (collision.collider.tag)
            {
                case "Block":
                    if (state == 2)
                    {
                        state = 0;
                        enabled = false;
                    }
                    break;
                case "Ground":
                    break;
                default:
                    Debug.Log("other collision stay");
                    break;
            }
        }
    }

}
