using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;

/// <summary>
/// 2017年11月22日 oyama 右スティック回転を暫定的に追加
/// 2017年11月23日 David PS4コントローラで対応に変更、調整できるように
/// 2017年11月23日 oyama AllCameraに調整値を移動,コントローラとの名前一致で回転できるように
/// </summary>


public class ChaseCamera : MonoBehaviour {
    public Transform target;
    public int idNumber; // id of the player(1-4)
    private float verticalSensitivity;
    private float horizontalSensitivity;

    private Vector3 offset;
	public string ControllerName = "Controller (Gamepad F310)";
	GameObject parent;

	// Use this for initialization
	void Start () {
		//transform.position = target.position + offset;
		AllCameraManager gm = transform.root.GetComponent<AllCameraManager>();
		offset = gm.offset;
		verticalSensitivity = gm.verticalSensitivity;
		horizontalSensitivity = gm.horizontalSensitivity;
		transform.position = target.position + offset;
		parent = transform.parent.gameObject;

    }

    // 各フレームで、Update の後に LateUpdate が呼び出されます。
    void LateUpdate()
    {
        float v = Input.GetAxis("Vertical2");
        float h = Input.GetAxis("Horizontal2");
        //float v = Input.GetAxis("Vertical" + idNumber); // for use later with multiplayer
        //float h = Input.GetAxis("Horizontal" + idNumber);	// Vertical1~4の設定がいる（将来的にPlayerManagerから値渡すはず）
															// また、同一コントローラ使う場合設定が被って他人の画面が動くようになるから一度処理を止める

        parent.transform.position = target.position;
		//parent.transform.localEulerAngles += new Vector3(v * verticalSensitivity, h * horizontalSensitivity, 0);
		// コントローラーが接続されていなければ回転しない
		// 接続されているコントローラが手持ちの物と一緒なら
		if(Controller.Controller.GetConnectController() && Controller.Controller.GetConnectControllerName(idNumber) == ControllerName)
			parent.transform.localEulerAngles += new Vector3(v * verticalSensitivity, h * horizontalSensitivity, 0);
		//this.transform.position = target.position + offset;

	}
}
