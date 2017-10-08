using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// CSVデータの読み込み用
/// </summary>
public class LoaddataToCSV : MonoBehaviour {
    string[]        csvDataStrArray;                        //リターン用文字列配列
    List<string>    csvDataStrList = new List<string>();    //リターン用文字列リスト
    int[]           csvDataIntArray;                        //リターン用整数配列
    List<int>       csvDataIntList = new List<int>();       //リターン用整数リスト
    int[,]          csvDataIntTwoArray;                     //リターン用整数２次元配列
    StreamReader    sr;                                     //文字列取得用
    FileInfo        fi;                                     //ファイルの場所

    /// <summary>
    /// csvファイルのロード　文字列型のリストに入れる
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns>文字列のリストに変換されたcsvのデータ</returns>
    public List<string> _LoadStrIntoList(string fileName)
    {
        OpenFile(fileName);

        //リストに読み込んだデータを入れていく
        while(sr.Peek()>-1)
        {
            string[] workStrLine = ReadLine();
            for (int i=0;i< workStrLine.Length;i++)
                csvDataStrList.Add(workStrLine[i]);
        }

        return csvDataStrList;
    }


    /// <summary>
    /// csvファイルのロード　文字列型のリストに入れる
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns>文字列の配列に変換されたcsvのデータ</returns>
    public string[] _LoadStrIntoArray(string fileName)
    {
        int countOfListElement = 0;//要素数カウンタ

        OpenFile(fileName);

        //データがいくつあるか数える
        while (sr.Peek()>-1)
        {
            countOfListElement += sr.ReadLine().Split(',').Length;
        }

        //配列の要素数確定
        csvDataStrArray = new string[countOfListElement];

        //初期位置に戻す
        sr.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

        //配列に入れていく
        csvDataStrArray = sr.ReadLine().Split(',');

        return csvDataStrArray;
    }


    /// <summary>
    /// csvファイルのロード　int型のリストに入れる
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns>int型のリストに変換されたcsvのデータ</returns>
    public List<int> _LoadIntIntoList(string fileName)
    {
        OpenFile(fileName);

        //リストに読み込んだデータを入れていく
        while (sr.Peek() > -1)
        {
            string[] csvParts = ReadLine();

            //文字列として取得したデータを数値に変換する
            foreach (string s in csvParts)
            {
                int value = int.Parse(s);
                csvDataIntList.Add(value);
            }
        }

        return csvDataIntList;
    }


    /// <summary>
    /// csvファイルのロード　int型の配列に入れる
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns>int型の配列に変換されたcsvのデータ</returns>
    public int[] _LoadIntIntoArray(string fileName)
    {
        OpenFile(fileName);

        //データがいくつあるか数える
        int countOfArrayElement = 0;//要素数カウンタ
        while(sr.Peek()>-1)
        {
            string[] workLength = ReadLine();
            countOfArrayElement += workLength.Length;
        }

        //配列の要素数確定
        csvDataIntArray = new int[countOfArrayElement];

        //初期位置に戻す
        sr.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

        //配列に入れていく
        int n = 0;
        while (sr.Peek() > -1)
        {
            string line = sr.ReadLine();
            string[] csvParts = line.Split(',');

            //文字列として取得したデータを数値に変換する
            foreach (string s in csvParts)
            {
                int value = int.Parse(s);
                csvDataIntArray[n] = value;
                n++;
            }
        }
        return csvDataIntArray;
    }

    /// <summary>
    /// csvファイルのロード
    /// int型の２次元配列に入れる
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns>int型の２次元配列に変換されたcsvのデータ</returns>
    public int[,] _LoadIntIntoTwoArray(string fileName)
    {
        OpenFile(fileName);

        //データがいくつあるか数える
        int countOfArrayElementHeight = 0;
        int countOfArrayElementWidth = 0;

        // 幅、高さを数える
        {
            //幅を数える
            string line = sr.ReadLine();
            string[] workLength = line.Split(',');
            countOfArrayElementWidth = workLength.Length;

            //初期位置に戻す
            sr.BaseStream.Seek(0, SeekOrigin.Begin);

            //高さを数える
            line = sr.ReadToEnd();
            workLength = line.Split('\n');
            countOfArrayElementHeight = workLength.Length - 1;

            //初期位置に戻す
            sr.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
        }

        //配列の要素数確定
        csvDataIntTwoArray = new int[countOfArrayElementHeight, countOfArrayElementWidth];

        //配列に入れていく
        int h = 0, w = 0;
        while(sr.Peek()>-1)
        {
            w = 0;
            string line = sr.ReadLine();
            string[] csvParts = line.Split(',');

            foreach(string s in csvParts)
            {
                int value = int.Parse(s);
                csvDataIntTwoArray[h, w] = value;
                w++;
            }
            h++;
        }

        return csvDataIntTwoArray;
    }

    /// <summary>
    /// ファイルを開く
    /// </summary>
    /// <param name="fileName"></param>
    private void OpenFile(string fileName)
    {
        //ファイル読み込み
        fi = new FileInfo(Application.dataPath + "/Resources/" + fileName + ".csv");
        sr = fi.OpenText();
    }

    /// <summary>
    /// カンマ区切りで１行読み込んで配列に入れる
    /// </summary>
    /// <returns></returns>
    private string[] ReadLine()
    {
        return sr.ReadLine().Split(',');
    }
}
