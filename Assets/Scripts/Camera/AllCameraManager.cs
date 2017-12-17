using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnly;

/// <summary>
/// カメラ初期位置、オフセット値などの設定
/// 設定された値を各Cameraに投げる
/// </summary>
public class AllCameraManager : MonoBehaviour {
    //微調整
    public Vector3 offset;

    //レイの当たっているもの
    public List<RaycastHit>[,] nowHit;
    public List<RaycastHit>[,] oldHit;

    //自分と重なっているもの
    public List<GameObject>[] selfHit;
    public List<GameObject>[] oldselfHit;

    //レイの数
    [SerializeField]
    public int rayNum;

    public Vector3 targetOffset;

    //レイヤー番号11のとき
    //ToDo:"Player"レイヤーを何番にしたかで数値が変わる
    public int PlayerLayerMask = 0xf7ff;
#if UNITY_EDITOR
	[SerializeField, ReadOnly]
#endif
	public int CameraNum;//カメラの数

    //[HideInInspector]
    //public int myNumAll = 0;


    private void Awake()
    {
        CameraNum = GetComponentsInChildren<Camera>().Length;
    }

    // Use this for initialization
    void Start()
    {
        nowHit = new List<RaycastHit>[CameraNum, rayNum];
        oldHit = new List<RaycastHit>[CameraNum, rayNum];

        selfHit = new List<GameObject>[CameraNum];
        oldselfHit = new List<GameObject>[CameraNum];

        for (int i = 0; i < CameraNum; i++)
        {
            selfHit[i] = new List<GameObject>();
            oldselfHit[i] = new List<GameObject>();
            for (int j = 0; j < rayNum; j++)
            {
                nowHit[i, j] = new List<RaycastHit>();
                oldHit[i, j] = new List<RaycastHit>();
            }
        }
    }
	
	 // Update is called once per frame
	 void Update () {
		
	 }
}
