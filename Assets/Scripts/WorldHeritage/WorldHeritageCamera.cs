﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

[System.Serializable]
public struct HeritageCamTrans
{
    [Tooltip("座標")] public Vector3 position;
    [Tooltip("回転")] public Quaternion quaternion;
    [Tooltip("移動時間地点(sec)")] public float switchingTimePont;
}

public class WorldHeritageCamera : MonoBehaviour
{
    [SerializeField] public List<HeritageCamTrans> m_CamTrasList;
    private float startTime, nowTime;
    [SerializeField] private int index = 0;
    public bool endFlg { get; private set; }

    private bool loop = false;

    public void CameraStart()
    {
        startTime = nowTime = Time.time;
        if (m_CamTrasList.Count > 0)
        {
            this.transform.position = m_CamTrasList[0].position;
            this.transform.rotation = m_CamTrasList[0].quaternion;
            index = 0;
            loop = true;
            endFlg = false;
            StartCoroutine(LateFixedUpdate());
        }
        else
        {
            Debug.LogWarning("No camera Transform list");
        }
    }

    private IEnumerator LateFixedUpdate()
    {
        while (loop)
        {
            yield return new WaitForFixedUpdate();
            //時間更新
            nowTime = Time.time;

            if (m_CamTrasList.Count > 0 && index + 1 < m_CamTrasList.Count)
            {
                float duration = nowTime - startTime;
                //次のインデックスの切り替え時間よりも差分時間が大きければカメラを移動
                if (m_CamTrasList[index + 1].switchingTimePont < duration)
                {
                    //インデックスを次へ
                    index++;
                    //座標情報を次のインデックスのものにする
                    this.transform.position = m_CamTrasList[index].position;
                    this.transform.rotation = m_CamTrasList[index].quaternion;
                }
            }
            else
            {
                loop = false;
            }
        }
        endFlg = true;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(WorldHeritageCamera))]
public class HeritageCamEx : Editor
{
    private GameObject cam = null;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Register"))
        {
            if (!cam)
            {
                cam = GameObject.Find("WorldHeritageCamera");
            }
            //現在の座標情報を保存
            HeritageCamTrans trans = new HeritageCamTrans();
            trans.position = cam.GetComponent<Transform>().position;
            trans.quaternion = cam.GetComponent<Transform>().rotation;
            trans.switchingTimePont = cam.GetComponent<WorldHeritageCamera>().m_CamTrasList.Count;
            cam.GetComponent<WorldHeritageCamera>().m_CamTrasList.Add(trans);
        }
        if (GUILayout.Button("Reset"))
        {
            if (!cam)
            {
                cam = GameObject.Find("WorldHeritageCamera");
            }
            cam.GetComponent<WorldHeritageCamera>().m_CamTrasList.Clear();
        }
    }
}

#endif