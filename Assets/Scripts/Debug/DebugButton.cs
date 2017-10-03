using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Button_XBox
{
    A,
    B,
    X,
    Y,
    LB,
    RB,
    MENU,
    VIEW,
    MAX
}

public class DebugButton : MonoBehaviour
{
    private GameObject[] m_debugButtons;

    private InputManagerGenerator m_inputManage;

    private string[] m_strDebugButtons =
    {
        "A",
        "B",
        "X",
        "Y",
        "LB",
        "RB",
        "VIEW",
        "MENU"
    };

    // Use this for initialization
    private void Start()
    {
        this.m_debugButtons = new GameObject[(int)Button_XBox.MAX];
        for (int i = 0; i < (int)Button_XBox.MAX; i++)
        {
            this.m_debugButtons[i] = GameObject.Find(this.m_strDebugButtons[i]).gameObject;
        }
        this.m_inputManage = new InputManagerGenerator();
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_A", 0, "joystick button 0", null));
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_B", 0, "joystick button 1", null));
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_X", 0, "joystick button 2", null));
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_Y", 0, "joystick button 3", null));
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_LB", 0, "joystick button 4", null));
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_RB", 0, "joystick button 5", null));
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_View", 0, "joystick button 6", null));
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_Menu", 0, "joystick button 7", null));
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Debug_A"))
        {
            Debug.Log("Pressed A");
        }
        if (Input.GetButtonDown("Debug_B"))
        {
            Debug.Log("Pressed B");
        }
        if (Input.GetButtonDown("Debug_X"))
        {
            Debug.Log("Pressed X");
        }
        if (Input.GetButtonDown("Debug_Y"))
        {
            Debug.Log("Pressed Y");
        }
        if (Input.GetButtonDown("Debug_LB"))
        {
            Debug.Log("Pressed LB");
        }
        if (Input.GetButtonDown("Debug_RB"))
        {
            Debug.Log("Pressed RB");
        }
        if (Input.GetButtonDown("Debug_View"))
        {
            Debug.Log("Pressed View");
        }
        if (Input.GetButtonDown("Debug_Menu"))
        {
            Debug.Log("Pressed Menu");
        }
    }
}