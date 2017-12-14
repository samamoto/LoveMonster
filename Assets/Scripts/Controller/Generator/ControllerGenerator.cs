using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ControllerGenerator : MonoBehaviour
{
    private static InputManagerGenerator ms_InputGenerator;
    private static bool ms_bInitedCommonController = false;
    private static bool[] ms_bGaneratedController = { false, false, false, false };
    private static bool[] ms_RegisteredController = { false, false, false, false };

    //ボタン名
    private static string[] strButton =
    {
        "A",
        "B",
        "X",
        "Y",
        "LB",
        "RB",
        "View",
        "Menu",
        "LStick",
        "RStick",
    };

    private static string[] strAxis =
    {
        "LStick_x",
        "LStick_y",
        "Trigger",
        "RStick_x",
        "RStick_y",
        "DPad_x",
        "DPad_y"
    };

    public static bool GenerateController(int useController, Controller.Controller controller)
    {
        if (ms_InputGenerator == null)
        {
            ms_InputGenerator = new InputManagerGenerator();
        }
        if (useController > 0)
            return GeneratePlayerController(useController, controller);
        else if (useController == 0)
            return GenerateCommonController(controller);

        return false;
    }

    public static bool GeneratePlayerController(int userID, Controller.Controller controller)
    {
        if (userID < 1 || userID > 4)
        {
            Debug.LogError("生成可能コントローラーの範囲を超えています。");
            return false;
        }

        if (ms_bGaneratedController[userID - 1] && ms_RegisteredController[userID - 1])
        {
            Debug.LogWarning("すでに存在するコントローラです。");
            return false;
        }
        string str_Player = "Controller" + userID.ToString() + "_";
        for (int i = 0; i < (int)Controller.Button.Max; i++)
        {
            //A ~ RStick
            if (!ms_bGaneratedController[userID - 1])
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strButton[i], "joystick " + userID.ToString() + " button " + i.ToString(), null, null, null));
            controller.SetButton((Controller.Button)i, str_Player + strButton[i]);
        }

        for (int i = 0; i < (int)Controller.Axis.Max; i++)
        {
            if (!ms_bGaneratedController[userID - 1])
            {
                bool invert = false;
                if (i == (int)Controller.Axis.Cross_y || i == (int)Controller.Axis.L_y || i == (int)Controller.Axis.R_y)
                    invert = true;
                //LStick_x ~ DPad_y
                ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strAxis[i], userID, i + 1, invert));
            }
            controller.SetAxis((Controller.Axis)i, str_Player + strAxis[i]);
        }
        ms_bGaneratedController[userID - 1] = true;
        ms_RegisteredController[userID - 1] = true;

        Debug.Log("Create controller ID:" + userID.ToString());

        return true;
    }

    public static bool GenerateCommonController(Controller.Controller controller)
    {
        string str_Player = "CommonController_";
        for (int i = 0; i < (int)Controller.Button.Max; i++)
        {
            if (!ms_bInitedCommonController)
                //A ~ RStick
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strButton[i], "joystick button " + i.ToString(), null, null, null));
            controller.SetButton((Controller.Button)i, str_Player + strButton[i]);
        }
        for (int i = 0; i < (int)Controller.Axis.Max; i++)
        {
            if (!ms_bInitedCommonController)
            {
                bool invert = false;
                if (i == (int)Controller.Axis.Cross_y || i == (int)Controller.Axis.L_y || i == (int)Controller.Axis.R_y)
                    invert = true;
                //LStick_x ~ DPad_y
                ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strAxis[i], 0, i + 1, invert));
            }
            controller.SetAxis((Controller.Axis)i, str_Player + strAxis[i]);
        }
        ms_bInitedCommonController = true;
        Debug.Log("Create common controller");

        return true;
    }

    public static void ReleaseController(int userID)
    {
        ms_RegisteredController[userID - 1] = false;
    }

    public static void CleanInputManager()
    {
        ms_InputGenerator.ClearSetting();
    }

    public static SerializedProperty GetPropaty(string axisName, InputManagerGenerator.AxisPropaty axisPropaty)
    {
        return ms_InputGenerator.GetProperty(axisName, axisPropaty);
    }

