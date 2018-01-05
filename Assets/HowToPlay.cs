using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Controller;


public class HowToPlay : MonoBehaviour {


    private Controller.Controller m_Controller;


    public Sprite[] Images = new Sprite[2];   //画像格納用配列　指定した数分確保


    private GameObject Image1;


    //Images.Length     //最大値


    private int count=0;    //カウント
    // Use this for initialization
    void Start () {

        m_Controller = GameObject.Find("TutorialManager").GetComponent<Controller.Controller>();

        count =0;      //初期化

        //Image1 = GameObject.FindObjectOfType<HowToPlay_Image1>().GetComponent<Image>().sprite;       //画像1確保
        Image1 = GameObject.Find("HowToPlay_Image1").gameObject;       //画像1確保

        Image1.GetComponent<Image>().sprite = Images[count];   //画像切り替え
    }
	
	// Update is called once per frame
	void Update () {



        //最後まで画像が進んだとき
        if (count + 1 == Images.Length)
        {
            if (m_Controller.GetButtonDown(Controller.Button.A))
            {
                //シーンの移動
                SceneChange.Instance._SceneLoadGame();
            }
        }

        //ボタンが押されたら次の画像に
        if (m_Controller.GetButtonDown(Controller.Button.A))
        {
            count++;
            Image1.GetComponent<Image>().sprite = Images[count];  //画像切り替え
        }

        
    }
}
