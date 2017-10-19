using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //デバッグスクリーンのプレハブ
    public GameObject m_DebugScreenPrefab = null;

    //このコントローラが持つデバッグスクリーン
    private GameObject m_DebugScreenClone = null;

    //コントローラのID
    public int m_ControllerID = 999;

    private string str_Controller = "Controller";

    //コントローラIDセット
    private void Start()
    {
        if (ControllerGenerator.GenerateController(this.m_ControllerID))
        {
            this.str_Controller += this.m_ControllerID.ToString() + "_";
            if (m_DebugScreenPrefab)
            {
                //デバッグスクリーン生成
                this.m_DebugScreenClone = Instantiate(this.m_DebugScreenPrefab, GameObject.Find("DebugCanvas").transform);
                //位置補正
                Vector3 pos = new Vector3(
                    (this.m_DebugScreenClone.GetComponent<RectTransform>().sizeDelta.x * 0.5f) + ((this.m_DebugScreenClone.GetComponent<RectTransform>().sizeDelta.x * (this.m_ControllerID - 1))),
                    (this.m_DebugScreenClone.GetComponent<RectTransform>().sizeDelta.y * -0.5f)
                    );
                this.m_DebugScreenClone.transform.position += pos;
                //名前変更(重複回避のため)
                this.m_DebugScreenClone.name = this.str_Controller + "DebugScreen";
                //デバッグスクリーン初期化
                this.m_DebugScreenClone.GetComponent<ControllerDebugScreen>().DebugInit();
            }
        }
        else
        {
            this.m_ControllerID = 999;
            Debug.LogError("コントローラ取得失敗");
        }
    }

    public bool GetButton(string button)
    {
        if (this.m_ControllerID != 999)
        {
            bool check = Input.GetButton(str_Controller + button);
            if (this.m_DebugScreenClone && check)
            {
                this.m_DebugScreenClone.GetComponent<ControllerDebugScreen>().UpdateButtonScreen(button);
            }
            return check;
        }
        else
        {
            Debug.LogWarning("Not having controller");
            return false;
        }
    }

    public bool GetButtonDown(string button)
    {
        if (this.m_ControllerID != 999)
        {
            bool check = Input.GetButtonDown(str_Controller + button);
            if (this.m_DebugScreenClone && check)
            {
                Debug.Log("Pressed down " + str_Controller + button);
                this.m_DebugScreenClone.GetComponent<ControllerDebugScreen>().UpdateButtonScreen(button);
            }
            return check;
        }
        else
        {
            Debug.LogWarning("Not having controller");
            return false;
        }
    }

    public bool GetButtonUp(string button)
    {
        if (this.m_ControllerID != 999)
        {
            bool check = Input.GetButtonUp(str_Controller + button);
            if (this.m_DebugScreenClone && check)
            {
                Debug.Log("Pressed up " + str_Controller + button);
                this.m_DebugScreenClone.GetComponent<ControllerDebugScreen>().UpdateButtonScreen(button);
            }
            return check;
        }
        else
        {
            Debug.LogWarning("Not having controller");
            return false;
        }
    }

    public float GetAxis(string axis)
    {
        if (this.m_ControllerID != 999)
        {
            float check = Input.GetAxis(str_Controller + axis);
            if (this.m_DebugScreenClone)
                this.m_DebugScreenClone.GetComponent<ControllerDebugScreen>().UpdateAxisScreen(axis, check);
            return check;
        }
        else
        {
            Debug.LogWarning("Not having controller");
            return 0.0f;
        }
    }
}