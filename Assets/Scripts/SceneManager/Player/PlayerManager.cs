using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class PlayerManager : MonoBehaviour {
    
    public PLAYER_ANIMATION_STATE AnimationState = PLAYER_ANIMATION_STATE.STAY;
    private Controller m_Controller;
    private CharacterController m_characterController;
    private Animator m_animator;

    static public int m_myNumberCount = 1;
    public int m_mynumber;

    public PlayerStay m_playerStay;
    public PlayerDash m_playerDash;
    public PlayerJump m_playerJump;
    public PlayerVault m_playerVault;
    public PlayerWallRun m_playerWallRun;


    // Use this for initialization
    void Start () {
        this.m_Controller = this.GetComponent<Controller>();
        m_characterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
        m_mynumber = m_myNumberCount;
        m_myNumberCount++;
    }

    // Update is called once per frame
    void Update()
    {
        ////仮で強制ダッシュ
        //if(m_playerDash.enabled==false)
        //{
        //    m_playerDash.enabled = true;
        //}


        PLAYER_ANIMATION_STATE workPAS = AnimationState;

        switch (AnimationState)
        {
            case PLAYER_ANIMATION_STATE.DUSH:
                if (/*this.m_Controller.GetButtonDown("X") ||*/
                    Input.GetKeyDown(KeyCode.Z))
                {
                    Debug.Log("Jump!");
                    m_playerDash.enabled = false;
                    m_playerJump.enabled = true;
                    workPAS = PLAYER_ANIMATION_STATE.JUMP;
                }
                if (/*this.m_Controller.GetButtonDown("A") || */
                    Input.GetKeyDown(KeyCode.V))
                {
                    m_playerDash.enabled = false;
                    m_playerVault.enabled = true;
                    workPAS = PLAYER_ANIMATION_STATE.VAULT;
                }
                if(/*this.m_Controller.GetButtonDown("LB") || */
                    Input.GetKeyDown(KeyCode.W))
                {
                    m_playerDash.enabled = false;
                    m_playerWallRun.enabled = true;
                    workPAS = PLAYER_ANIMATION_STATE.WALL_RUN;
                }

                if(m_playerDash.enabled==false)
                {
                    m_playerStay.enabled = true;
                    workPAS = PLAYER_ANIMATION_STATE.STAY;
                }

                break;

            case PLAYER_ANIMATION_STATE.JUMP:
                if (m_playerJump.enabled == false)
                {
                    m_playerDash.enabled = true;
                    workPAS = PLAYER_ANIMATION_STATE.DUSH;
                }
                break;

            case PLAYER_ANIMATION_STATE.VAULT:
                if (m_playerVault.enabled == false)
                {
                    m_playerDash.enabled = true;
                    workPAS = PLAYER_ANIMATION_STATE.DUSH;
                }
                break;

            case PLAYER_ANIMATION_STATE.WALL_RUN:
                if (m_playerWallRun.enabled == false)
                {
                    m_playerDash.enabled = true;
                    workPAS = PLAYER_ANIMATION_STATE.DUSH;
                }

                break;

            case PLAYER_ANIMATION_STATE.STAY:
                if(m_animator.GetFloat("Speed")>0.0f)
                {
                    m_playerStay.enabled = false;
                    m_playerDash.enabled = true;
                    workPAS = PLAYER_ANIMATION_STATE.DUSH;
                }

                break;
        }

        AnimationState = workPAS;

        //デバッグ処理
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_playerDash.enabled = true;
            m_playerJump.enabled = false;
            transform.position = Vector3.zero;
            AnimationState = PLAYER_ANIMATION_STATE.DUSH;
        }


        //if (this.m_Controller.GetButton("A"))
        //    Debug.Log("aです");
    }
}
