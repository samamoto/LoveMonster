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
	public enum ResultPhase {
		Start,
		Move,
		LookDown,
		Stop,
		Result,
		Info,
		None,
	}

	public Transform[] CameraLocation = new Transform[2];

	public ResultPhase m_Phase = ResultPhase.Start;
	Camera m_cam;
	AudioList m_Audio;

    // Use this for initialization
    private void Start()
    {
		//m_ScreenChange = GameObject.Find("SceneChange").GetComponent<SceneChange>();
		//フェード       仕様書によって場所変更アリ(現在は一番最初)
		//読み込みは一度でおｋ？ 現在はタイトルに設置
		//追加シーン     ここでフェードを読み込むのもアリ
		//SceneManager.LoadScene("TitleScene", LoadSceneMode.Additive);
		m_cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		m_cam.transform.position = Vector3.zero;
		m_Audio = GameObject.Find("SoundManager").GetComponent<AudioList>();
    }

    // Update is called once per frame
    private void Update()
    {
		//フェードが終了したフラグを受け取ってから判定開始
		//ここにif文を追加

		switch (m_Phase) {
		case ResultPhase.Start:
			m_Audio.Play((int)AudioList.SoundList_BGM.BGM_Game_Stage3);
			m_Phase++;
			break;
		case ResultPhase.Move:
			iTween.MoveTo(m_cam.gameObject, iTween.Hash(
				"position", CameraLocation[0].position,
				"time", 5f,
				"easetype", "easeInCubic"
				)
			);
			//iTween.MoveTo(m_cam.gameObject, CameraLocation[0].position, 5f);
			if (m_cam.transform.position == CameraLocation[0].position) {
				m_Phase++;
			}
			break;
		case ResultPhase.LookDown:
			iTween.MoveTo(m_cam.gameObject, iTween.Hash(
				"position", CameraLocation[1].position,
				"time", 2.5f,
				"easetype", "easeOutCirc"
				)
			);
			//iTween.MoveTo(m_cam.gameObject, CameraLocation[1].position, 2.5f);
			iTween.RotateTo(m_cam.gameObject, CameraLocation[1].rotation.eulerAngles, 2.5f);
			if (m_cam.transform.position == CameraLocation[1].position) {
				m_Phase++;
			}
			break;
		case ResultPhase.Stop:
			m_Phase++;
			break;
		case ResultPhase.Result:
			// アニメーションさせたり
			// 結果表示したり
			// エフェクト再生したり
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
