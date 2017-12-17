using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGenerator : MonoBehaviour
{
    private static InputManagerGenerator ms_InputGenerator;
    private static bool ms_bInitedCommonController = false;
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

        if (ms_RegisteredController[userID - 1])
        {
            Debug.LogWarning("すでに存在するコントローラです。");
            return false;
        }
        string str_Player = "Controller" + userID.ToString() + "_";
        for (int i = 0; i < (int)Controller.Button.Max; i++)
        {
            controller.SetButton((Controller.Button)i, str_Player + strButton[i]);
        }

        for (int i = 0; i < (int)Controller.Axis.Max; i++)
        {
            controller.SetAxis((Controller.Axis)i, str_Player + strAxis[i]);
        }
        ms_RegisteredController[userID - 1] = true;

        Debug.Log("Create controller ID:" + userID.ToString());

        return true;
    }

    public static bool GenerateCommonController(Controller.Controller controller)
    {
        string str_Player = "CommonController_";
        for (int i = 0; i < (int)Controller.Button.Max; i++)
        {
            controller.SetButton((Controller.Button)i, str_Player + strButton[i]);
        }
        for (int i = 0; i < (int)Controller.Axis.Max; i++)
        {
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
}