=======
﻿using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ControllerGenerator : MonoBehaviour
{
    private static InputManagerGenerator ms_InputGenerator;
    private static bool ms_bInitedCommonController = false;
    private static bool[] ms_bInitedController = { false, false, false, false };

    //ボタン名
    private static string[] strButton =
    {
        "A",
        "B",
        "X",
        "Y",
        "LB",
        "RB",
        "View",
        "Menu",
        "LStick",
        "RStick",
    };

    private static string[] strAxis =
    {
        "LStick_x",
        "LStick_y",
        "Trigger",
        "RStick_x",
        "RStick_y",
        "DPad_x",
        "DPad_y"
    };

    public static bool GenerateController(int useController, Controller.Controller controller)
    {
        if (ms_InputGenerator == null)
        {
            ms_InputGenerator = new InputManagerGenerator();
        }

        if (useController > 0 && useController < 5 && !ms_bInitedController[useController - 1])
        {
            string str_Player = "Controller" + useController.ToString() + "_";
            for (int i = 0; i < (int)Controller.Button.Max; i++)
            {
                //A ~ RStick
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strButton[i], "joystick " + useController.ToString() + " button " + i.ToString(), null, null, null));
                controller.SetButton((Controller.Button)i, str_Player + strButton[i]);
            }

            for (int i = 0; i < (int)Controller.Axis.Max; i++)
            {
                bool invert = false;
                if (i == (int)Controller.Axis.Cross_y || i == (int)Controller.Axis.L_y || i == (int)Controller.Axis.R_y)
                    invert = true;
                //LStick_x ~ DPad_y
                ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strAxis[i], useController, i + 1, invert));
                controller.SetAxis((Controller.Axis)i, str_Player + strAxis[i]);
            }
            ms_bInitedController[useController - 1] = true;

            Debug.Log("Create controller ID:" + useController.ToString());

            return true;
        }
        else if (useController == 0)
        {
            string str_Player = "CommonController_";
            for (int i = 0; i < (int)Controller.Button.Max; i++)
            {
                if (!ms_bInitedCommonController)
                    //A ~ RStick
                    ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strButton[i], "joystick button " + i.ToString(), null, null, null));
                controller.SetButton((Controller.Button)i, str_Player + strButton[i]);
            }
            for (int i = 0; i < (int)Controller.Axis.Max; i++)
            {
                if (!ms_bInitedCommonController)
                {
                    bool invert = false;
                    if (i == (int)Controller.Axis.Cross_y || i == (int)Controller.Axis.L_y || i == (int)Controller.Axis.R_y)
                        invert = true;
                    //LStick_x ~ DPad_y
                    ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strAxis[i], 0, i + 1, invert));
                }
                controller.SetAxis((Controller.Axis)i, str_Player + strAxis[i]);
            }
            ms_bInitedCommonController = true;
            Debug.Log("Create common controller");

            return true;
        }
        else if (ms_bInitedController[useController - 1])
        {
            Debug.LogError("Already exist controller ID : " + useController.ToString());
            return false;
        }
        else
        {
            Debug.LogWarning("コントローラー生成失敗");
            return false;
        }
    }

    public static void CleanInputManager()
    {
        ms_InputGenerator.ClearSetting();
    }
#if UNITY_EDITOR
    public static SerializedProperty GetPropaty(string axisName, InputManagerGenerator.AxisPropaty axisPropaty)
    {
        return ms_InputGenerator.GetProperty(axisName, axisPropaty);
    }
#endif
>>>>>>> c6cff8dec8b254bf8d4567b15c5e16561a55877f
}