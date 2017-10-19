using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGenerator : MonoBehaviour
{
    private static InputManagerGenerator ms_InputGenerator;
    private static bool ms_bInitedCommonController = false;
    private static bool[] ms_bInitedController = { false, false, false, false };

    //ボタン名列挙
    public enum enumControllerName
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
        TRIGGER,
        RSTICK_X,
        RSTICK_Y,
        DPAD_X,
        DPAD_Y,
        MAX
    }

    //ボタン名
    public static string[] strControllerName =

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
        "LStick_x",
        "LStick_y",
        "Trigger",
        "RStick_x",
        "RStick_y",
        "DPad_x",
        "DPad_y"
    };

    public static bool GenerateController(int useController)
    {
        if (ms_InputGenerator == null)
        {
            ms_InputGenerator = new InputManagerGenerator();
        }

        if (useController > 0 && !ms_bInitedController[useController - 1])
        {
            string str_Player = "Controller" + useController.ToString() + "_";
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[0], "joystick " + useController.ToString() + " button 0", null));         //A
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[1], "joystick " + useController.ToString() + " button 1", null));         //B
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[2], "joystick " + useController.ToString() + " button 2", null));         //X
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[3], "joystick " + useController.ToString() + " button 3", null));         //Y
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[4], "joystick " + useController.ToString() + " button 4", null));        //LB
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[5], "joystick " + useController.ToString() + " button 5", null));        //RB
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[6], "joystick " + useController.ToString() + " button 6", null));      //View
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[7], "joystick " + useController.ToString() + " button 7", null));      //Menu
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[8], "joystick " + useController.ToString() + " button 8", null));    //LStickButton
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[9], "joystick " + useController.ToString() + " button 9", null));    //RstickButton
            ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strControllerName[10], useController, 1));                            //LStick X
            ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strControllerName[11], useController, 2));                            //LStick Y
            ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strControllerName[12], useController, 3));                             //LT&RT
            ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strControllerName[13], useController, 4));                            //RStick X
            ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strControllerName[14], useController, 5));                            //RStick Y
            ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strControllerName[15], useController, 6));                              //DPad X
            ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strControllerName[16], useController, 7));                              //DPad Y

            ms_bInitedController[useController - 1] = true;
            return true;
        }
        else if (useController == 0)
        {
            if (!ms_bInitedCommonController)
            {
                string str_Player = "Controller" + useController.ToString() + "_";
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[0], "joystick button 0", null));         //A
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[1], "joystick button 1", null));         //B
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[2], "joystick button 2", null));         //X
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[3], "joystick button 3", null));         //Y
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[4], "joystick button 4", null));        //LB
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[5], "joystick button 5", null));        //RB
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[6], "joystick button 6", null));      //View
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[7], "joystick button 7", null));      //Menu
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[8], "joystick button 8", null));    //LStickButton
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + strControllerName[9], "joystick button 9", null));    //RstickButton
                ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strControllerName[10], 0, 1));                            //LStick X
                ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strControllerName[11], 0, 2));                            //LStick Y
                ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strControllerName[12], 0, 3));                             //LT&RT
                ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strControllerName[13], 0, 4));                            //RStick X
                ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strControllerName[14], 0, 5));                            //RStick Y
                ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strControllerName[15], 0, 6));                              //DPad X
                ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + strControllerName[16], 0, 7));                              //DPad Y
                ms_bInitedCommonController = true;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void CleanInputManager()
    {
        ms_InputGenerator.ClearSetting();
    }
}