using UnityEngine;
using UnityEditor;


/// <summary>
/// それぞれのUIを管理するScript
/// PlayerのUI用のScriptは表示のみ(中身の構成)を
/// 行い、値の変動はこちらで管理する。
/// </summary>


public class UIManager : ScriptableObject
{
    [MenuItem("Tools/MyTool/Do It in C#")]
    static void DoIt()
    {
        EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
    }
}