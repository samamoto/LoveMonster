using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct WorldHeritageInfo
{
    public GameObject worldHeritage;
    public Sprite nameImage;
    public bool instanced;
}

public class WorldHeritageSpawner : MonoBehaviour
{
    public bool isControll { get; private set; }
    public bool stageRelease { get; set; }

    //世界遺産カメラ
    private GameObject worldHeritageCamera = null;

    //カメラキャンバスへの参照
    private GameObject WorldHeritageCanvas = null;

    //ゲージを取得するため
    private AllPlayerManager allPlayerManager = null;

    //生成した世界遺産のインスタンス
    private GameObject instanceWorldHeritage = null;

    [SerializeField, Tooltip("ランダムで生成させる世界遺産リスト")] public WorldHeritageInfo[] WorldHeritageList;
    [SerializeField, Tooltip("生成座標")] private Vector3 startPoint;
    [SerializeField, Tooltip("移動後座標")] private Vector3 endPoint;
    [SerializeField, Tooltip("移動時間")] private float MoveTime = 0.0f;
    [SerializeField, Tooltip("帯の移動速度")] private float beltSpeed = 0.1f;
    [SerializeField, Tooltip("名前出現の遅延時間")] private float nameDelayTime = 1.0f;
    [SerializeField, Tooltip("名前出現の移動時間")] private float nameMoveTime = 1.0f;

    //デバック起動用
    [SerializeField] private bool debug_Spawn = false;

    //デバッグ解放用
    [SerializeField] private bool debug_Release = false;

    private float waitingTime = 0.0f;

    // Use this for initialization
    private void Start()
    {
        worldHeritageCamera = GameObject.Find("WorldHeritageCamera");
        this.WorldHeritageCanvas = GameObject.Find("WorldHeritageCamera/HeritageCameraCanvas");
        worldHeritageCamera.GetComponent<Camera>().enabled = false;//カメラ無効化
        this.allPlayerManager = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //出現
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

            //解放処理
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
        int spawnHeritageNum = Random.Range(0, this.WorldHeritageList.Length);
        int trial = 0;
        while (trial++ < 10 && this.WorldHeritageList[spawnHeritageNum].instanced || this.WorldHeritageList[spawnHeritageNum].worldHeritage == null)
        {
            //10回試行(すべてチェックが入っていたらランダムに出る。)
            spawnHeritageNum = Random.Range(0, this.WorldHeritageList.Length);
        }

        this.WorldHeritageList[spawnHeritageNum].instanced = true;
        this.worldHeritageCamera.GetComponent<Camera>().enabled = true;
        this.worldHeritageCamera.GetComponent<WorldHeritageCamera>().CameraStart(this.MoveTime);
        this.WorldHeritageCanvas.GetComponent<WorldHeritageCanvas>().InitBelt(this.beltSpeed);
        this.WorldHeritageCanvas.GetComponent<WorldHeritageCanvas>().InitName(this.nameDelayTime, this.nameMoveTime, this.WorldHeritageList[spawnHeritageNum].nameImage);
        this.instanceWorldHeritage = Instantiate(this.WorldHeritageList[spawnHeritageNum].worldHeritage, Vector3.zero, Quaternion.identity, this.transform);
        this.instanceWorldHeritage.GetComponent<WorldHeritage>().Init(this.startPoint, this.endPoint, this.MoveTime);
        this.stageRelease = false;
    }

    private void Release()
    {
        Destroy(this.instanceWorldHeritage);
        this.worldHeritageCamera.GetComponent<Camera>().enabled = false;
        this.worldHeritageCamera.GetComponent<WorldHeritageCamera>().Reset();
        this.instanceWorldHeritage = null;
    }
}