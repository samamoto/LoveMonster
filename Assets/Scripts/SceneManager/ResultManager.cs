using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 結果画面を表示
/// 
/// 
/// 恐らくClearとGameoverの2つを管理
/// Sceneは1つ?
/// 
/// </summary>


public class ResultManager : MonoBehaviour {
    //スクリプト群
	enum ResultPhase {
		Start,
		LookDown,
		Stop,
		Result,
		Info,
		None,
	}

	ResultPhase m_Phase = ResultPhase.Start;

    // Use this for initialization
    private void Start()
    {
        //m_ScreenChange = GameObject.Find("SceneChange").GetComponent<SceneChange>();
        //フェード       仕様書によって場所変更アリ(現在は一番最初)
        //読み込みは一度でおｋ？ 現在はタイトルに設置
        //追加シーン     ここでフェードを読み込むのもアリ
        //SceneManager.LoadScene("TitleScene", LoadSceneMode.Additive);


    }

    // Update is called once per frame
    private void Update()
    {
		//フェードが終了したフラグを受け取ってから判定開始
		//ここにif文を追加

		switch (m_Phase) {
		case ResultPhase.Start:
			
			break;
		case ResultPhase.LookDown:

			break;
		case ResultPhase.Stop:
			m_Phase++;
			break;
		case ResultPhase.Result:
			m_Phase++;
			break;
		case ResultPhase.Info:
			SceneToNext();//次のシーンへ移動(タイトルへ)   
			break;

		default:
			break;
		}



    }


    public void SceneToNext()
    {
        //m_ScreenChange._DebugInput();
        if (Input.GetKeyUp(KeyCode.Space))//スペースキーで
        {
			//移動したいシーンへ移動(タイトルシーン)
			//SceneManager.LoadScene("TitleScene_");
			SceneChange.Instance._SceneLoadTitle();
        }

        //ここにフェード終了処理設置？

    }
}
