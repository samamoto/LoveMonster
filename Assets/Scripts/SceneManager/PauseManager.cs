using UnityEngine;
using System.Collections;

// 追加
using UnityEngine.SceneManagement;
using Controller;

public class PauseManager : MonoBehaviour
{
    private GameObject refController;   // コントローラーの参照先
                                        //public GameObject[] StopObj;			// 動きを止めるもの

    public GameObject OnPanel;

    private bool pauseGame = false;
    private Controller.Controller m_Con;

    private int MenuID = 3;   //デフォルト設定は現在一番下
                              //誤ってポーズを押したときにタイトルに戻ってしまうのを防ぐため、
                              //本来は「ゲーム本編に戻る」の部分に合わせる

    private int MenuID_MAX = 3;   //メニュー項目の最大数

    private int Choice_count;   //選択移動における、一時停止用のカウント

    private GameObject Choice_Image;    //選択用画像

    private Vector3 Choice_pos;

	private bool is_PauseRestriction;   // Pause禁止　2017年12月26日 oyama add

	private void Start()
    {
        refController = GameObject.Find("Player1"); // 1Pコントローラーを使う
        m_Con = refController.GetComponent<Controller.Controller>();
        Choice_Image = GameObject.Find("Choice_Image");//選択用画像の取得
        OnUnPause();

        Choice_pos = Vector3.zero;
        Choice_count = 0;
    }

    public void Update()
    {
		// Pause禁止ならアップデートしない
		if (is_PauseRestriction)
			return;

		if (Input.GetKeyDown(KeyCode.Escape) || m_Con.GetButtonDown(Button.Menu))
        {
            if (Input.GetKeyDown(KeyCode.Escape) || m_Con.GetButtonDown(Button.Menu))
            {
                pauseGame = !pauseGame;
            }
			// キャンセルボタンで解除
			// なんか音がすごく連鎖してできないから諦める
			/*
			if (m_Con.GetButtonDown(Button.A) && pauseGame) {
				pauseGame = !pauseGame;
			}
			*/

            if (pauseGame == true)
            {
                OnPause();
            }
            else
            {
                OnUnPause();
            }
        }
        if (pauseGame == true)
        {
            Menu_UI_update();  //表示更新
        }
    }

    public void OnPause()
    {
        OnPanel.SetActive(true);        // PanelMenuをtrueにする
        Time.timeScale = 0;
        pauseGame = true;

        // ここからポーズかけるクラスを列挙する
        //Todo:ポーズをかける場所
        //! プレイヤーについているコンポーネントをすべて停止
        MonoBehaviour[] mono = GetComponents<MonoBehaviour>();
        foreach (var m in mono)
        {
            m.enabled = !pauseGame;
        }
        this.enabled = true;    // 生き残らせる　めっちゃ力尽く
        m_Con.enabled = true;
        // timeScale=0で停止する
        // ポーズマネージャだけ生き残らせる
        // ⇒ポーズ完成
        // ちゃんとやるならPlayerのコンポーネントだけ、などの対処が必要
    }

    public void OnUnPause()
    {
        OnPanel.SetActive(false);       // PanelMenuをfalseにする
        Time.timeScale = 1;
        pauseGame = false;

        //Todo:ポーズをかける場所
        //! プレイヤーについているコンポーネントをすべて有効に
        MonoBehaviour[] mono = GetComponents<MonoBehaviour>();
        foreach (var m in mono)
        {
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
    public bool getPauseState()
    {
        return pauseGame;
    }

    // 使ってない
    public void OnRetry()
    {
        //SceneManager.LoadScene("Scene_01");
    }

    public void OnResume()
    {
        OnUnPause();
    }

    //半透明画像での選択をアップデートする関数(主に表示)
    public void Menu_UI_update()
    {
        //↑が押されたら   キーボードはI
        if (m_Con.GetAxisDown(Axis.L_y) == 1 || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MenuID--;       //↑にずらす
            if (MenuID < 1) //１より小さくはならない
            {
                MenuID = 1;
            }
        }
        //↓が押されたら   キーボードはK

        if (m_Con.GetAxisDown(Axis.L_y) == -1 || Input.GetKeyDown(KeyCode.K)|| Input.GetKeyDown(KeyCode.DownArrow))
        {
            MenuID++;
            if (MenuID > MenuID_MAX) //項目の最大数より大きくはならない
            {
                MenuID = MenuID_MAX;
            }
        }
		// 決定が押されたら  キーボードは右シフト
		// 決定キーが普通はAだけど、クセで○ボタンの位置を押してしまうのでBにしよう　2017年12月26日 oyama add 
		if (m_Con.GetButtonDown(Button.B) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            //選択している場所によって違う結果を返す(フラグで管理する予定)

            //現在は3項目
            switch (MenuID)
            {
                //タイトルが選択されているなら

                case 1:
					//タイトルに戻る
					//SceneManager.LoadScene("TitleScene");
					SceneChange.Instance._SceneLoadTitle();
                    break;

                //リスタートが選択されているなら

                case 2:
					//リスタートする
					//SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 再読み込み
					SceneChange.Instance._SceneReload();
                    break;

                //戻るが選択されているなら

                case 3:
                    //ゲームに戻る
                    OnUnPause();        //ポーズ解除んご
                    break;

                default: break;
            }
            //画像の表示位置を変更する(選択が変わる)
            //MenuIDの値に応じて、予め指定された箇所に移動させる
        }
        switch (MenuID)
        {
            //タイトルが選択されているなら
            case 1:
                //選択用画像の位置変更
                Choice_pos.y = 82.8f;
                break;

            //リスタートが選択されているなら
            case 2:
                //選択用画像の位置変更
                Choice_pos.y = -20;

                break;

            //戻るが選択されているなら
            case 3:
                //選択用画像の位置変更
                Choice_pos.y = -125;
                break;

            default: break;
        }
		Choice_Image.GetComponent<RectTransform>().localPosition = Choice_pos;
	}

	/// <summary>
	/// Pauseの禁止/解除処理
	/// </summary>
	/// <param name="flag">禁止/解除</param>
	public void PauseRestriction(bool flag) {
		is_PauseRestriction = flag;
	}


}