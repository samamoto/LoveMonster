using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnly;


/// <summary>
/// 床のマテリアルのRenderingModeをOpaqueにする　（一つ変えると全部変わる）
/// AllCameraManagerの
/// TargetOffsetのrayNumを3
/// sizeを3
/// yだけを変更　１つ目0.5　二つ目1.2　３つ目2
/// </summary>


public class WallInvisible : MonoBehaviour
{
    //共通部分用
    AllCameraManager ACM;

	private GameObject Target;
	float[] distance;
    Ray[] ray;

    [SerializeField, ReadOnly]
    int myNum;//自分の番号

    // Use this for initialization
    void Start()
    {
        ACM = GetComponentInParent<AllCameraManager>();
		myNum = GetComponent<ChaseCamera>().getCameraID();  // ChaseCameraからカメラIDを取得
		string strPlayer = "Player" + myNum.ToString();
		Target = GameObject.Find(strPlayer);    // Player1~4を探す
		
        //自分の番号取得
        distance = new float[ACM.rayNum];
        ray = new Ray[ACM.rayNum];

    }

    // Update is called once per frame
    void Update()
    {
        CreateRay();
    }

    /// <summary>
    /// レイの作成
    /// </summary>
    void CreateRay()
    {
        Vector3 myPos = transform.position;
        Vector3 workm, workt;
        Vector3 rayTarget;

        for (int i = 0; i < ACM.rayNum; i++)
        {
            rayTarget = Target.transform.position;
            rayTarget += ACM.targetOffset[i];
            workm = myPos;
            workt = rayTarget - workm;
            ray[i] = new Ray(workm, workt);
            distance[i] = Vector3.Distance(workm, rayTarget);

            Debug.DrawRay(workm, workt, Color.blue);

        }
    }

    /// <summary>
    /// 色を元に戻す
    /// </summary>
    /// <param name="rend"></param>
    void RestoreColor(Renderer rend)
    {
        rend.material.shader = Shader.Find("Standard");
    }

    /// <summary>
    /// 半透明にする
    /// </summary>
    /// <param name="rend"></param>
    void Translucent(Renderer rend)
    {
        rend.material.shader = Shader.Find("ShowInsides");
    }

    /// <summary>
    /// 描画の直前に呼ばれる
    /// カメラごとに描画の仕方を変える
    /// </summary>
    private void OnPreRender()
    {
        //当たったやつと当たってたやつ取得
        Collider[] nowCol = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);
        List<Collider> nowColList = new List<Collider>();
        nowColList.AddRange(nowCol);
        List<RaycastHit> nowHit = new List<RaycastHit>();

        //すべてのカメラを判定
        for (int cameraCount = 0; cameraCount < ACM.CameraNum; cameraCount++)
        {
            //レイにあたっているものを判定
            for (int rayCount = 0; rayCount < ACM.rayNum; rayCount++)
            {
                //レイにあたっているもの取得
                nowHit = new List<RaycastHit>(Physics.RaycastAll(ray[rayCount], distance[rayCount], ACM.PlayerLayerMask));

                //oldCount oldHitに入っているオブジェクトの数
                for (int oldCount = 0; oldCount < ACM.oldHit[cameraCount, rayCount].Count; oldCount++)
                {
                    //レンダラーを変えるかどうか
                    bool rendFlag = false;
                    //ワーク
                    GameObject GOold = ACM.oldHit[cameraCount, rayCount][oldCount].collider.gameObject;

                    //変更対象のレンダラー取得
                    Renderer rend = GOold.transform.GetComponent<Renderer>();
                    if (rend == null)
                    {
                        continue;
                    }

                    //nowCount nowHitに入っているオブジェクトの数
                    for (int nowCount = 0; nowCount < nowHit.Count; nowCount++)
                    {
                        GameObject GOnow = nowHit[nowCount].collider.gameObject;

                        if (GOnow == GOold)
                        {
                            rendFlag = true;
                            Translucent(rend);
                            break;
                        }
                    }
                    if (!rendFlag)
                    {
                        RestoreColor(rend);
                    }
                }

                //oldHitの更新
                ACM.oldHit[myNum, rayCount] = new List<RaycastHit>(nowHit);
            }

            //レイにあたらないもの（自分と重なるもの）を判定
            for (int oldCount = 0; oldCount < ACM.oldColList[cameraCount].Count; oldCount++)
            {
                bool nowFlag = false;
                GameObject oldObj = ACM.oldColList[cameraCount][oldCount].gameObject;
                Renderer rend = oldObj.transform.GetComponent<Renderer>();
                //rendererのついてないものは無視
                if (rend == null)
                {
                    continue;
                }

                //レイにすでにぶつかっているものは無視
                for (int rayCount = 0; rayCount < ACM.rayNum; rayCount++)
                {
                    for(int a = 0; a < ACM.oldHit[cameraCount,rayCount].Count;a++)
                    {
                        GameObject checkObj = ACM.oldHit[cameraCount, rayCount][a].transform.gameObject;
                        if(oldObj == checkObj)
                        {
                            nowFlag = true;
                            break;
                        }
                    }
                    if (nowFlag == true)
                        break;
                }
                if (nowFlag == true)
                    continue;

                //比較して透明かどうか決める
                for (int nowCount = 0; nowCount < nowCol.Length; nowCount++)
                {
                    GameObject nowObj = nowCol[nowCount].gameObject;

                    if (oldObj == nowObj)
                    {
                        nowFlag = true;
                        Translucent(rend);
                        break;
                    }
                }
                if (nowFlag == false)
                {
                    RestoreColor(rend);
                }
            }
            ACM.oldColList[myNum] = new List<Collider>(nowColList);
        }
    }
}
