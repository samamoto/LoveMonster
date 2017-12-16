using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;

/// <summary>
/// 2017年11月22日 oyama 右スティック回転を暫定的に追加
/// 2017年11月23日 David PS4コントローラで対応に変更、調整できるように
/// 2017年11月23日 oyama AllCameraに調整値を移動,コントローラとの名前一致で回転できるように
/// 2017年11月25日 kashima 全体的に改変
/// 2017年12月09日 oyama m_ChaseObjectをプレイヤーをFindで探す仕組みに変更(Prefab更新のたびにリセットされるため)
/// </summary>

public class ChaseCamera : MonoBehaviour
{
	public int PlayerID;
    private GameObject m_ChaseObject;	// privateにした prefab更新のたびに毎回設定するのがめんどい
    public Vector3 Offset;

    private Controller.Controller controller;

    private PauseManager m_Pause;

    [SerializeField, Tooltip("距離")] private float m_Distance = 2.78f;
    [SerializeField, Tooltip("仰角")] private float m_PolarAngle = 85f;
    [SerializeField, Tooltip("方位角")] private float m_AzimuthalAngle = 270f;

    [SerializeField, Tooltip("最小仰角")] private float m_MinPolar = 30f;
    [SerializeField, Tooltip("最大仰角")] private float m_MaxPolar = 85f;

    [SerializeField, Tooltip("x軸感度")] private float m_Cont_x_Secsitivity = 3.0f;
    [SerializeField, Tooltip("y軸感度")] private float m_Cont_y_Secsitivity = 3.0f;
    [SerializeField, Tooltip("x軸移動閾値")] private float m_Cont_x_Threshold = 0.1f;
    [SerializeField, Tooltip("y軸移動閾値")] private float m_Cont_y_Threshold = 0.1f;

    [SerializeField, Tooltip("補間速度倍率")] private float m_LerpTimeMultiply = 5.0f;

    [Tooltip("仰角の初期値")] private float initAzimuthal = 0.0f;
    [Tooltip("仰角の初期値")] private float initPolar = 0.0f;
    [Tooltip("プレイヤーの後ろの方位角")] private float resetAzimuthal = 0.0f;
    [Tooltip("補間時間")] private float lerpTime = 1.0f;

    public Vector3 forward;
    public Vector3 right;

    private Vector3 resetBeginPos;

    // Use this for initialization
    private void Start()
    {
        if (PlayerID == 0)
            PlayerID = 1;
        string strPlayer = "Player" + PlayerID.ToString();
        m_ChaseObject = GameObject.Find(strPlayer);    // Player1~4を探す
                                                       ///コントローラ取得
        //ターゲットのこんとろーらーをげっと
        this.controller = m_ChaseObject.GetComponent<Controller.Controller>();
        //仰角、方位角の初期値保存
        initPolar = m_PolarAngle;
        initAzimuthal = m_AzimuthalAngle;

        m_Pause = GameObject.Find("PauseManager").GetComponent<PauseManager>();

        //LateFixedUpdate実行
        StartCoroutine(LateFixedUpdate());
    }

    // 各フレームで、Update の後に LateUpdate が呼び出されます。
    private void LateUpdate()
    {
        //カメラリセット
        //if (lerpTime == 1.0f && controller.GetButtonDown(Controller.Button.RStick))
        //{
        //    this.lerpTime = 0.0f;
        //    this.resetBeginPos = this.transform.position;
        //    //カメラの仰角、方位角をリセット後に上書き
        //    this.m_PolarAngle = initPolar;
        //    this.m_AzimuthalAngle = resetAzimuthal;
        //}

        //通常動作
        if (lerpTime == 1.0f)
            updateAngle(controller.GetAxisRawThreshold(Controller.Axis.R_x, m_Cont_x_Threshold), controller.GetAxisRawThreshold(Controller.Axis.R_y, m_Cont_y_Threshold));
    }

    private IEnumerator LateFixedUpdate()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            //カメラの現在地をオフセット分移動
            Vector3 lookAtPos = m_ChaseObject.transform.TransformPoint(Offset);

            updatePosition(lookAtPos);
            transform.LookAt(lookAtPos);
            forward = this.transform.forward;
            right = this.transform.right;
        }
    }

    private void updateAngle(float x, float y)
    {
        //方位角計算
        x = m_AzimuthalAngle - x * m_Cont_x_Secsitivity;
        m_AzimuthalAngle = Mathf.Repeat(x, 360);
        //カメラリセット用にターゲットの後ろの方位角を算出
        resetAzimuthal = Mathf.Repeat(((360 - m_ChaseObject.transform.rotation.eulerAngles.y) + initAzimuthal), 360.0f);
        //仰角計算
        y = m_PolarAngle + y * m_Cont_y_Secsitivity;
        m_PolarAngle = Mathf.Clamp(y, m_MinPolar, m_MaxPolar);
    }

    private void updatePosition(Vector3 lookAtPos)
    {
        if (lerpTime >= 1.0f)
        {
            //通常動作
            var da = m_AzimuthalAngle * Mathf.Deg2Rad;
            var dp = m_PolarAngle * Mathf.Deg2Rad;
            Vector3 newPos = new Vector3(
                lookAtPos.x + m_Distance * Mathf.Sin(dp) * Mathf.Cos(da),
                lookAtPos.y + m_Distance * Mathf.Cos(dp),
                lookAtPos.z + m_Distance * Mathf.Sin(dp) * Mathf.Sin(da));
            this.transform.position = newPos;
        }
        else
        {
            //リセット動作
            var da = resetAzimuthal * Mathf.Deg2Rad;
            var dp = initPolar * Mathf.Deg2Rad;
            Vector3 resetEndPos = new Vector3(
               lookAtPos.x + m_Distance * Mathf.Sin(dp) * Mathf.Cos(da),
               lookAtPos.y + m_Distance * Mathf.Cos(dp),
               lookAtPos.z + m_Distance * Mathf.Sin(dp) * Mathf.Sin(da));
            this.transform.position = Vector3.Lerp(this.resetBeginPos, resetEndPos, lerpTime);
            lerpTime = Mathf.Clamp01(lerpTime + Time.deltaTime * m_LerpTimeMultiply);
        }
    }
	/// <summary>
	/// カメラのID取得
	/// </summary>
	public int getCameraID() {
		return PlayerID;
	}
}