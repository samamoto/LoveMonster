using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {
    //スクリプト群
    //private SceneChange m_ScreenChange;

    private InputManagerGenerator inputmanage;

    // Use this for initialization
    private void Start()
    {
		// カーソルを削除
		Cursor.visible = false;

		//m_ScreenChange = GameObject.Find("SceneChange").GetComponent<SceneChange>();
	}

	// Update is called once per frame
	private void Update()
    {
        //m_ScreenChange._DebugInput();

        if (Input.GetKeyUp(KeyCode.Space))  //スペースキーで
        {
			//SceneManager.LoadScene("GameSceneProto");//ゲームシーンへ移動
			SceneChange.Instance._SceneLoadGame();
        }

    }
}
