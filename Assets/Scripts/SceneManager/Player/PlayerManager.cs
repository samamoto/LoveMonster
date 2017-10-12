using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public partial class PlayerManager : MonoBehaviour {

    [SerializeField]
    private PLAYER_ANIMATION_STATE AnimationState = PLAYER_ANIMATION_STATE.STAY;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
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
