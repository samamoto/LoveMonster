using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnly;
public class WallInvisible : MonoBehaviour
{
    //共通部分用
    AllCameraManager ACM;

   // private GameObject Target;//rayの向く向き
	private GameObject Target;   // Rayの方向　privateにした prefab更新のたびに毎回設定するのがめんどい
	float[] distance;
    Ray[] ray;
#if UNITY_EDITOR
	[SerializeField, ReadOnly]
#endif
	int myNum;//自分の番号



    // Use this for initialization
    void Start()
    {
        ACM = GetComponentInParent<AllCameraManager>();
		myNum = GetComponent<ChaseCamera>().getCameraID();  // ChaseCameraからカメラIDを取得
		string strPlayer = "Player" + myNum.ToString();
		Target = GameObject.Find(strPlayer);    // Player1~4を探す
		//自分の番号取得
		//myNum = ACM.myNumAll;
		// ACM.myNumAll++;　// 各スクリプトごとに++されるんだから0++で1しかならない　そもそも++で綺麗に1～4が割り振られるとは限らない
        distance = new float[ACM.rayNum];
        ray = new Ray[ACM.rayNum];

    }

    // Update is called once per frame
    void Update()
    {
        // レイの作成
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
            rayTarget += ACM.targetOffset;
            workm = myPos;
            workt = rayTarget - workm;
            ray[i] = new Ray(workm, workt);
            distance[i] = Vector3.Distance(workm, rayTarget);

            //Debug.DrawRay(workm, workt, Color.blue);

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
        //すべてのカメラを判定
        for (int cameraCount = 0; cameraCount < ACM.CameraNum; cameraCount++)
        {
            //レイにあたっているものを判定
            for (int rayCount = 0; rayCount < ACM.rayNum; rayCount++)
            {
                //レイにあたっているもの取得
                ACM.nowHit[myNum, rayCount] = new List<RaycastHit>(Physics.RaycastAll(ray[rayCount], distance[rayCount], ACM.PlayerLayerMask));

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
                    for (int nowCount = 0; nowCount < ACM.nowHit[myNum, rayCount].Count; nowCount++)
                    {
                        //ワーク
                        GameObject GOnow = ACM.nowHit[myNum, rayCount][nowCount].collider.gameObject;
                        if (GOnow == GOold)
                        {
                            rendFlag = true;
                            //半透明にする
                            Translucent(rend);
                            break;
                        }
                    }
                    if (!rendFlag)
                    {
                        //元に戻す
                        RestoreColor(rend);
                    }
                }

                //oldHitの更新
                UpdateRay(myNum, rayCount);
            }


            //レイにあたらないもの（自分と重なるもの）を判定
            for (int oldCount = 0; oldCount < ACM.oldselfHit[cameraCount].Count; oldCount++)
            {
                bool rendFlag = false;
                //ワーク
                GameObject GOold = ACM.oldselfHit[cameraCount][oldCount].transform.gameObject;
                Renderer rend = GOold.transform.GetComponent<Renderer>();
                //rendererのついてないものは無視
                if (rend == null)
                {
                    continue;
                }

                //比較して透明かどうか決める
                for (int nowCount = 0; nowCount < ACM.selfHit[myNum].Count; nowCount++)
                {
                    //ワーク
                    GameObject GOnow = ACM.selfHit[myNum][nowCount].transform.gameObject;
                    if (GOold == GOnow)
                    {
                        rendFlag = true;
                        //Debug.Log("色変わる");
                        Translucent(rend);
                        break;
                    }
                }
                if (!rendFlag)
                {
                    //if(name == "MainCamera1") Debug.Log("色戻る");
                    RestoreColor(rend);
                }
            }

            //oldの更新
            ACM.oldselfHit[myNum] = new List<GameObject>(ACM.selfHit[myNum]);
        }


    }

    /// <summary>
    /// カメラ自身の接触しているものを取得する
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        ACM.selfHit[myNum].Add(other.transform.gameObject);
		/*
        if(name == "MainCamera1")
        {
            //Debug.Log(name + "　が　" +other.name + "　に当たった" + ACM.selfHit[myNum].Count);
        }
		*/
    }

    private void OnTriggerStay(Collider other)
    {
		/*
        if(name == "MainCamera1")
        {
            //Debug.Log(name + "　が　" + other.name + "　に当たってる" + ACM.selfHit[myNum].Count);
        }
		*/
    }

    /// <summary>
    /// カメラが離れたものをよける
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        ACM.selfHit[myNum].Remove(other.transform.gameObject);
		/*
		if (name == "MainCamera1")
        {
            //Debug.Log(name + "　が　" + other.name + "　から離れた" + ACM.selfHit[myNum].Count);
        }
		*/
    }

    /// <summary>
    /// レイの更新
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    void UpdateRay(int c1, int c2)
    {
        ACM.oldHit[c1, c2] = new List<RaycastHit>(ACM.nowHit[c1, c2]);
    }

    /// <summary>
    /// 現状いらない　デバッグ用
    /// </summary>
    private void OnPostRender()
    {
        //Debug.Log(name + "end");
    }
}
