using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    //テスト用
    public void _DebugInput()
    {
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
    }

    //Scene変更
    public void _SceneLoadTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
    public void _SceneLoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void _SceneLoadResult()
    {
        SceneManager.LoadScene("ResultScene");
    }
    public void _SceneLoadTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }
}