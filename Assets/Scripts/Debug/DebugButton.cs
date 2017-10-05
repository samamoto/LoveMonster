using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    LSTICK,
    RSTICK,
    LSTICK_X,
    LSTICK_Y,
    RSTICK_X,
    RSTICK_Y,
    TRIGGER,
    DPAD_L,
    DPAD_R,
    DPAD_U,
    DPAD_D,
    MAX
}

public class DebugButton : MonoBehaviour
{
    private GameObject[] m_debugUI;

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
        "MENU",
        "LSTICK",
        "RSTICK",
        "LSTICK_X",
        "LSTICK_Y",
        "RSTICK_X",
        "RSTICK_Y",
        "TRIGGER",
        "DPAD_L",
        "DPAD_R",
        "DPAD_U",
        "DPAD_D"
    };

    // Use this for initialization
    private void Start()
    {
        this.m_debugUI = new GameObject[(int)Button_XBox.MAX];
        for (int i = 0; i < (int)Button_XBox.MAX; i++)
        {
            this.m_debugUI[i] = GameObject.Find(this.m_strDebugButtons[i]).gameObject;
        }
        this.m_inputManage = new InputManagerGenerator();
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_A", 0, "joystick button 0", null));         //A
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_B", 0, "joystick button 1", null));         //B
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_X", 0, "joystick button 2", null));         //X
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_Y", 0, "joystick button 3", null));         //Y
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_LB", 0, "joystick button 4", null));        //LB
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_RB", 0, "joystick button 5", null));        //RB
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_View", 0, "joystick button 6", null));      //View
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_Menu", 0, "joystick button 7", null));      //Menu
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_LStick", 0, "joystick button 8", null));    //LStickButton
        this.m_inputManage.AddAxis(InputAxis.CreatePadButton("Debug_RStick", 0, "joystick button 9", null));    //RstickButton
        this.m_inputManage.AddAxis(InputAxis.CreatePadAxis("Debug_LStick_X", 0, 1));                            //LStick X
        this.m_inputManage.AddAxis(InputAxis.CreatePadAxis("Debug_LStick_Y", 0, 2));                            //LStick Y
        this.m_inputManage.AddAxis(InputAxis.CreatePadAxis("Debug_Trigger", 0, 3));                             //LT&RT
        this.m_inputManage.AddAxis(InputAxis.CreatePadAxis("Debug_RStick_X", 0, 4));                            //RStick X
        this.m_inputManage.AddAxis(InputAxis.CreatePadAxis("Debug_RStick_Y", 0, 5));                            //RStick Y
        this.m_inputManage.AddAxis(InputAxis.CreatePadAxis("Debug_DPad_X", 0, 6));                              //DPad X
        this.m_inputManage.AddAxis(InputAxis.CreatePadAxis("Debug_DPad_Y", 0, 7));                              //DPad Y
    }

    // Update is called once per frame
    private void Update()
    {
        //Aボタン
        if (Input.GetButton("Debug_A"))
        {
            this.m_debugUI[(int)Button_XBox.A].GetComponent<Image>().color = Color.gray;
            if (Input.GetButtonDown("Debug_A"))
                Debug.Log("Pressed A");
        }
        else
        {
            this.m_debugUI[(int)Button_XBox.A].GetComponent<Image>().color = Color.white;
        }

        //Bボタン
        if (Input.GetButton("Debug_B"))
        {
            this.m_debugUI[(int)Button_XBox.B].GetComponent<Image>().color = Color.gray;
            if (Input.GetButtonDown("Debug_B"))
                Debug.Log("Pressed B");
        }
        else
        {
            this.m_debugUI[(int)Button_XBox.B].GetComponent<Image>().color = Color.white;
        }

        //Xボタン
        if (Input.GetButton("Debug_X"))
        {
            this.m_debugUI[(int)Button_XBox.X].GetComponent<Image>().color = Color.gray;
            if (Input.GetButtonDown("Debug_X"))
                Debug.Log("Pressed X");
        }
        else
        {
            this.m_debugUI[(int)Button_XBox.X].GetComponent<Image>().color = Color.white;
        }

        //Yボタン
        if (Input.GetButton("Debug_Y"))
        {
            this.m_debugUI[(int)Button_XBox.Y].GetComponent<Image>().color = Color.gray;
            if (Input.GetButtonDown("Debug_Y"))
                Debug.Log("Pressed Y");
        }
        else
        {
            this.m_debugUI[(int)Button_XBox.Y].GetComponent<Image>().color = Color.white;
        }

        //Lボタン
        if (Input.GetButton("Debug_LB"))
        {
            this.m_debugUI[(int)Button_XBox.LB].GetComponent<Image>().color = Color.gray;
            if (Input.GetButtonDown("Debug_LB"))
                Debug.Log("Pressed LB");
        }
        else
        {
            this.m_debugUI[(int)Button_XBox.LB].GetComponent<Image>().color = Color.white;
        }

        //Rボタン
        if (Input.GetButton("Debug_RB"))
        {
            this.m_debugUI[(int)Button_XBox.RB].GetComponent<Image>().color = Color.gray;
            if (Input.GetButtonDown("Debug_RB"))
                Debug.Log("Pressed RB");
        }
        else
        {
            this.m_debugUI[(int)Button_XBox.RB].GetComponent<Image>().color = Color.white;
        }

        //ビューボタン
        if (Input.GetButton("Debug_View"))
        {
            this.m_debugUI[(int)Button_XBox.VIEW].GetComponent<Image>().color = Color.gray;
            if (Input.GetButtonDown("Debug_View"))
                Debug.Log("Pressed View");
        }
        else
        {
            this.m_debugUI[(int)Button_XBox.VIEW].GetComponent<Image>().color = Color.white;
        }

        //メニューボタン
        if (Input.GetButton("Debug_Menu"))
        {
            this.m_debugUI[(int)Button_XBox.MENU].GetComponent<Image>().color = Color.gray;
            if (Input.GetButtonDown("Debug_Menu"))
                Debug.Log("Pressed Menu");
        }
        else
        {
            this.m_debugUI[(int)Button_XBox.MENU].GetComponent<Image>().color = Color.white;
        }

        //Lスティックボタン
        if (Input.GetButton("Debug_LStick"))
        {
            this.m_debugUI[(int)Button_XBox.LSTICK].GetComponent<Image>().color = Color.gray;
            if (Input.GetButtonDown("Debug_LStick"))
                Debug.Log("Pressed LStick");
        }
        else
        {
            this.m_debugUI[(int)Button_XBox.LSTICK].GetComponent<Image>().color = Color.white;
        }

        //Rスティックボタン
        if (Input.GetButton("Debug_RStick"))
        {
            this.m_debugUI[(int)Button_XBox.RSTICK].GetComponent<Image>().color = Color.gray;
            if (Input.GetButtonDown("Debug_RStick"))
                Debug.Log("Pressed RStick");
        }
        else
        {
            this.m_debugUI[(int)Button_XBox.RSTICK].GetComponent<Image>().color = Color.white;
        }

        //Lスティック==================================================================================
        float lstick_x = 0.0f;
        lstick_x = Input.GetAxis("Debug_LStick_X");
        string str_lstick_x = "L Stick x : " + lstick_x.ToString("F2");
        this.m_debugUI[(int)Button_XBox.LSTICK_X].GetComponent<Text>().text = str_lstick_x;

        float lstick_y = 0.0f;
        lstick_y = Input.GetAxis("Debug_LStick_Y");
        string str_lstick_y = "L Stick y : " + lstick_y.ToString("F2");
        this.m_debugUI[(int)Button_XBox.LSTICK_Y].GetComponent<Text>().text = str_lstick_y;
        //Lスティック==================================================================================

        //Rスティック==================================================================================
        float rstick_x = 0.0f;
        rstick_x = Input.GetAxis("Debug_RStick_X");
        string str_rstick_x = "R Stick x : " + rstick_x.ToString("F2");
        this.m_debugUI[(int)Button_XBox.RSTICK_X].GetComponent<Text>().text = str_rstick_x;

        float rstick_y = 0.0f;
        rstick_y = Input.GetAxis("Debug_RStick_Y");
        string str_rstick_y = "R Stick y : " + rstick_y.ToString("F2");
        this.m_debugUI[(int)Button_XBox.RSTICK_Y].GetComponent<Text>().text = str_rstick_y;
        //Rスティック==================================================================================

        //トリガー=====================================================================================
        float trigger = 0.0f;
        trigger = Input.GetAxis("Debug_Trigger");
        this.m_debugUI[(int)Button_XBox.TRIGGER].GetComponent<Slider>().value = trigger;
        string str_trigger = "Value : " + trigger.ToString("F2");
        GameObject.Find("TRIGGER_VALUE").GetComponent<Text>().text = str_trigger;
        //トリガー=====================================================================================

        //十字キー=====================================================================================
        //左右*****************************************************************************************
        float dpad_x = 0.0f;
        dpad_x = Input.GetAxis("Debug_DPad_X");
        if (dpad_x < 0.0f)
            this.m_debugUI[(int)Button_XBox.DPAD_L].GetComponent<Image>().color = Color.gray;
        else if (dpad_x > 0.0f)
            this.m_debugUI[(int)Button_XBox.DPAD_R].GetComponent<Image>().color = Color.gray;
        else
        {
            this.m_debugUI[(int)Button_XBox.DPAD_L].GetComponent<Image>().color = Color.white;
            this.m_debugUI[(int)Button_XBox.DPAD_R].GetComponent<Image>().color = Color.white;
        }
        //左右*****************************************************************************************
        //上下*****************************************************************************************
        float dpad_y = 0.0f;
        dpad_y = Input.GetAxis("Debug_DPad_Y");
        if (dpad_y > 0.0f)
            this.m_debugUI[(int)Button_XBox.DPAD_U].GetComponent<Image>().color = Color.gray;
        else if (dpad_y < 0.0f)
            this.m_debugUI[(int)Button_XBox.DPAD_D].GetComponent<Image>().color = Color.gray;
        else
        {
            this.m_debugUI[(int)Button_XBox.DPAD_U].GetComponent<Image>().color = Color.white;
            this.m_debugUI[(int)Button_XBox.DPAD_D].GetComponent<Image>().color = Color.white;
        }
        //上下*****************************************************************************************
        //十字キー=====================================================================================
    }
}