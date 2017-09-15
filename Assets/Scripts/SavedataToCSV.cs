using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// CSVデータに書き込む用
/// </summary>
public class SavedataToCSV : MonoBehaviour
{

    StreamWriter    m_sw;             //文字列書き込み用
    FileInfo        m_fi;             //ファイルの場所
    string          m_filePath;       //ファイルパス
    string          m_saveTxt = "";   //保存する文字列

    /// <summary>
    /// 文字列型のデータをCSVに入れる
    /// </summary>
    /// <param name="fileName">パス指定と.csvは除く</param>
    /// <param name="txt">CSVに書き込みたい文字列</param>
    public void _Save(string fileName, List<string> txt)
    {
        OpenFile(fileName);

        for (int i = 0; i < txt.Count; i++)
        {
            if (i != 0) m_saveTxt += ',';
            m_saveTxt += txt[i];
        }

        CloseFile();
    }

    /// <summary>
    /// 文字列型のデータをCSVに入れる
    /// </summary>
    /// <param name="fileName">パス指定と.csvは除く</param>
    /// <param name="txt">CSVに書き込みたい文字列</param>
    public void _Save(string fileName, string[] txt)
    {
        OpenFile(fileName);

        for (int i = 0; i < txt.Length; i++)
        {
            if (i != 0) m_saveTxt += ',';
            m_saveTxt += txt[i];
        }

        CloseFile();
    }

    /// <summary>
    /// 整数型のデータをCSVに入れる
    /// </summary>
    /// <param name="fileName">パス指定と.csvは除く</param>
    /// <param name="num">CSVに書き込みたい数字</param>
    public void _Save(string fileName, List<int> num)
    {
        OpenFile(fileName);

        for (int i = 0; i < num.Count; i++)
        {
            if (i != 0) m_saveTxt += ',';
            m_saveTxt += num[i].ToString();
        }

        CloseFile();
    }

    /// <summary>
    /// 整数型の１次元配列をCSVに入れる
    /// </summary>
    /// <param name="fileName">パス指定と.csvは除く</param>
    /// <param name="num">CSVに書き込みたい数字</param>
    public void _Save(string fileName, int[] num)
    {
        OpenFile(fileName);

        for (int i = 0; i < num.Length; i++)
        {
            if (i != 0) m_saveTxt += ',';
            m_saveTxt += num[i].ToString();
        }

        CloseFile();
    }

    /// <summary>
    /// 整数型の２次元配列をCSVに入れる
    /// </summary>
    /// <param name="fileName">パス指定と.csvは除く</param>
    /// <param name="num">CSVに書き込みたい数字</param>
    public void _Save(string fileName, int[,] num)
    {
        OpenFile(fileName);

        //数値を文字列に変換する
        for (int h = 0; h < num.GetLength(0); h++)//高さ
        {
            for (int w = 0; w < num.GetLength(1); w++)//幅
            {
                if (w != 0) m_saveTxt += ',';
                m_saveTxt += num[h, w].ToString();
            }
            if (h < num.GetLength(0) - 1) m_saveTxt += "\r\n";
        }

        CloseFile();
    }

    /// <summary>
    /// ファイルを開く
    /// </summary>
    /// <param name="fileName">パス指定と.csvは除く</param>
    private void OpenFile(string fileName)
    {
        m_filePath = Application.dataPath + "/Resources/" + fileName + ".csv";//ファイルパス
        m_fi = new FileInfo(m_filePath);

        //上書きすることを知らせる
        m_sw = m_fi.CreateText();

        //テキストの初期化
        m_saveTxt = "";
    }

    /// <summary>
    /// 上書きしてファイルを閉じる
    /// </summary>
    private void CloseFile()
    {
        //上書き
        m_sw.Write(m_saveTxt);

        //上書きを確定
        m_sw.Flush();
        m_sw.Close();
    }
}
