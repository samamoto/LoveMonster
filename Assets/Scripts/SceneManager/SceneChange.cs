using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : Singleton<SceneChange>
{

	 private void Update() {

	 }

	 //テスト用
	 public void _DebugInput()
    {
		/*
        if (Input.GetKeyUp("1"))
        {
            _SceneLoadTitle();
        }

        if (Input.GetKeyUp("2"))
        {
            _SceneLoadGame();
        }

        if (Input.GetKeyUp("3"))
        {
            _SceneLoadResult();
        }

        if (Input.GetKeyUp("4"))
        {
            _SceneLoadTutorial();
        }
		*/
    }

    //Scene変更
    public void _SceneLoadTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
    public void _SceneLoadGame()
    {
        SceneManager.LoadScene("GameSceneStage2");
    }
    public void _SceneLoadResult()
    {
        SceneManager.LoadScene("ResultScene");
    }
    public void _SceneLoadTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
	}

	// 2017年12月17日 oyama add
	/// <summary>
	/// 現在のシーンをリロードする
	/// </summary>
	public void _SceneReload() {
		// ここにリロード時のなにかを書く？
		// とりあえず値のリセット
		GlobalParam.GetInstance().resetParameter();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 再読み込み
	}


	/// <summary>
	/// 2017年11月22日 oyama add
	/// PrototypeStageの呼び出しにつかう
	/// </summary>
	public void _SceneLoadString(string name) {
		  SceneManager.LoadScene(name);
	 }

}