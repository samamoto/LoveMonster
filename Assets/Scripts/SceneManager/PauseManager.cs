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

    GameObject Choice_Image;    //選択用画像

    Vector3 Choice_pos;



    void Start()
    {
        refController = GameObject.Find("Player1"); // 1Pコントローラーを使う
        m_Con = refController.GetComponent<Controller.Controller>();
        Choice_Image = GameObject.Find("Choice_Image").gameObject;//選択用画像の取得
        OnUnPause();

        Choice_pos = Vector3.zero;
        Choice_count = 0;

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || m_Con.GetButtonUp(Button.Menu) || m_Con.GetButtonDown(Button.RB))
        {
            if (Input.GetKeyDown(KeyCode.Escape) || m_Con.GetButtonUp(Button.Menu))
            {
                pauseGame = !pauseGame;
            }
#if DEBUG
            // デバッグ処理　ポーズしながらRBでシーンをリセットする
            if (m_Con.GetButtonDown(Button.RB))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 再読み込み
            }
#endif
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
        if (m_Con.GetButtonHold(Button.Y) || Input.GetKeyDown(KeyCode.I))
        {

            MenuID--;       //↑にずらす
            if (MenuID < 1) //１より小さくはならない
            {
                MenuID = 1;
            }

        }
        //↓が押されたら   キーボードはK

        if (m_Con.GetButtonHold(Button.X) || Input.GetKeyDown(KeyCode.K))
        {
            MenuID++;
            if (MenuID > MenuID_MAX) //項目の最大数より大きくはならない
            {
                MenuID = MenuID_MAX;
            }

        }
        //決定が押されたら  キーボードは右シフト
        if (m_Con.GetButtonHold(Button.A) || Input.GetKeyDown(KeyCode.RightShift))
        {
            //選択している場所によって違う結果を返す(フラグで管理する予定)


            //現在は3項目
            switch (MenuID)
            {
                //タイトルが選択されているなら

                case 1:
                    //タイトルに戻る
                    SceneManager.LoadScene("TitleScene");
                    break;

                //リスタートが選択されているなら

                case 2:
                    //リスタートする
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 再読み込み
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
                Choice_pos.y = 120;
                Choice_Image.GetComponent<RectTransform>().anchoredPosition = Choice_pos;
                break;

            //リスタートが選択されているなら
            case 2:
                //選択用画像の位置変更
                Choice_pos.y = 0;
                Choice_Image.GetComponent<RectTransform>().anchoredPosition = Choice_pos;

                break;

            //戻るが選択されているなら
            case 3:
                //選択用画像の位置変更
                Choice_pos.y = -120;
                Choice_Image.GetComponent<RectTransform>().anchoredPosition = Choice_pos;

                break;

            default: break;
        }

    }
}


