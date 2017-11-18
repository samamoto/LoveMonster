using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour {
    private int minite;
    private float second;
    private int oldSecond;
    private bool timerFlag= true;
    private char textField;

    float countTime = 0;


    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        countTime += Time.deltaTime; //スタートしてからの秒数を格納
        GetComponent<Text>().text = countTime.ToString("F2"); //小数2桁にして表示
    }
}
