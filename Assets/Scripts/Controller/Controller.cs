using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public enum Axis
    {
        L_x, L_y, Trigger, R_x, R_y, Cross_x, Cross_y, Max
    }

    public enum Button
    {
        A, B, X, Y, LB, RB, View, Menu, LStick, RStick, Max
    }

    public class Controller : MonoBehaviour
    {
        //デバッグスクリーンのプレハブ
        public GameObject m_DebugScreenPrefab = null;

        //このコントローラが持つデバッグスクリーン
        private GameObject m_DebugScreenClone = null;

        private SingleButton[] m_Button;
        private SingleAxis[] m_Axis;

        //コントローラのID
        public int m_ControllerID = 999;

        /// <summary>
        /// 軸入力の閾値
        /// </summary>
        public float AxisThreshold = 0.5f;

        //コントローラIDセット
        private void Start()
        {
            this.m_Button = new SingleButton[(int)Button.Max];
            this.m_Axis = new SingleAxis[(int)Axis.Max];

            if (ControllerGenerator.GenerateController(this.m_ControllerID, this))
            {
                if (m_DebugScreenPrefab)
                {
                    //親(キャンバス)
                    Transform parent = GameObject.Find("DebugCanvas").transform;
                    if (parent)
                    {
                        //デバッグスクリーン生成
                        this.m_DebugScreenClone = Instantiate(this.m_DebugScreenPrefab, parent);
                        Vector3 cloneRect = this.m_DebugScreenClone.GetComponent<RectTransform>().sizeDelta;
                        //デバッグスクリーンの位置補正
                        this.m_DebugScreenClone.GetComponent<RectTransform>().position += new Vector3((cloneRect.x * 0.5f) + (cloneRect.x * (this.m_ControllerID - 1)), cloneRect.y * -0.5f, 0);
                        //名前変更(重複回避のため)
                        this.m_DebugScreenClone.name = "Controller" + this.m_ControllerID.ToString() + "_" + "DebugScreen";
                        //デバッグスクリーン初期化
                        this.m_DebugScreenClone.GetComponent<ControllerDebugScreen>().DebugInit();
                    }
                }
            }
            else
            {
                this.m_ControllerID = 999;
                Debug.LogError("コントローラ取得失敗");
            }
        }

        private void Update()
        {
            if (this.m_DebugScreenClone)
            {
                //デバッグモード
                for (Button i = 0; i < Button.Max; i++)
                {
                    bool check = this.GetButtonHold(i);
                    if (this.m_DebugScreenClone && check)
                    {
                        this.m_DebugScreenClone.GetComponent<ControllerDebugScreen>().UpdateButtonScreen(i);
                    }
                }

                for (Axis i = 0; i < Axis.Max; i++)
                {
                    if (this.m_DebugScreenClone)
                        this.m_DebugScreenClone.GetComponent<ControllerDebugScreen>().UpdateAxisScreen(i, this.GetAxisRaw(i));
                }
            }
        }

        private void LateUpdate()
        {
            //ボタン更新
            for (Button i = 0; i < Button.Max; i++)
            {
                this.m_Button[(int)i].Update();
            }
            for (Axis i = 0; i < Axis.Max; i++)
            {
                this.m_Axis[(int)i].Update();
            }
        }

        /// <summary>
        /// this function use ControllerGenerator only
        /// </summary>
        /// <param name="button"></param>
        /// <param name="name"></param>
        public void SetButton(Button button, string name)
        {
            this.m_Button[(int)button] = new SingleButton(button, name);
        }

        /// <summary>
        /// this function use ControllerGenerator only
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="name"></param>
        public void SetAxis(Axis axis, string name)
        {
            this.m_Axis[(int)axis] = new SingleAxis(axis, name);
        }

        /// <summary>
        /// Get button hold
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool GetButtonHold(Button button)
        {
            if (this.m_ControllerID != 999)
            {
                bool check = false;
                check = this.m_Button[(int)button].GetButtonHold();

                return check;
            }
            else
            {
                Debug.LogWarning("Not having controller");
                return false;
            }
        }

        /// <summary>
        /// Get button trigger
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool GetButtonDown(Button button)
        {
            if (this.m_ControllerID != 999)
            {
                bool check = false;
                check = this.m_Button[(int)button].GetButtonDown();
                return check;
            }
            else
            {
                Debug.LogWarning("Not having controller");
                return false;
            }
        }

        /// <summary>
        /// Get button release
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool GetButtonUp(Button button)
        {
            if (this.m_ControllerID != 999)
            {
                bool check = false;
                check = this.m_Button[(int)button].GetButtonUp();
                return check;
            }
            else
            {
                Debug.LogWarning("Not having controller");
                return false;
            }
        }

        /// <summary>
        /// Get axis raw param(-1.0f ~ 0.0f ~ 1.0f)
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public float GetAxisRaw(Axis axis)
        {
            if (this.m_ControllerID != 999)
            {
                float check = 0.0f;
                check = this.m_Axis[(int)axis].GetAxisRaw();
                return check;
            }
            else
            {
                Debug.LogWarning("Not having controller");
                return 0.0f;
            }
        }

        /// <summary>
        /// Get axis hold(-1|| 0 || 1)
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public int GetAxisHold(Axis axis)
        {
            if (this.m_ControllerID != 999)
            {
                int check = 0;
                check = this.m_Axis[(int)axis].GetAxisHold(this.AxisThreshold);
                return check;
            }
            else
            {
                Debug.LogWarning("Not having controller");
                return 0;
            }
        }

        /// <summary>
        /// Get axis trigger (-1 || 0 || 1)
        /// </summary>
        /// <param name="axis"></param>
        /// <returns>-1 || 0 || 1</returns>
        public int GetAxisDown(Axis axis)
        {
            if (this.m_ControllerID != 999)
            {
                int check = 0;
                check = this.m_Axis[(int)axis].GetAxisDown(this.AxisThreshold);
                return check;
            }
            else
            {
                Debug.LogWarning("Not having controller");
                return 0;
            }
        }

        /// <summary>
        /// Get axis release(-1 || 0 || 1)
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public int GetAxisUp(Axis axis)
        {
            if (this.m_ControllerID != 999)
            {
                int check = 0;
                check = this.m_Axis[(int)axis].GetAxisUp(this.AxisThreshold);
                return check; ;
            }
            else
            {
                Debug.LogWarning("Not having controller");
                return 0;
            }
        }
    }
}