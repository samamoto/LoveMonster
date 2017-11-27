using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;

/// <summary>
/// 2017年11月22日 oyama 右スティック回転を暫定的に追加
/// 2017年11月23日 David PS4コントローラで対応に変更、調整できるように
/// 2017年11月23日 oyama AllCameraに調整値を移動,コントローラとの名前一致で回転できるように
/// 2017年11月25日 kashima 全体的に改変
/// </summary>

public class ChaseCamera : MonoBehaviour
{
    public GameObject chaseObject;
    public Vector3 Offset;

    private Controller.Controller chaseObjController;

    [SerializeField, Tooltip("距離")] private float distance = 7.0f;
    [SerializeField, Tooltip("最小距離")] private float minDistance = 5.0f;
    [SerializeField, Tooltip("最大距離")] private float maxDistance = 20.0f;
    [SerializeField, Tooltip("移動距離")] private float deltaDistance = 1.0f;

    [SerializeField, Tooltip("仰角")] private float polerAngle = 60.0f;
    [SerializeField, Tooltip("最小仰角")] private float minPolerAngle = 5.0f;
    [SerializeField, Tooltip("最大仰角")] private float maxPolerAngle = 75.0f;

    [SerializeField, Tooltip("方位角")] private float azimuthalAlngle = 270;
    [SerializeField, Tooltip("x軸感度")] private float Cont_X_Secsitivity = 3.0f;
    [SerializeField, Tooltip("y軸感度")] private float Cont_Y_Secsitivity = 3.0f;
    [SerializeField, Tooltip("移動閾値")] private float Cont_Threshold = 0.1f;

    // Use this for initialization
    private void Start()
    {
        ///コントローラ取得
        this.chaseObjController = chaseObject.GetComponent<Controller.Controller>();
    }

    // 各フレームで、Update の後に LateUpdate が呼び出されます。
    private void LateUpdate()
    {
        Vector2 angle = new Vector2(
            chaseObjController.GetAxisRawThreshold(Controller.Axis.R_x, Cont_Threshold),
            chaseObjController.GetAxisRawThreshold(Controller.Axis.R_y, Cont_Threshold));

        updateAngle(angle);

        if (chaseObjController.GetButtonDown(Controller.Button.RStick))
        {
            distance += deltaDistance;
            if (distance > maxDistance)
                distance = minDistance;
        }

        Vector3 lookAtPos = chaseObject.transform.position + Offset;
        this.updatePosition(lookAtPos);
        transform.LookAt(lookAtPos);
    }

    private void updateAngle(Vector2 angle)
    {
        angle.x = azimuthalAlngle - angle.x * Cont_X_Secsitivity;
        ///360度を超えた時0に戻る
        azimuthalAlngle = Mathf.Repeat(angle.x, 360);

        angle.y = polerAngle - angle.y * Cont_Y_Secsitivity;
        polerAngle = Mathf.Clamp(angle.y, minPolerAngle, maxPolerAngle);
    }

    private void updatePosition(Vector3 lookat)
    {
        var da = azimuthalAlngle * Mathf.Deg2Rad;
        var dp = polerAngle * Mathf.Deg2Rad;
        this.transform.position = new Vector3(
            lookat.x + distance * Mathf.Sin(dp) * Mathf.Cos(da),
            lookat.y + distance * Mathf.Cos(dp),
       lookat.z + distance * Mathf.Sin(dp) * Mathf.Sin(da));
    }
}