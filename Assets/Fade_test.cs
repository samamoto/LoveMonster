using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Fadeのテスト用スクリプト
/// 
/// 
/// </summary>


public class Fade_test : MonoBehaviour
{
    //スクリプト群
    //private SceneChange m_ScreenChange;

    private InputManagerGenerator inputmanage;

    private bool MovieEndFlag = false;  //ムービー再生終了フラグ
    private Animator animator;

    //フェード用----------------------------------------------------------------
    [SerializeField, Header("フェードスクリプト")]
    private Fade fade = null;   //フェード

    private bool FadeEnded = false; //フェードの終了を確認するためのフラグ

    //--------------------------------------------------------------------------


    // Use this for initialization
    private void Start()
    {
        fade = GameObject.Find("FadeCanvas").GetComponent<Fade>();

        fade.ForceStart = true; //無理やり黒からフェードさせる
        //fade.FadeOut(2.0f, () => { FadeEnded = true; });    //フェードアウトする&終わったらフラグを立てるように設定
        fade.FadeOut(2.0f);
    }

     
    // Update is called once per frame
    private void Update()
    {


        // m_ScreenChange._DebugInput();
        if (Input.GetKeyUp(KeyCode.Space))//スペースキーで
        {
            //移動したいシーンへ移動(チュートリアルシーン)
            //SceneManager.LoadScene("TutorialScene");

            fade.FadeIn(2.0f, () => { FadeEnded = true; });


        }


        if (FadeEnded == true)  //フェード終了後
        {
            SceneChange.Instance._SceneLoadTitle();

        }
    }

    

}
