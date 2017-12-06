using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Scene開始後、ムービーを再生(再生中は操作不可?、スキップはアリ?)、
/// 現在はスペースキーでSceneの移動を行う。
/// 
/// 
/// [ToDo]
/// Fadeの追加、現在スクリプトのみ
/// チームロゴ(HyperVault)
/// 
/// 
/// </summary>


public class TitleManager : MonoBehaviour {
    //スクリプト群
    //private SceneChange m_ScreenChange;

    private InputManagerGenerator inputmanage;

    private bool MovieEndFlag=false;  //ムービー再生終了フラグ
    


    // Use this for initialization
    private void Start()
    {
        //m_ScreenChange = GameObject.Find("SceneChange").GetComponent<SceneChange>();

        //フェード       仕様書によって場所変更アリ(現在は一番最初)
        //追加シーン     ここでフェードを読み込むのもアリ
        //SceneManager.LoadScene("TitleScene", LoadSceneMode.Additive);


        //ここにムービー再生処理を追加?　→MovieTextureにフラグを追加するべき?
        MovieEndFlag = false;       //再生フラグ初期化
    }

    // Update is called once per frame
    private void Update()
    {
        //if (MovieEndFlag == true)   //動画の再生が終了していたら
        //{
            SceneToNext();  //次のシーンへ移動(チュートリアルへ)
        //}
    }

    public void SceneToNext()     //次のシーンへ移動
    {
        //m_ScreenChange._DebugInput();
        if (Input.GetKeyUp(KeyCode.Space))//スペースキーで
        {
            //移動したいシーンへ移動(チュートリアルシーン)
            SceneManager.LoadScene("TutorialScene");
            
               
        }


    }

}
