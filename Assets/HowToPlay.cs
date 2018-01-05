using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Controller;


public class HowToPlay : MonoBehaviour {

	private AudioList m_Audio;
    private Controller.Controller m_Controller;


    public Sprite[] Images = new Sprite[2];   //画像格納用配列　指定した数分確保


    private GameObject Image1;


	//Images.Length     //最大値

	private float timeCount = 0;

    private int count=0;    //カウント
    // Use this for initialization
    void Start () {
		m_Audio = GameObject.Find("SoundManager").GetComponent<AudioList>();
		m_Audio.Play((int)AudioList.SoundList_BGM.BGM_Tutorial0);
        m_Controller = GameObject.Find("TutorialManager").GetComponent<Controller.Controller>();

        count =0;      //初期化

        //Image1 = GameObject.FindObjectOfType<HowToPlay_Image1>().GetComponent<Image>().sprite;       //画像1確保
        Image1 = GameObject.Find("HowToPlay_Image1").gameObject;       //画像1確保

        Image1.GetComponent<Image>().sprite = Images[count];   //画像切り替え
    }

	// Update is called once per frame
	void Update() {

		// フェードを待つ
		if (timeCount > 0) {
			timeCount += Time.deltaTime;
			SetFade(timeCount / 3);
			if(timeCount > 3) {
				SceneChange.Instance._SceneLoadGame();
			}
		}

        //最後まで画像が進んだとき
        if (count + 1 == Images.Length)
        {
            if (m_Controller.GetButtonDown(Controller.Button.A) || m_Controller.GetButtonDown(Controller.Button.B))
            {
                //シーンの移動
                //SceneChange.Instance._SceneLoadGame();
				m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_UI_Title_Decision);
				timeCount += Time.deltaTime;
			}
		}

		//ボタンが押されたら次の画像に
		if (m_Controller.GetButtonDown(Controller.Button.A) || m_Controller.GetButtonDown(Controller.Button.B)){
			m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_UI_Menu_Decision);
			count++;
            Image1.GetComponent<Image>().sprite = Images[count];  //画像切り替え
        }

        
    }

	void SetFade(float value) {
		float v = value;
		if (v >= 1) {
			v = 1f;
		}
		GameObject.Find("Fade").GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, v);
	}

}
