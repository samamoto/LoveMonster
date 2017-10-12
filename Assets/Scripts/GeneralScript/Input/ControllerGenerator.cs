using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGenerator : MonoBehaviour
{
    private static int ms_Controller = 1;
    private static InputManagerGenerator ms_InputGenerator;
    private static bool msb_MakeCommonController = false;

    public GameObject m_DebugScreen;
    public bool mb_ViewDebugScreen = false;

    public static void GenerateController(Controller controller)
    {
        if (ms_InputGenerator == null)
            ms_InputGenerator = new InputManagerGenerator();

        if (controller.SetID(ms_Controller))
        {
            string str_Player = "Player" + ms_Controller.ToString() + "_";
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "A", ms_Controller, "joystick button 0", null));         //A
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "B", ms_Controller, "joystick button 1", null));         //B
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "X", ms_Controller, "joystick button 2", null));         //X
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "Y", ms_Controller, "joystick button 3", null));         //Y
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "LB", ms_Controller, "joystick button 4", null));        //LB
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "RB", ms_Controller, "joystick button 5", null));        //RB
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "View", ms_Controller, "joystick button 6", null));      //View
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "Menu", ms_Controller, "joystick button 7", null));      //Menu
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "LStick", ms_Controller, "joystick button 8", null));    //LStickButton
            ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "RStick", ms_Controller, "joystick button 9", null));    //RstickButton
            ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + "LStick_X", ms_Controller, 1));                            //LStick X
            ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + "LStick_Y", ms_Controller, 2));                            //LStick Y
            ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + "Trigger", ms_Controller, 3));                             //LT&RT
            ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + "RStick_X", ms_Controller, 4));                            //RStick X
            ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + "RStick_Y", ms_Controller, 5));                            //RStick Y
            ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + "DPad_X", ms_Controller, 6));                              //DPad X
            ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + "DPad_Y", ms_Controller, 7));                              //DPad Y
            //ID送り
            ms_Controller++;
        }
    }

    public static void GenerateCommonController(Controller controller)
    {
        if (ms_InputGenerator == null)
            ms_InputGenerator = new InputManagerGenerator();

        if (controller.SetID(0))
        {
            if (!msb_MakeCommonController)
            {
                string str_Player = "Player" + ms_Controller.ToString() + "_";
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "A", 0, "joystick button 0", null));         //A
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "B", 0, "joystick button 1", null));         //B
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "X", 0, "joystick button 2", null));         //X
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "Y", 0, "joystick button 3", null));         //Y
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "LB", 0, "joystick button 4", null));        //LB
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "RB", 0, "joystick button 5", null));        //RB
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "View", 0, "joystick button 6", null));      //View
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "Menu", 0, "joystick button 7", null));      //Menu
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "LStick", 0, "joystick button 8", null));    //LStickButton
                ms_InputGenerator.AddAxis(InputAxis.CreatePadButton(str_Player + "RStick", 0, "joystick button 9", null));    //RstickButton
                ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + "LStick_X", 0, 1));                            //LStick X
                ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + "LStick_Y", 0, 2));                            //LStick Y
                ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + "Trigger", 0, 3));                             //LT&RT
                ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + "RStick_X", 0, 4));                            //RStick X
                ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + "RStick_Y", 0, 5));                            //RStick Y
                ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + "DPad_X", 0, 6));                              //DPad X
                ms_InputGenerator.AddAxis(InputAxis.CreatePadAxis(str_Player + "DPad_Y", 0, 7));                              //DPad Y
            }
        }
    }

    public static void CleanInputManager()
    {
        ms_InputGenerator.ClearSetting();
    }
}