using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct DebugUpdateStruct
{
    public bool A;
    public bool B;
    public bool X;
    public bool Y;
    public bool LB;
    public bool RB;
    public bool View;
    public bool Menu;
    public bool LStick;
    public bool RStick;
    public Vector2 LStick_Axis;
    public Vector2 RStick_Axis;
    public float Trigger;
    public Vector2 DPad;

    public void Clear()
    {
        A = B = X = Y = LB = RB = View = Menu = LStick = RStick = false;
        LStick_Axis = RStick_Axis = DPad = Vector2.zero;
        Trigger = 0.0f;
    }
}

public class ControllerDebugScreen : MonoBehaviour
{
    private Transform[] m_debugUI;

    //UI名列挙型
    private enum enumDbgObjName
    {
        A,
        B,
        X,
        Y,
        LB,
        RB,
        VIEW,
        MENU,
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

    //UI名リスト
    private string[] m_strDbgObjName =
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

    private DebugUpdateStruct m_ScreenUpdator;

    public void DebugInit()
    {
        //UI取得
        this.m_debugUI = new Transform[(int)enumDbgObjName.MAX];
        for (int i = 0; i < (int)enumDbgObjName.MAX; i++)
        {
            this.m_debugUI[i] = this.transform.Find(this.m_strDbgObjName[i]);
        }
        this.m_ScreenUpdator = new DebugUpdateStruct();
        this.m_ScreenUpdator.Clear();
    }

    public void UpdateButtonScreen(Controller.Button button)
    {
        switch (button)
        {
            case Controller.Button.A:
                this.m_ScreenUpdator.A = true;
                break;

            case Controller.Button.B:
                this.m_ScreenUpdator.B = true;
                break;

            case Controller.Button.X:
                this.m_ScreenUpdator.X = true;
                break;

            case Controller.Button.Y:
                this.m_ScreenUpdator.Y = true;
                break;

            case Controller.Button.LB:
                this.m_ScreenUpdator.LB = true;
                break;

            case Controller.Button.RB:
                this.m_ScreenUpdator.RB = true;
                break;

            case Controller.Button.View:
                this.m_ScreenUpdator.View = true;
                break;

            case Controller.Button.Menu:
                this.m_ScreenUpdator.Menu = true;
                break;

            case Controller.Button.LStick:
                this.m_ScreenUpdator.LStick = true;
                break;

            case Controller.Button.RStick:
                this.m_ScreenUpdator.RStick = true;
                break;
        }
    }

    public void UpdateAxisScreen(Controller.Axis axis, float value)
    {
        switch (axis)
        {
            case Controller.Axis.L_x:
                this.m_ScreenUpdator.LStick_Axis.x = value;
                break;

            case Controller.Axis.L_y:
                this.m_ScreenUpdator.LStick_Axis.y = value;
                break;

            case Controller.Axis.Trigger:
                this.m_ScreenUpdator.Trigger = value;
                break;

            case Controller.Axis.R_x:
                this.m_ScreenUpdator.RStick_Axis.x = value;
                break;

            case Controller.Axis.R_y:
                this.m_ScreenUpdator.RStick_Axis.y = value;
                break;

            case Controller.Axis.Cross_x:
                this.m_ScreenUpdator.DPad.x = value;
                break;

            case Controller.Axis.Cross_y:
                this.m_ScreenUpdator.DPad.y = value;
                break;
        }
    }

