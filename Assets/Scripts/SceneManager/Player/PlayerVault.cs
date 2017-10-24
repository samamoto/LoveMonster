using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVault : MonoBehaviour {

    int state = 0;
    int vaultstate = 0;
    Vector3 velocity;
    AllPlayerManager playerManager;
	// Use this for initialization
	void Start () {
        playerManager = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
        //this.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        switch (state)
        {
            case 0://条件を満たしていない
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    velocity.x = -playerManager.m_SideRunSpeed;
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    velocity.x = playerManager.m_SideRunSpeed;
                }
                else
                {
                    velocity.x = 0.0f;
                }
                this.enabled = false;
                break;
            case 1:
                Debug.Log("ボルト！");
                if(Vault())
                {
                    state = 0;
                }
                break;
        }

        transform.position += velocity;
        ////終了
        //this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Vault":
                state = 1;
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Vault":
                state = 0;
                break;
        }
    }

    Vector3 transOld=Vector3.zero;
    bool Vault()
    {
        Vector3 velocity=Vector3.zero;
        switch (vaultstate)
        {
            case 0:
                transOld = transform.position;
                vaultstate = 1;
                break;

            case 1:
                velocity.y = 0.3f;
                if (transform.position.y > transOld.y+1.0f)
                {
                    velocity.y = 0.0f;
                    vaultstate = 2;
                }
                break;

            case 2:
                velocity.z += 0.5f;
                if (velocity.z > transOld.z+1.0f)
                {
                    velocity.z = 0.0f;
                    vaultstate = 0;
                    return true;
                }
                break;

            default:
                break;
        }
        transform.position += velocity;
        return false;
    }

}

