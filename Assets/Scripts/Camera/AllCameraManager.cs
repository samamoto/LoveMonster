using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnly;
/// <summary>
/// カメラ初期位置、オフセット値などの設定
/// 設定された値を各Cameraに投げる
/// </summary>
public class AllCameraManager : MonoBehaviour
{
    //レイの当たっているもの
    public List<RaycastHit>[,] oldHit;

    //自分と重なっているもの
    public List<Collider>[] oldColList;

    //レイの数
    [SerializeField]
    public int rayNum;

    public Vector3[] targetOffset;

    //レイヤー番号11のとき
    //ToDo:"Player"レイヤーを何番にしたかで数値が変わる
    public int PlayerLayerMask = 0xf7ff;

    [SerializeField, ReadOnly]
    public int CameraNum;//カメラの数

    private void Awake()
    {
        CameraNum = GetComponentsInChildren<Camera>().Length;
        oldHit = new List<RaycastHit>[CameraNum, rayNum];
        oldColList = new List<Collider>[CameraNum];

        for (int i = 0; i < CameraNum; i++)
        {
            oldColList[i] = new List<Collider>();

            for (int j = 0; j < rayNum; j++)
            {
                oldHit[i, j] = new List<RaycastHit>();
            }
        }
    }

    // Use this for initialization
    void Start()
    {

    }
}
