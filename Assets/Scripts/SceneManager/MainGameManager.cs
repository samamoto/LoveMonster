using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// MainGameManager
//------------------------------------------------------------
// ゲームシーン全体のコントロールを司る
//------------------------------------------------------------
public class MainGameManager : MonoBehaviour {

	// Gameの進行状態を管理する列挙型
	public enum GameState {
		START,
		COUNT,
		GAME,
		PAUSE,
		GOAL,
	}

	private GameState m_NowState;	// 現在ステート

	//スクリプト群
	private SceneChange m_ScreenChange;

    private InputManagerGenerator inputmanage;

    // Use this for initialization
    private void Start()
    {
        m_ScreenChange = GetComponent<SceneChange>();
    }

    // Update is called once per frame
    private void Update()
    {

		// NowStateごとに処理を変える
		// 各処理は別関数に分けて記述する
		switch (m_NowState) {
		// ゲーム開始
		case GameState.START:
			GameStart();
			break;
		// カウントダウン
		case GameState.COUNT:
			CountDown();
			break;
		// ゲーム実行中
		case GameState.GAME:
			GamePlay();
			break;
		// ポーズ処理中
		case GameState.PAUSE:
			Pause();
			break;
		// ゴール
		case GameState.GOAL:
			Goal();
			break;

		default:
			break;

		}


		// DebugSceneChange
		m_ScreenChange._DebugInput();
    }

	/// <summary>
	/// ゲーム開始処理
	/// </summary>
	void GameStart() {
		
	}

	/// <summary>
	/// カウントダウン
	/// </summary>
	void CountDown() {
		
	}

	/// <summary>
	/// ゲーム実行中
	/// </summary>
	void GamePlay() {

	}

	/// <summary>
	/// ポーズ処理
	/// </summary>
	void Pause() {

	}
	/// <summary>
	/// ゴール処理
	/// </summary>
	void Goal() {

	}


	// ============================================================
	// ゲッター
	// ============================================================
	/// <summary>
	/// 現在ステートを取得する
	/// </summary>
	/// <param></param>
	/// <returns>GameState</returns>
	public GameState getNowState() {
		return m_NowState;
	}


}
