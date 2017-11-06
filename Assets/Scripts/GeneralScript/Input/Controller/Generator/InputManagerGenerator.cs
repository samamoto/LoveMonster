using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// InputManagerを設定するためのクラス
/// </summary>
public class InputManagerGenerator
{
    private SerializedObject serializedObject;
    private SerializedProperty axesProperty;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public InputManagerGenerator()
    {
        // InputManager.assetをシリアライズされたオブジェクトとして読み込む
        serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        axesProperty = serializedObject.FindProperty("m_Axes");
    }

    public enum AxisPropaty
    {
        Name, DescriptiveName, DescriptiveNegativeName, NegativeButton, PositiveButton, AltNegativeButton, AltPositiveButton, Gravity, Dead, Sensitivity, Snap, Invert, Type, Axis, JoyNum
    }

    private string[] strAxisPropaty =
    {
        "m_Name","descriptiveName","descriptiveNegativeName","negativeButton","positiveButton","altNegativeButton","altPositiveButton","gravity","sensitivity","snap","invert","type","axis","joyNum"
    };

    /// <summary>
    /// 軸を追加します。
    /// </summary>
    /// <param name="serializedObject">Serialized object.</param>
    /// <param name="axis">Axis.</param>
    public void AddAxis(InputAxis axis)
    {
        if (axis.axis < 1)
            Debug.LogError("Axisは1以上に設定してください。");
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

        axesProperty.arraySize++;   //配列追加
        serializedObject.ApplyModifiedProperties();

        SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);  //配列の最後を取得

        GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
        GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
        GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
        GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
        GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
        GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
        GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
        GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
        GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
        GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
        GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
        GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
        GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
        GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
        GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// 子要素のプロパティを取得します。
    /// </summary>
    /// <returns>The child property.</returns>
    /// <param name="parent">Parent.</param>
    /// <param name="name">Name.</param>
    private SerializedProperty GetChildProperty(SerializedProperty parent, string name)
    {
        SerializedProperty child = parent.Copy();
        child.Next(true);
        do
        {
            if (child.name == name) return child;
        }
        while (child.Next(false));
        Debug.LogWarning("Not find property");
        return null;
    }

    public SerializedProperty GetProperty(string axisName, AxisPropaty getPropaty)
    {
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
        return GetChildProperty(GetChildProperty(axesProperty, axisName), strAxisPropaty[(int)getPropaty]);
    }

    /// <summary>
    /// 設定を全てクリアします。
    /// </summary>
    public void ClearSetting()
    {
        axesProperty.ClearArray();
        serializedObject.ApplyModifiedProperties();
    }
}