using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour {
    //スクリプト群
    private SceneChange m_ScreenChange;

    private InputManagerGenerator inputmanage;

    // Use this for initialization
    private void Start()
    {
        m_ScreenChange = GetComponent<SceneChange>();
    }

    // Update is called once per frame
    private void Update()
    {
        m_ScreenChange._DebugInput();
    }

	/// <summary>
	/// プレイヤーが死んだ、落ちたときにする処理
	/// </summary>
	private void PlayerDead() {
		
	}

}
