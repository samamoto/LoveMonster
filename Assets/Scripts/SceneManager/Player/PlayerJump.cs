using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {
    int time = 0;
    int state;
    Vector3 velocity;
    AllPlayerManager playerManager;
    // Use this for initialization
    void Start() {
        playerManager = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
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
                velocity = Vector3.zero;
                velocity.z = 0.1f;
                state = 1;
                break;

            case 1:
                velocity.y += playerManager.m_JumpPower * Time.deltaTime;
                if (time > playerManager.m_JumpTime)
                {
                    state = 2;
                }
                break;
        }
        time++;
        transform.position += velocity;
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
