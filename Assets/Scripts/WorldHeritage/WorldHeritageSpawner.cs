using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldHeritageSpawner : MonoBehaviour
{
    public bool isControll { get; private set; }
    public bool stageRelease { get; set; }
    private GameObject worldHeritageCamera = null;
    private AllPlayerManager allPlayerManager = null;
    private GameObject instanceWorldHeritage = null;

    [SerializeField, Tooltip("ランダムで生成させる世界遺産リスト")] private GameObject[] WorldHeritageList;

    [SerializeField, Tooltip("生成座標")] private Vector3 startPoint;
    [SerializeField, Tooltip("移動後座標")] private Vector3 endPoint;
    [SerializeField, Tooltip("移動時間")] private float MoveTime = 0.0f;

    //デバック起動用
    [SerializeField] private bool debug_Spawn = false;

    //デバッグ解放用
    [SerializeField] private bool debug_Release = false;

    // Use this for initialization
    private void Start()
    {
        worldHeritageCamera = GameObject.Find("WorldHeritageCamera");
        worldHeritageCamera.GetComponent<WorldHeritageCamera>().enabled = false;    //カメラ無効化
        this.allPlayerManager = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (allPlayerManager.getisEntryBonusStage() && this.instanceWorldHeritage == null || this.debug_Spawn && this.instanceWorldHeritage == null)
        {
            this.debug_Spawn = false;
            this.isControll = false;
            allPlayerManager.stopPlayerControl();
            this.Spawn();
        }

        if (this.instanceWorldHeritage != null)
        {
            // カメラを移動終了させ、プレイヤーのコントロールを戻す
            if (this.instanceWorldHeritage.GetComponent<WorldHeritage>().normalizedTime >= 1.0f && this.worldHeritageCamera.GetComponent<WorldHeritageCamera>().endFlg)
            {
                this.allPlayerManager.returnPlayerControl();
                this.worldHeritageCamera.GetComponent<Camera>().enabled = false;
                isControll = true;
            }

            //デバッグよう解放処理
            if ((this.debug_Release || this.stageRelease))
            {
                this.debug_Release = false;
				this.stageRelease = false;
                allPlayerManager.returnPlayerControl();
                this.Release();
            }
        }
    }

    private void Spawn()
    {
        this.worldHeritageCamera.GetComponent<Camera>().enabled = true;
        this.worldHeritageCamera.GetComponent<WorldHeritageCamera>().CameraStart();
        this.instanceWorldHeritage = Instantiate(this.WorldHeritageList[Random.Range(0, this.WorldHeritageList.Length)], Vector3.zero, Quaternion.identity, this.transform);
        this.instanceWorldHeritage.GetComponent<WorldHeritage>().Init(this.startPoint, this.endPoint, this.MoveTime);
    }

    private void Release()
    {
        if (this.instanceWorldHeritage)
        {
            Destroy(this.instanceWorldHeritage);
            this.worldHeritageCamera.GetComponent<Camera>().enabled = false;
            this.instanceWorldHeritage = null;
			GameObject.DestroyImmediate(instanceWorldHeritage);
        }
    }
}