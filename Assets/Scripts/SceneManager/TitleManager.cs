using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Controller;
using UnityStandardAssets.ImageEffects;
using TMPro;

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
    private bool MovieEndFlag=false;  //ムービー再生終了フラグ
	private Animator animator;
	private AudioList m_Audio;
	private Controller.Controller m_Controller;
	private NoiseAndScratches m_Noise;
	private Image m_TitleLogo;
	private TextMeshProUGUI m_Press;

	private float timeCount = 0f;
	private bool buttonFlag;
	public enum TitlePhase {
		Awake,
		Start,
		Wait,
		End,
		NextScene,
	};

	TitlePhase m_Phase = TitlePhase.Awake;

	// Use this for initialization
	private void Start()
    {
		GameObject.Find("Camera").GetComponent<NoiseAndScratches>().enabled = true;
		m_Audio = GameObject.Find("SoundManager").GetComponent<AudioList>();
		m_Controller = GetComponent<Controller.Controller>();
		m_TitleLogo = GameObject.Find("TitleLogo").GetComponent<Image>();
		m_TitleLogo.enabled = false;
		m_Press = GameObject.Find("Press").GetComponent<TextMeshProUGUI>();
		m_Press.enabled = false;

	}

	// Update is called once per frame
	private void Update()
    {

		switch (m_Phase) {
		case TitlePhase.Awake:
			m_Audio.Play((int)AudioList.SoundList_BGM.BGM_Game_Stage1);
			m_Phase = TitlePhase.Start;
			break;

		case TitlePhase.Start:
			timeCount += Time.deltaTime * 2;
			if (timeCount >= 5f) {
				GameObject.Find("Camera").GetComponent<NoiseAndScratches>().enabled = false;
				GameObject.Find("Camera").GetComponent<SunShafts>().enabled = true;
				m_Phase = TitlePhase.Wait;
				timeCount = 0f;
			} else {
				GameObject.Find("Camera").GetComponent<NoiseAndScratches>().grainIntensityMin -= Time.deltaTime * 2;
			}
			break;

		case TitlePhase.Wait:
			timeCount += Time.deltaTime;
			iTween.ValueTo(gameObject, iTween.Hash(
				"from", 40f,
				"to", 3.43f,
				"time", 1f,
				"onupdate", "SetIntensity",  // 毎フレーム SetAlpha() を呼びます。
				"easetype", "easeOutCubic"
				));

			m_TitleLogo.enabled = true;
			if(timeCount >= 0 && timeCount < 1f) {
				iTween.FadeTo(m_TitleLogo.gameObject, 1f, 1f);
				iTween.ScaleTo(m_TitleLogo.gameObject, new Vector3(4.1f, 4.1f), 1f);
			}

			if (timeCount >= 2.5f) {
				m_Press.enabled = true;
				iTween.FadeTo(m_Press.gameObject, 1f, 1.0f);
			}

			if (timeCount >= 3f) {
				timeCount = 0f;
				m_Phase++;
				// ロゴを縮小
				iTween.ScaleTo(m_TitleLogo.gameObject, new Vector3(4f, 4f), 1f);
			}
			break;

		case TitlePhase.End:
			if (m_Controller.GetButtonDown(Controller.Button.B) ||
				m_Controller.GetButtonDown(Controller.Button.A) || 
				m_Controller.GetButtonDown(Controller.Button.Menu) ||
				Input.GetKeyDown(KeyCode.Space)) {
				m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_UI_Title_Decision);
				m_Phase = TitlePhase.NextScene;
				m_Press.gameObject.GetComponent<Animator>().enabled = false;
			}

			if (Input.GetKey(KeyCode.Escape)) {
				Application.Quit();
			}

			break;

		case TitlePhase.NextScene:
			SceneToNext();
			break;


		}
    }

    public void SceneToNext()     //次のシーンへ移動
    {
        //m_ScreenChange._DebugInput();
		//移動したいシーンへ移動(チュートリアルシーン)
		timeCount += Time.deltaTime;
		//iTween.FadeTo(GameObject.Find("Panel"), 1f, 2f);
		SetFade(timeCount/3);
		//iTween.ValueTo(gameObject, iTween.Hash("from", 1f, "to", 0f, "time", 1f, "onupdate", "SetFade"));
		if (timeCount > 3.0f) {
			SceneChange.Instance._SceneLoadTutorial();
			//SceneChange.Instance._SceneLoadGame();
		}
	}

	void SetIntensity(float value) {
		GameObject.Find("Camera").GetComponent<SunShafts>().sunShaftIntensity = value;
	}

	void SetFade(float value) {
		float v = value;
		if(v >= 1) {
			v = 1f;
		} 
		GameObject.Find("Fade").GetComponent<UnityEngine.UI.Image>().color = new Color(0,0,0, v);
	}
}
