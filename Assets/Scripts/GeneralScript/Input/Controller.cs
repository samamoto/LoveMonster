using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject m_DebugWindow;
    private int m_ControllerID = 999;
    private string str_Controller = "Player";

    public bool SetID(int id)
    {
        if (this.m_ControllerID == 999)
        {
            this.m_ControllerID = id;
            this.str_Controller = this.str_Controller + this.m_ControllerID.ToString() + "_";
            return true;
        }
        else
        {
            Debug.LogWarning("Already exist ID");
            return false;
        }
    }

    public bool GetButton(string button)
    {
        if (this.m_ControllerID != 999)
        {
            return Input.GetButton(str_Controller + button);
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
            return Input.GetButtonDown(str_Controller + button);
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
            return Input.GetButtonUp(str_Controller + button);
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
            return Input.GetAxis(str_Controller + axis);
        }
        else
        {
            Debug.LogWarning("Not having controller");
            return 0.0f;
        }
    }
}