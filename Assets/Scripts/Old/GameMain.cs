using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    //スクリプト群
    private SceneChange m_ScreenChange;

    private InputManagerGenerator inputmanage;

    // Use this for initialization
    private void Start()
    {
        m_ScreenChange = GameObject.Find("SceneChange").GetComponent<SceneChange>();
    }

    // Update is called once per frame
    private void Update()
    {
        m_ScreenChange._DebugInput();
    }
}