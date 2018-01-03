using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Controller;
using TMPro;

/// <summary>
/// 結果画面を表示
/// 恐らくClearとGameoverの2つを管理
/// Sceneは1つ?
/// </summary>


public class ResultManager : MonoBehaviour {
    //スクリプト群
	public enum ResultPhase {
		Start,
		Move,
		LookDown,
		Stop,
		Result,
		ResultIndication,
		Info,
		NextScene,
	}

	public Transform[] CameraLocation = new Transform[2];

	public ResultPhase m_Phase = ResultPhase.Start;
	Camera m_cam;
	AudioList m_Audio;
	Animator[] m_Animator = new Animator[4];
	Controller.Controller m_Controller;
	Image[] m_imageRank = new Image[4];
	Image m_DialogBack;
	Image m_DialogCursor;
	TextMeshProUGUI m_ResultLogo;
	private float countTimer = 0f;

	// Use this for initialization
	private void Start() {
		//m_ScreenChange = GameObject.Find("SceneChange").GetComponent<SceneChange>();
		//フェード       仕様書によって場所変更アリ(現在は一番最初)
		//読み込みは一度でおｋ？ 現在はタイトルに設置
		//追加シーン     ここでフェードを読み込むのもアリ
		//SceneManager.LoadScene("TitleScene", LoadSceneMode.Additive);
		m_cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		m_cam.transform.position = Vector3.zero;
		m_Audio = GameObject.Find("SoundManager").GetComponent<AudioList>();
		for (int i = 0; i < 4; i++) {
			m_Animator[i] = GameObject.Find("Player" + (i + 1).ToString()).GetComponent<Animator>();
			m_imageRank[3 - i] = GameObject.Find("Ranking-" + (i + 1).ToString()).GetComponent<Image>();
			m_imageRank[3 - i].enabled = false;
		}
		m_DialogBack = GameObject.Find("Dialog_Back").GetComponent<Image>();
		m_DialogBack.enabled = false;
		m_DialogCursor = GameObject.Find("Dialog_Cursor").GetComponent<Image>();
		m_DialogCursor.enabled = false;
		m_ResultLogo = GameObject.Find("ResultLogo").GetComponent<TextMeshProUGUI>();
		m_ResultLogo.enabled = false;
		m_Controller = GetComponent<Controller.Controller>();
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
			if (m_cam.transform.position == CameraLocation[0].position) {
				m_Phase++;

			}
			break;

		case ResultPhase.LookDown:

			m_ResultLogo.enabled = true;
			iTween.ScaleTo(m_ResultLogo.gameObject, new Vector3(1.2f, 1.2f, 1.2f), 1.5f);
			
			iTween.MoveTo(m_cam.gameObject, iTween.Hash(
				"position", CameraLocation[1].position,
				"time", 2.5f,
				"easetype", "easeOutCirc"
				)
			);
			//iTween.MoveTo(m_cam.gameObject, CameraLocation[1].position, 2.5f);
			iTween.RotateTo(m_cam.gameObject, CameraLocation[1].rotation.eulerAngles, 2.5f);
			if (m_cam.transform.position == CameraLocation[1].position) {
				for (int i = 0; i < 4; i++) {
					// ランキングによってアニメーションを変更
					switch (i) {
					case 0:
						m_Animator[i].SetTrigger("CW_Fulltwist");
						break;
					case 1:
						m_Animator[i].SetTrigger("CW_FlashKick");
						break;
					case 2:
						m_Animator[i].SetTrigger("BHS_FlashKick");
						break;
					case 3:
						break;
					}
				}
				m_Phase++;
			}
			break;

		case ResultPhase.Stop:
			countTimer += Time.deltaTime;
			if(countTimer > 0 && countTimer < 4) {
				m_imageRank[(int)countTimer].enabled = true;
				iTween.FadeTo(m_imageRank[(int)countTimer].gameObject, 1f, 1.5f);
				iTween.ScaleTo(m_imageRank[(int)countTimer].gameObject, new Vector3(1f, 1f), 1.5f);
			}
			// 秒数待機
			if(countTimer >= 8f) {
				m_Phase++;
				countTimer = 0f;
			}
			
			break;

		case ResultPhase.Result:
			// アニメーションさせたり


			// 結果表示したり
			// エフェクト再生したり

			// ま　だ　！　！

			// ダイアログ表示
			if (m_Controller.GetButtonDown(Controller.Button.B)) {
				m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_UI_Menu_Decision);
				m_Phase++;
			}
			break;

		case ResultPhase.ResultIndication:
			m_DialogBack.enabled = true;
			m_DialogCursor.enabled = true;
			iTween.FadeTo(m_DialogBack.gameObject, 1f, 0.5f);
			iTween.FadeTo(m_DialogCursor.gameObject, 1f, 0.5f);
			m_DialogCursor.rectTransform.localPosition = new Vector3(-235f, -140f);
			m_Phase++;
			break;

		case ResultPhase.Info:
			SceneToNext();//次のシーンへ移動(タイトルへ)   
			break;

		case ResultPhase.NextScene:
			SceneChange.Instance._SceneLoadTitle();
			break;
		}



    }


    public void SceneToNext()
    {
		if(countTimer > 0f) {
			SetFade(countTimer / 3);
			countTimer += Time.deltaTime;
			if (countTimer > 3f) {
				SceneChange.Instance._SceneLoadTitle();

			}
			return;
		}

		bool Dialog = true;	// ←/⇒
		if (m_DialogCursor.rectTransform.localPosition.x == 246f) {
			Dialog = false;
		}

		if (m_Controller.GetAxisDown(Axis.L_x) == -1) {
			if (m_DialogCursor.rectTransform.localPosition.x != -235f) {
				m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_UI_Menu_Cursor);
				m_DialogCursor.rectTransform.localPosition = new Vector3(-235f, -140f);
			}
		}
		if (m_Controller.GetAxisDown(Axis.L_x) == 1) {
			if (m_DialogCursor.rectTransform.localPosition.x != 246f) {
				m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_UI_Menu_Cursor);
				m_DialogCursor.rectTransform.localPosition = new Vector3(246f, -140f);
			}
		}

		if (m_Controller.GetButtonDown(Controller.Button.A)) {
			Dialog = false;
		}

		if (m_Controller.GetButtonDown(Controller.Button.B) || m_Controller.GetButtonDown(Controller.Button.A))
        {
			// タイトルへ
			if (Dialog || countTimer > 0) {
				if(countTimer == 0f) {
					m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_UI_Title_Decision);
					countTimer += Time.deltaTime;
				}

			} else {
				m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_UI_Menu_Cancel);
				m_Phase = ResultPhase.Result;
				m_DialogBack.enabled = false;
				m_DialogCursor.enabled = false;
			}
			//移動したいシーンへ移動(タイトルシーン)
			//SceneManager.LoadScene("TitleScene_");
		}

        //ここにフェード終了処理設置？

    }

	void SetFade(float value) {
		float v = value;
		if (v >= 1) {
			v = 1f;
		}
		GameObject.Find("Fade").GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, v);
	}
}
