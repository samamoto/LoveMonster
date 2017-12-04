using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 触れたらゴールする（シーンを切り替える）
/// </summary>
public class Goal : MonoBehaviour
{
    static int count = 0;
    public int countmax = 4;
    private void OnTriggerEnter(Collider other)
    {

        switch (other.tag)
        {
            case "Player":
                count++;
                Debug.Log("GOAL Count = " + count);
                if (count >= countmax)
                    SceneManager.LoadScene("ResultScene");
                break;
        }
    }

    public void CountReset()
    {
        count = 0;
    }
}
