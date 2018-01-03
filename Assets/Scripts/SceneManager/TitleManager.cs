using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Controller;
using UnityStandardAssets.ImageEffects;
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
		//m_ScreenChange = GameObject.Find("SceneChange").GetComponent<SceneChange>();

		//フェード       仕様書によって場所変更アリ(現在は一番最初)
		//追加シーン     ここでフェードを読み込むのもアリ
		//SceneManager.LoadScene("TitleScene", LoadSceneMode.Additive);

		//animator = GameObject.Find("Player1").GetComponent<Animator>();
		//animator.SetTrigger("BHS_FlashKick");
		//ここにムービー再生処理を追加?　→MovieTextureにフラグを追加するべき?
		//MovieEndFlag = false;       //再生フラグ初期化
		GameObject.Find("Camera").GetComponent<NoiseAndScratches>().enabled = true;
		m_Audio = GameObject.Find("SoundManager").GetComponent<AudioList>();
		m_Controller = GetComponent<Controller.Controller>();
	}

	// Update is called once per frame
	private void Update()
    {

		switch (m_Phase) {
		case TitlePhase.Awake:
			m_Audio.Play((int)AudioList.SoundList_BGM.BGM_Game_Stage1);
            
			m_Phase = TitlePhase.Start;
			Debug.Log("next" + m_Phase.ToString());
			break;
		case TitlePhase.Start:
			timeCount += Time.deltaTime * 2;
			if (timeCount >= 5f) {
				GameObject.Find("Camera").GetComponent<NoiseAndScratches>().enabled = false;
				GameObject.Find("Camera").GetComponent<SunShafts>().enabled = true;
				m_Phase = TitlePhase.Wait;
				timeCount = 0f;
				Debug.Log("next" + m_Phase.ToString());
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
			if (timeCount >= 3f) {
				timeCount = 0f;
				m_Phase++;
				Debug.Log("next" + m_Phase.ToString());
			}
			break;
		case TitlePhase.End:
			if (m_Controller.GetButtonDown(Button.B) || m_Controller.GetButtonDown(Button.A) || m_Controller.GetButtonDown(Button.Menu)) {
				m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_UI_Title_Decision);
				m_Phase = TitlePhase.NextScene;
				Debug.Log("next" + m_Phase.ToString());
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
		//SceneManager.LoadScene("TutorialScene");
		timeCount += Time.deltaTime;
		//iTween.FadeTo(GameObject.Find("Panel"), 1f, 2f);
		SetFade(timeCount/3);
		//iTween.ValueTo(gameObject, iTween.Hash("from", 1f, "to", 0f, "time", 1f, "onupdate", "SetFade"));
		/*
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", 0f,
			"to", 1f,
			"time", 2f,
			"looptype", iTween.LoopType.none,
			"onupdate", "SetFade",  // 毎フレーム SetAlpha() を呼びます。
			"onupdatetarget", GameObject.Find("Fade")
			));
		*/
		if (timeCount > 3.0f) {
			//SceneChange.Instance._SceneLoadTutorial();
			SceneChange.Instance._SceneLoadGame();
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
