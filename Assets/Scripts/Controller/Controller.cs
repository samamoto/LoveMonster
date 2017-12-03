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

    public enum Axis_Direction
    {
        Up = -1, Down = 1, Right = 1, Left = -1
    }

    public class Controller : MonoBehaviour
    {
        //デバッグスクリーンのプレハブ
        public GameObject m_DebugScreenPrefab = null;

        private enum DebugView
        {
            Raw,
            Threshold,
            Curve
        }

        [SerializeField] private DebugView m_AxisDebugView = DebugView.Raw;

        //このコントローラが持つデバッグスクリーン
        private GameObject m_DebugScreenClone = null;

        private SingleButton[] m_Button;
        private SingleAxis[] m_Axis;

        //コントローラのID
        public int m_ControllerID = 999;

        /// <summary>
        /// 軸入力の閾値
        /// </summary>
        public float m_Threshold = 0.5f;

        public AnimationCurve m_AxisCurve;

        //コントローラIDセット
        private void Start()
        {
            this.m_Button = new SingleButton[(int)Button.Max];
            this.m_Axis = new SingleAxis[(int)Axis.Max];

            if (this.m_ControllerID != 999 && ControllerGenerator.GenerateController(this.m_ControllerID, this))
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
            else if (this.m_ControllerID == 999)
            {
                Debug.LogWarning("Not setting controller ID");
            }
            else
            {
                this.m_ControllerID = 999;
            }
        }

        private void Update()
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
                {
                    float value = 0.0f;
                    switch (this.m_AxisDebugView)
                    {
                        case DebugView.Raw:
                            value = this.GetAxisRaw(i);
                            break;

                        case DebugView.Threshold:
                            value = this.GetAxisHold(i);
                            break;

                        case DebugView.Curve:
                            value = this.GetAxisCurve(i);
                            break;
                    }
                    this.m_DebugScreenClone.GetComponent<ControllerDebugScreen>().UpdateAxisScreen(i, value);
                }
            }
        }

        private void LateUpdate()
        {
            Button button = 0;
            Axis axis = 0;
            try
            {
                //ボタン更新
                for (button = 0; button < Button.Max; button++)
                {
                    this.m_Button[(int)button].Update();
                }
                for (axis = 0; axis < Axis.Max; axis++)
                {
                    this.m_Axis[(int)axis].Update();
                }
            }
            catch
            {
                Debug.LogError("Controller error button : " + button.ToString() + " axis : " + axis.ToString());
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
        /// Get axis raw param and truncate under threshold(-1.0f ~ 0.0f ~ 1.0f)
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public float GetAxisRawThreshold(Axis axis, float threshold)
        {
            if (this.m_ControllerID != 999)
            {
                float check = 0.0f;
                check = this.m_Axis[(int)axis].GetAxisRaw();
                if (check < 0 && check > -threshold || 0 < check && check < threshold)
                    check = 0.0f;
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
                check = this.m_Axis[(int)axis].GetAxisHold(this.m_Threshold);
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
                check = this.m_Axis[(int)axis].GetAxisDown(this.m_Threshold);
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
                check = this.m_Axis[(int)axis].GetAxisUp(this.m_Threshold);
                return check; ;
            }
            else
            {
                Debug.LogWarning("Not having controller");
                return 0;
            }
        }

        public float GetAxisCurve(Axis axis)
        {
            if (this.m_ControllerID != 999)
            {
                float value = 0;
                value = this.m_Axis[(int)axis].GetAxisCurve(this.m_AxisCurve);
                return value; ;
            }
            else
            {
                Debug.LogWarning("Not having controller");
                return 0;
            }
        }

        // 2017/11/14 oyama add
        /// <summary>
        /// 現在一台でもコントローラーが接続されているか調べる（デバッグ用途）
        /// </summary>
        public static bool GetConnectController()
        {
            // 接続されているコントローラの名前を調べる
            string[] controllerNames = Input.GetJoystickNames();
            if (controllerNames[0] == "" || controllerNames == null)
            {   // コントローラの名前が空っぽなら
                return false;               // 繋がってない
            }
            return true;
        }

        // 2017/11/14 oyama add
        /// <summary>
        /// 現在のコントローラーが接続されているか台数を調べる（デバッグ用途）
        /// </summary>
        public static int GetConnectControllers()
        {
            // 接続されているコントローラの名前を調べる
            string[] controllerNames = Input.GetJoystickNames();
            if (controllerNames.Length == 0 || controllerNames[0] == "" || controllerNames == null)
            { // コントローラの名前が空っぽなら
                return 0;               // 繋がってない
            }
            else
            {
                return controllerNames.Length;
            }
        }

        // 2017/11/14 oyama add
        /// <summary>
        /// 現在のコントローラーの名前を調べる（デバッグ用途）
        /// </summary>
        /// <param name="n">プレイヤーのID</param>
        public static string GetConnectControllerName(int n)
        {
            // 接続されているコントローラの名前を調べる
            string[] controllerNames = Input.GetJoystickNames();
            if (controllerNames.Length == 0 || controllerNames[0] == "" || controllerNames == null)
            {
                return "";
            }
            // 雑なエラー処理
            if (n == 0)
            {
                return controllerNames[n + 1];
            }
            else if (n > 0 && n < 5)
            {
                return controllerNames[n - 1];
            }
            return "";
        }
    }
}