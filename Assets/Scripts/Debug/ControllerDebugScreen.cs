using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CONTROLLERSCEERN
{
    public enum Button_XBox
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

    public class ControllerDebugScreen : MonoBehaviour
    {
        private Transform[] m_debugUI;
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
            this.m_debugUI = new Transform[(int)Button_XBox.MAX];
            for (int i = 0; i < (int)Button_XBox.MAX; i++)
            {
                this.m_debugUI[i] = transform.Find(this.m_strDebugButtons[i]);
            }
        }

        public void ScreenButton(Button_XBox button_XBox, bool button)
        {
            //ボタンかどうかチェック
            if (Button_XBox.A <= button_XBox && button_XBox <= Button_XBox.RSTICK)
            {
                if (button)
                    this.m_debugUI[(int)Button_XBox.A].GetComponent<Image>().color = Color.gray;
                else
                    this.m_debugUI[(int)Button_XBox.A].GetComponent<Image>().color = Color.white;
            }
        }

        public void ScreenAxis(Button_XBox axis_XBox, float value)
        {
            //軸系かチェック
            if (Button_XBox.LSTICK_X <= axis_XBox && axis_XBox <= Button_XBox.DPAD_D)
            {
                string screen_str;
                switch (axis_XBox)
                {
                    case Button_XBox.LSTICK_X:
                        screen_str = "L Stick x : " + value.ToString("F2");
                        this.m_debugUI[(int)axis_XBox].GetComponent<Text>().text = screen_str;
                        break;

                    case Button_XBox.LSTICK_Y:
                        screen_str = "L Stick y : " + value.ToString("F2");
                        this.m_debugUI[(int)axis_XBox].GetComponent<Text>().text = screen_str;
                        break;

                    case Button_XBox.RSTICK_X:
                        screen_str = "R Stick x : " + value.ToString("F2");
                        this.m_debugUI[(int)axis_XBox].GetComponent<Text>().text = screen_str;
                        break;

                    case Button_XBox.RSTICK_Y:
                        screen_str = "L Stick y : " + value.ToString("F2");
                        this.m_debugUI[(int)axis_XBox].GetComponent<Text>().text = screen_str;
                        break;

                    case Button_XBox.TRIGGER:
                        this.m_debugUI[(int)axis_XBox].GetComponent<Slider>().value = value;
                        screen_str = "value : " + value.ToString("F2");
                        this.m_debugUI[(int)axis_XBox].Find("TRIGGER_VALUE").GetComponent<Text>().text = screen_str;
                        break;

                    case Button_XBox.DPAD_L:
                    case Button_XBox.DPAD_R:
                        if (value < 0.0f)
                            this.m_debugUI[(int)Button_XBox.DPAD_L].GetComponent<Image>().color = Color.gray;       //色変更(左)
                        else if (value > 0.0f)
                            this.m_debugUI[(int)Button_XBox.DPAD_R].GetComponent<Image>().color = Color.gray;       //色変更(右)
                        else
                        {
                            this.m_debugUI[(int)Button_XBox.DPAD_L].GetComponent<Image>().color = Color.white;      //0なら(無入力)白にする
                            this.m_debugUI[(int)Button_XBox.DPAD_R].GetComponent<Image>().color = Color.white;
                        }
                        break;

                    case Button_XBox.DPAD_U:
                    case Button_XBox.DPAD_D:
                        if (value > 0.0f)
                            this.m_debugUI[(int)Button_XBox.DPAD_U].GetComponent<Image>().color = Color.gray;       //色変更(上)
                        else if (value < 0.0f)
                            this.m_debugUI[(int)Button_XBox.DPAD_D].GetComponent<Image>().color = Color.gray;       //色変更(下)
                        else
                        {
                            this.m_debugUI[(int)Button_XBox.DPAD_U].GetComponent<Image>().color = Color.white;      //0なら(無入力)白にする
                            this.m_debugUI[(int)Button_XBox.DPAD_D].GetComponent<Image>().color = Color.white;
                        }
                        break;
                }
            }
        }
    }
}