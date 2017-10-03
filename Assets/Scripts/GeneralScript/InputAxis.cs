using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 軸のタイプ
/// </summary>
public enum AxisType
{
    KeyOrMouseButton = 0,
    MouseMovement = 1,
    JoystickAxis = 2
};

/// <summary>
/// 入力の情報
/// </summary>
public class InputAxis
{
    public string name = "";
    public string descriptiveName = "";
    public string descriptiveNegativeName = "";
    public string negativeButton = "";
    public string positiveButton = "";
    public string altNegativeButton = "";
    public string altPositiveButton = "";

    public float gravity = 0;
    public float dead = 0;
    public float sensitivity = 0;

    public bool snap = false;
    public bool invert = false;

    public AxisType type = AxisType.KeyOrMouseButton;

    // 1から始まる。
    public int axis = 1;

    // 0なら全てのゲームパッドから取得される。1以降なら対応したゲームパッド。
    public int joyNum = 0;

    /// <summary>
    /// 押すと1になるキーの設定データを作成する
    /// </summary>
    /// <returns>The button.</returns>
    /// <param name="name">Name.</param>
    /// <param name="positiveButton">Positive button.</param>
    /// <param name="altPositiveButton">Alternate positive button.</param>
    public static InputAxis CreateButton(string name, string positiveButton, string altPositiveButton)
    {
        var axis = new InputAxis();
        axis.name = name;
        axis.positiveButton = positiveButton;
        axis.altPositiveButton = altPositiveButton;
        axis.gravity = 1000;
        axis.dead = 0.001f;
        axis.sensitivity = 1000;
        axis.type = AxisType.KeyOrMouseButton;

        return axis;
    }

    /// <summary>
    /// ゲームパッド用の軸の設定データを作成する
    /// </summary>
    /// <returns>The joy axis.</returns>
    /// <param name="name">Name.</param>
    /// <param name="joystickNum">Joystick number.</param>
    /// <param name="axisNum">Axis number.</param>
    public static InputAxis CreatePadAxis(string name, int joystickNum, int axisNum)
    {
        var axis = new InputAxis();
        axis.name = name;
        axis.dead = 0.2f;
        axis.sensitivity = 1;
        axis.type = AxisType.JoystickAxis;
        axis.axis = axisNum;
        axis.joyNum = joystickNum;

        return axis;
    }

    ///<summary>
    ///ゲームパッド用のボタンの設定データを作成する
    ///</summary>
    ///<returns>The joy Buttons</returns>
    ///<param name="name">Name.</param>
    /// <param name="joystickNum">Joystick number.</param>
    /// <param name="positiveButton">Positive button.</param>
    /// <param name="negativeButton">Negative button.</param>
    public static InputAxis CreatePadButton(string name, int joystickNum, string positiveButton, string negativeButton)
    {
        var axis = new InputAxis();
        axis.name = name;
        axis.joyNum = joystickNum;
        axis.positiveButton = positiveButton;
        axis.negativeButton = negativeButton;
        axis.gravity = 1000;
        axis.dead = 0.001f;
        axis.sensitivity = 1000;
        return axis;
    }

    /// <summary>
    /// キーボード用の軸の設定データを作成する
    /// </summary>
    /// <returns>The key axis.</returns>
    /// <param name="name">Name.</param>
    /// <param name="negativeButton">Negative button.</param>
    /// <param name="positiveButton">Positive button.</param>
    /// <param name="axisNum">Axis number.</param>
    public static InputAxis CreateKeyAxis(string name, string negativeButton, string positiveButton, string altNegativeButton, string altPositiveButton)
    {
        var axis = new InputAxis();
        axis.name = name;
        axis.negativeButton = negativeButton;
        axis.positiveButton = positiveButton;
        axis.altNegativeButton = altNegativeButton;
        axis.altPositiveButton = altPositiveButton;
        axis.gravity = 3;
        axis.sensitivity = 3;
        axis.dead = 0.001f;
        axis.type = AxisType.KeyOrMouseButton;

        return axis;
    }
}