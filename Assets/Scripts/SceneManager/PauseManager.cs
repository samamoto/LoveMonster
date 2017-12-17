using UnityEngine;
using System.Collections;
// 追加
using UnityEngine.SceneManagement;
using Controller;

public class PauseManager : MonoBehaviour {

	private GameObject refController;	// コントローラーの参照先
	//public GameObject[] StopObj;			// 動きを止めるもの

	public GameObject OnPanel;

	private bool pauseGame = false;
	private Controller.Controller m_Con;

	void Start() {
		refController = GameObject.Find("Player1");	// 1Pコントローラーを使う
		m_Con = refController.GetComponent<Controller.Controller>();
		OnUnPause();
	}

	public void Update() {
		if (Input.GetKeyDown(KeyCode.Escape) || m_Con.GetButtonUp(Button.Menu) || m_Con.GetButtonDown(Button.RB)) {
			if (Input.GetKeyDown(KeyCode.Escape) || m_Con.GetButtonUp(Button.Menu)) {
				pauseGame = !pauseGame;
			}
#if DEBUG
			// デバッグ処理　ポーズしながらRBでシーンをリセットする
			if (m_Con.GetButtonDown(Button.RB)) {
				//SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 再読み込み
				SceneChange.Instance._SceneReload();
			}
#endif
			if (pauseGame == true) {
				OnPause();
			} else {
				OnUnPause();
			}
		}
	}

	public void OnPause() {
		OnPanel.SetActive(true);        // PanelMenuをtrueにする
		Time.timeScale = 0;
		pauseGame = true;

		// ここからポーズかけるクラスを列挙する
		//Todo:ポーズをかける場所
		//! プレイヤーについているコンポーネントをすべて停止
		MonoBehaviour[] mono = GetComponents<MonoBehaviour>();
		foreach(var m in mono) {
			m.enabled = !pauseGame;
		}
		this.enabled = true;    // 生き残らせる　めっちゃ力尽く
		m_Con.enabled = true;
		// timeScale=0で停止する
		// ポーズマネージャだけ生き残らせる
		// ⇒ポーズ完成
		// ちゃんとやるならPlayerのコンポーネントだけ、などの対処が必要
	}

	public void OnUnPause() {
		OnPanel.SetActive(false);       // PanelMenuをfalseにする
		Time.timeScale = 1;
		pauseGame = false;

		//Todo:ポーズをかける場所
		//! プレイヤーについているコンポーネントをすべて有効に
		MonoBehaviour[] mono = GetComponents<MonoBehaviour>();
		foreach (var m in mono) {
			m.enabled = !pauseGame;
		}
		//this.enabled = true;    // 生き残らせる　めっちゃ力尽く
		//m_Con.enabled = true;
	}

	/// <summary>
	/// ポーズ状態の確認
	/// ただ全部止めてるので値を受け取る側はenabled = true;にすること
	/// </summary>
	/// <returns>true:ポーズ中</returns>
	public bool getPauseState() {
		return pauseGame;
	}


}