    private void LateUpdate()
    {
        //きたねぇ...
        //ボタン=====================================================================================
        //A更新
        if (this.m_ScreenUpdator.A)
            this.m_debugUI[(int)enumDbgObjName.A].GetComponent<Image>().color = Color.gray;
        else
            this.m_debugUI[(int)enumDbgObjName.A].GetComponent<Image>().color = Color.white;
        //B更新
        if (this.m_ScreenUpdator.B)
            this.m_debugUI[(int)enumDbgObjName.B].GetComponent<Image>().color = Color.gray;
        else
            this.m_debugUI[(int)enumDbgObjName.B].GetComponent<Image>().color = Color.white;
        //X更新
        if (this.m_ScreenUpdator.X)
            this.m_debugUI[(int)enumDbgObjName.X].GetComponent<Image>().color = Color.gray;
        else
            this.m_debugUI[(int)enumDbgObjName.X].GetComponent<Image>().color = Color.white;
        //Y更新
        if (this.m_ScreenUpdator.Y)
            this.m_debugUI[(int)enumDbgObjName.Y].GetComponent<Image>().color = Color.gray;
        else
            this.m_debugUI[(int)enumDbgObjName.Y].GetComponent<Image>().color = Color.white;
        //LB更新
        if (this.m_ScreenUpdator.LB)
            this.m_debugUI[(int)enumDbgObjName.LB].GetComponent<Image>().color = Color.gray;
        else
            this.m_debugUI[(int)enumDbgObjName.LB].GetComponent<Image>().color = Color.white;
        //RB更新
        if (this.m_ScreenUpdator.RB)
            this.m_debugUI[(int)enumDbgObjName.RB].GetComponent<Image>().color = Color.gray;
        else
            this.m_debugUI[(int)enumDbgObjName.RB].GetComponent<Image>().color = Color.white;
        //View更新
        if (this.m_ScreenUpdator.View)
            this.m_debugUI[(int)enumDbgObjName.VIEW].GetComponent<Image>().color = Color.gray;
        else
            this.m_debugUI[(int)enumDbgObjName.VIEW].GetComponent<Image>().color = Color.white;
        //Menu更新
        if (this.m_ScreenUpdator.Menu)
            this.m_debugUI[(int)enumDbgObjName.MENU].GetComponent<Image>().color = Color.gray;
        else
            this.m_debugUI[(int)enumDbgObjName.MENU].GetComponent<Image>().color = Color.white;
        //LStick更新
        if (this.m_ScreenUpdator.LStick)
            this.m_debugUI[(int)enumDbgObjName.LSTICK].GetComponent<Image>().color = Color.gray;
        else
            this.m_debugUI[(int)enumDbgObjName.LSTICK].GetComponent<Image>().color = Color.white;
        //RStick更新
        if (this.m_ScreenUpdator.RStick)
            this.m_debugUI[(int)enumDbgObjName.RSTICK].GetComponent<Image>().color = Color.gray;
        else
            this.m_debugUI[(int)enumDbgObjName.RSTICK].GetComponent<Image>().color = Color.white;
        //ボタン=====================================================================================

        //軸========================================================================================
        Vector2 axisValue;
        string strAxis;

        //トリガー
        float trigger = this.m_ScreenUpdator.Trigger;
        this.m_debugUI[(int)enumDbgObjName.TRIGGER].GetComponent<Slider>().value = trigger;
        this.m_debugUI[(int)enumDbgObjName.TRIGGER].Find("TRIGGER_VALUE").GetComponent<Text>().text = "Value : " + trigger.ToString("F2");

        //Lスティック
        axisValue = this.m_ScreenUpdator.LStick_Axis;
        strAxis = "L Stick ";
        this.m_debugUI[(int)enumDbgObjName.LSTICK_X].GetComponent<Text>().text = strAxis + "x : " + axisValue.x.ToString("F2");
        this.m_debugUI[(int)enumDbgObjName.LSTICK_Y].GetComponent<Text>().text = strAxis + "y : " + axisValue.y.ToString("F2");

        //Rスティック
        axisValue = this.m_ScreenUpdator.RStick_Axis;
        strAxis = "R Stick ";
        this.m_debugUI[(int)enumDbgObjName.RSTICK_X].GetComponent<Text>().text = strAxis + "x : " + axisValue.x.ToString("F2");
        this.m_debugUI[(int)enumDbgObjName.RSTICK_Y].GetComponent<Text>().text = strAxis + "y : " + axisValue.y.ToString("F2");

        //Dパッド
        axisValue = this.m_ScreenUpdator.DPad;
        //左右
        if (axisValue.x < 0.0f)
            this.m_debugUI[(int)enumDbgObjName.DPAD_L].GetComponent<Image>().color = Color.gray;
        else if (axisValue.x > 0.0f)
            this.m_debugUI[(int)enumDbgObjName.DPAD_R].GetComponent<Image>().color = Color.gray;
        else
        {
            this.m_debugUI[(int)enumDbgObjName.DPAD_L].GetComponent<Image>().color = Color.white;
            this.m_debugUI[(int)enumDbgObjName.DPAD_R].GetComponent<Image>().color = Color.white;
        }

        //上下
        if (axisValue.y < 0.0f)
            this.m_debugUI[(int)enumDbgObjName.DPAD_D].GetComponent<Image>().color = Color.gray;
        else if (axisValue.y > 0.0f)
            this.m_debugUI[(int)enumDbgObjName.DPAD_U].GetComponent<Image>().color = Color.gray;
        else
        {
            this.m_debugUI[(int)enumDbgObjName.DPAD_D].GetComponent<Image>().color = Color.white;
            this.m_debugUI[(int)enumDbgObjName.DPAD_U].GetComponent<Image>().color = Color.white;
        }
        //軸========================================================================================
        //ボタン情報クリア
        this.m_ScreenUpdator.Clear();
    }
}