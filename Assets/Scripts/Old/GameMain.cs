using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    //スクリプト群
    private SceneChange m_ScreenChange;

    private Controller.Controller m_Controller;

    // Use this for initialization
    private void Start()
    {
        m_ScreenChange = GameObject.Find("SceneChange").GetComponent<SceneChange>();
        this.m_Controller = new Controller.Controller();
    }

    // Update is called once per frame
    private void Update()
    {
        m_ScreenChange._DebugInput();

        if (this.m_Controller.GetButtonDown(Controller.Button.A))
        {
            Debug.Log("Pressed A");
        }
        if (this.m_Controller.GetButtonDown(Controller.Button.B))
        {
            Debug.Log("Pressed B");
        }
        if (this.m_Controller.GetButtonDown(Controller.Button.X))
        {
            Debug.Log("Pressed X");
        }
        if (this.m_Controller.GetButtonDown(Controller.Button.Y))
        {
            Debug.Log("Pressed Y");
        }
        if (this.m_Controller.GetButtonDown(Controller.Button.LB))
        {
            Debug.Log("Pressed LB");
        }
        if (this.m_Controller.GetButtonDown(Controller.Button.RB))
        {
            Debug.Log("Pressed RB");
        }
    }
}