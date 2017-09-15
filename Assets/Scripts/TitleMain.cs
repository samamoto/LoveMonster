using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMain : MonoBehaviour {

    //スクリプト群
    SceneChange m_ScreenChange;

    // Use this for initialization
    void Start () {
        m_ScreenChange = GameObject.Find("SceneChange").GetComponent<SceneChange>();
    }

    // Update is called once per frame
    void Update()
    {
        m_ScreenChange._DebugInput();
    }

    
}
