using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ターゲットの方向を向き続ける
/// </summary>
public class ArrowSignTarget : MonoBehaviour
{
    public GameObject Target;   //見てる方向
    public Vector3 offset;      //Modelの向きによる補正

	void Start() {

	}

	void Update()
    {
        //ターゲットの方向を見続ける
        transform.LookAt(Target.transform.position);

        //モデルの向きによる補正
        transform.Rotate(offset);

    }
}
