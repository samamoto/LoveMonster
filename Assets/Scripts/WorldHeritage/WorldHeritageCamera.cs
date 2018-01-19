using System.Collections;
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
    [SerializeField, Tooltip("演出終了後の待機時間")] private float endWaitTime = 1.0f;
    [SerializeField] private float endwaitMovedFoV = 75.0f;
    private float initFoV = 0.0f, FoV_NormalTime = 0.0f;
    private float startTime, nowTime, heritageMoveTime, waitingTime;
    private bool loop = false;
    public int index;
    public bool endFlg { get; private set; }

    public void CameraStart(float _heritageMoveTime)
    {
        startTime = nowTime = Time.time;
        this.heritageMoveTime = _heritageMoveTime;
        this.waitingTime = 0.0f;
        this.initFoV = this.GetComponent<Camera>().fieldOfView;
        this.FoV_NormalTime = 0.0f;
        if (m_CamTrasList.Count > 0)
        {
            //リスト０番目の位置へカメラを移動
            this.transform.position = m_CamTrasList[0].position;
            this.transform.rotation = m_CamTrasList[0].quaternion;
            index = 0;
            loop = true;
            endFlg = false;
            //コルーチン開始
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
            float duration = nowTime - startTime;
            if (duration < this.heritageMoveTime)
            {
                if (index + 1 < this.m_CamTrasList.Count)
                {
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
            }
            else
            {
                if (this.waitingTime < this.endWaitTime)
                {
                    this.waitingTime += Time.deltaTime;
                    this.FoV_NormalTime += Mathf.Clamp01(Time.deltaTime / this.endWaitTime);
                    this.GetComponent<Camera>().fieldOfView = Mathf.Lerp(this.initFoV, this.endwaitMovedFoV, this.FoV_NormalTime);
                }
                else
                {
                    loop = false;
                }
            }
        }
        endFlg = true;
    }

    public void Reset()
    {
        this.GetComponent<Camera>().fieldOfView = this.initFoV;
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
                cam = GameObject.Find("WorldHeritageCamera");

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
                cam = GameObject.Find("WorldHeritageCamera");
            //リストクリア
            cam.GetComponent<WorldHeritageCamera>().m_CamTrasList.Clear();
        }
        if (GUILayout.Button("MovetoIndex"))
        {
            if (!cam)
                cam = GameObject.Find("WorldHeritageCamera");
            //インデックスの番号の位置へカメラを移動
            WorldHeritageCamera camera = cam.GetComponent<WorldHeritageCamera>();
            if (camera.index < camera.m_CamTrasList.Count)
            {
                camera.transform.position = camera.m_CamTrasList[camera.index].position;
                camera.transform.rotation = camera.m_CamTrasList[camera.index].quaternion;
            }
        }
    }
}

#endif