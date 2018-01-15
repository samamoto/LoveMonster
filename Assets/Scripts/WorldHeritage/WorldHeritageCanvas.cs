using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldHeritageCanvas : MonoBehaviour
{
    private WorldHeritageName m_HeritageName;
    private WorldHeritageInfoBelt m_InfoBelt_Up;
    private WorldHeritageInfoBelt m_InfoBelt_Down;

    // Use this for initialization
    private void Start()
    {
        this.m_HeritageName = GameObject.Find("HeritageName").GetComponent<WorldHeritageName>();
        this.m_InfoBelt_Up = GameObject.Find("InfoBelt_Up").GetComponent<WorldHeritageInfoBelt>();
        this.m_InfoBelt_Down = GameObject.Find("InfoBelt_Down").GetComponent<WorldHeritageInfoBelt>();
    }

    public void InitBelt(float moveSpeed)
    {
        this.m_InfoBelt_Up.m_MoveSpeed = moveSpeed;
        this.m_InfoBelt_Down.m_MoveSpeed = moveSpeed;
    }

    public void InitName(float delayTime, float moveTime, Sprite sprite)
    {
        this.m_HeritageName.Init(delayTime, moveTime, sprite);
    }
}