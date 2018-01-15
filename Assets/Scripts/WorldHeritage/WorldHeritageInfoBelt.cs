using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldHeritageInfoBelt : MonoBehaviour
{
    [System.NonSerialized]
    public float m_MoveSpeed = 0.1f;

    private RawImage m_RawImage;

    private void Start()
    {
        this.m_RawImage = this.GetComponent<RawImage>();
    }

    private void Update()
    {
        //UV移動
        Rect move = this.m_RawImage.uvRect;
        move.x = Mathf.Repeat(Time.time * this.m_MoveSpeed, 1);
        this.m_RawImage.uvRect = move;
    }
}