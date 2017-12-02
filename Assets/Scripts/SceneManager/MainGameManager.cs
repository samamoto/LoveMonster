using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour {
    //スクリプト群
    private SceneChange m_ScreenChange;
	private AllPlayerManager m_AllPlayerMgr;
    //private InputManagerGenerator inputmanage;

	public enum PhaseLevel {
		Start,
		CountDown,
		Game,
		Pause,
		None,
	};

	private PhaseLevel m_Phase = PhaseLevel.None;

    // Use this for initialization
    private void Start()
    {
		// シーンが生成されたらStart
		m_Phase = PhaseLevel.Start;
        m_ScreenChange = GetComponent<SceneChange>();
		m_AllPlayerMgr = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();

    }

    // Update is called once per frame
    private void Update()
    {

		
		// AllPlayerMgr側でPauseキー押されたことを検知
    }

	/// <summary>
	/// 現在のフェイズを取得する
	/// </summary>
	/// <returns>現在実行中のフェイズ</returns>
	public PhaseLevel getGamePhase() {
		return m_Phase;
	}

	/// <summary>
	/// プレイヤーが死んだ、落ちたときにする処理
	/// </summary>
	private void PlayerDead() {
		
	}

}
