using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldHeritageSpawner : MonoBehaviour
{
    private GameObject worldHeritageCamera = null;
    private AllPlayerManager allPlayerManager = null;
    private GameObject instanceWorldHeritage = null;
    private Animator HeritageAnimator = null;

    [SerializeField, Tooltip("ランダムで生成させる世界遺産リスト")] private GameObject[] WorldHeritageList;

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
            allPlayerManager.stopPlayerControl();
            this.Spawn();
        }

        if (this.HeritageAnimator && this.HeritageAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && this.worldHeritageCamera.GetComponent<WorldHeritageCamera>().endFlg)
        {
            this.allPlayerManager.returnPlayerControl();
            this.worldHeritageCamera.GetComponent<Camera>().enabled = false;
        }

        //デバッグよう解放処理
        if (this.instanceWorldHeritage && this.debug_Release)
        {
            this.debug_Release = false;
            allPlayerManager.returnPlayerControl();
            this.Release();
        }
    }

    private void Spawn()
    {
        this.worldHeritageCamera.GetComponent<Camera>().enabled = true;
        this.worldHeritageCamera.GetComponent<WorldHeritageCamera>().CameraStart();
        this.instanceWorldHeritage = Instantiate(this.WorldHeritageList[Random.Range(0, this.WorldHeritageList.Length)]);
        this.HeritageAnimator = instanceWorldHeritage.GetComponent<Animator>();
    }

    private void Release()
    {
        if (this.instanceWorldHeritage)
        {
            HeritageAnimator = null;
            Destroy(this.instanceWorldHeritage);
            this.worldHeritageCamera.GetComponent<Camera>().enabled = false;
            this.instanceWorldHeritage = null;
        }
    }
}