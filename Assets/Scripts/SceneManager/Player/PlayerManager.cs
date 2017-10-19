using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public partial class PlayerManager : MonoBehaviour {

    [SerializeField]
    private PLAYER_ANIMATION_STATE AnimationState = PLAYER_ANIMATION_STATE.DUSH;
    private Controller m_Controller;


    public PlayerDash m_playerDash;
    public PlayerJump m_playerJump;
    public PlayerVault m_playerVault;
    public PlayerWallRun m_playerWallRun;

    //走る速さ
    public float m_RunSpeed = 0.2f;
    public float m_SideRunSpeed = 0.05f;

    //ジャンプ力
    public float m_JumpPower = 1.0f;
    //ジャンプ時間
    public float m_JumpTime = 2;

    // Use this for initialization
    void Start () {
        this.m_Controller = this.GetComponent<Controller>();

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
                if (this.m_Controller.GetButtonDown("X") ||
                    Input.GetKeyDown(KeyCode.Z))
                {
                    m_playerDash.enabled = false;
                    m_playerJump.enabled = true;
                    workPAS = PLAYER_ANIMATION_STATE.JUMP;
                }
                if (Input.GetKeyDown(KeyCode.V))
                {
                    m_playerDash.enabled = false;
                    m_playerVault.enabled = true;
                    workPAS = PLAYER_ANIMATION_STATE.VAULT;
                }
                if(Input.GetKeyDown(KeyCode.W))
                {
                    m_playerDash.enabled = false;
                    m_playerWallRun.enabled = true;
                    workPAS = PLAYER_ANIMATION_STATE.WALL_RUN;
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
































#if UNITY_EDITOR
    /**
     * Inspector拡張クラス
     */
    [CustomEditor(typeof(Player))]
    public class CharacterEditor : Editor
    {
        PlayerManager Pm;

        // 経由用
        SerializedProperty animstate;

        void OnEnable()
        {
            // 基礎クラスからmyIntをSerializedPropertyで受け取る
            animstate = serializedObject.FindProperty("AnimationState");
        }


        public override void OnInspectorGUI()
        {
            Pm = target as PlayerManager;

            serializedObject.Update();
            //Pm.AnimationState = (PLAYER_ANIMATION_STATE)EditorGUILayout.EnumPopup(Pm.AnimationState);


            //EditorGUILayout.EnumPopup((PLAYER_ANIMATION_STATE)animstate.enumValueIndex);

            if (GUILayout.Button("元に戻す"))
            {
                //animstate.enumValueIndex = 0;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
