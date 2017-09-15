using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMain : MonoBehaviour {

    //スクリプト群
    SceneChange m_ScreenChange;

    // Use this for initialization
    void Start () {
        m_ScreenChange = GameObject.Find("SceneChange").GetComponent<SceneChange>();
    }
    private bool flag=false;
    private bool flag2 = false;
    // Update is called once per frame
    void Update()
    {
        //書き込み
        if (!flag)
        {
            SavedataToCSV SaveCSV = GameObject.Find("Save").GetComponent<SavedataToCSV>();
            flag = true;

            //テスト１：文字列の配列を書き込む
            string[] test1 = new string[] { "a", "b", "c", "d", "e" };
            SaveCSV._Save("test1", test1);

            //テスト２：文字列のリストを書き込む
            List<string> test2 = new List<string>();
            for (int i = 0; i < test1.Length; i++)
            {
                test2.Add(test1[i] + i);
            }
            SaveCSV._Save("test2", test2);

            //テスト３：数値の配列を書き込む
            int[] test3 = new int[] { 0, 1, 2, 3, 4 };
            SaveCSV._Save("test3", test3);

            //テスト４：数値のリストを書き込む
            List<int> test4 = new List<int>();
            for (int i = 0; i < test3.Length; i++)
                test4.Add(test3[i] + 5);
            SaveCSV._Save("test4", test4);

            //テスト５：数値の２次元配列を書き込む
            int[,] test5 = new int[3,5] { { 0, 1, 2, 3, 4 }, { 5, 6, 7, 8, 9 }, { 10, 11, 12, 13, 14 } };
            SaveCSV._Save("test5", test5);

            Debug.Log("Save");
        }
        else if (flag && !flag2)//読込
        {
            flag2 = true;

            //テスト１：文字列の配列を読み込む
            string[] test1;
            test1 = GameObject.Find("Load").GetComponent<LoaddataToCSV>()._LoadStrIntoArray("test1");
            for (int i = 0; i < test1.Length; i++)
                Debug.Log("test1 : " + test1[i]);

            //テスト２：文字列のリストを読み込む
            List<string> test2;
            test2 = GameObject.Find("Load").GetComponent<LoaddataToCSV>()._LoadStrIntoList("test2");
            for (int i = 0; i < test2.Count; i++)
                Debug.Log("test2 : " + test2[i]);

            //テスト３：数値の配列を読み込む
            int[] test3;
            test3 = GameObject.Find("Load").GetComponent<LoaddataToCSV>()._LoadIntIntoArray("test3");
            for (int i = 0; i < test3.Length; i++)
                Debug.Log("test3 : " + test3[i]);

            //テスト４：数値のリストを読み込む
            List<int> test4;
            test4 = GameObject.Find("Load").GetComponent<LoaddataToCSV>()._LoadIntIntoList("test4");
            for (int i = 0; i < test4.Count; i++)
                Debug.Log("test4 : " + test4[i]);

            //テスト５：数値を２次元配列に読み込む
            int[,] test5;
            test5 = GameObject.Find("Load").GetComponent<LoaddataToCSV>()._LoadIntIntoTwoArray("test5");
            for (int h = 0; h < test5.GetLength(0); h++)
            {
                for (int w = 0; w < test5.GetLength(1); w++)
                {
                    Debug.Log("test5 : " + test5[h, w]);
                }
                Debug.Log("test5 : " + h + "行目　改行");
            }
        }
        m_ScreenChange._DebugInput();
    }
}
