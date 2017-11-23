using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera : MonoBehaviour {
    public Transform target;
    public int idNumber; // id of the player(1-4)
    public float verticalSensitivity;
    public float horizontalSensitivity;

    private Vector3 offset;
	AllCameraManager m_AllCam;
	GameObject parent;

	// Use this for initialization
	void Start () {
		//transform.position = target.position + offset;
		AllCameraManager gm = transform.root.GetComponent<AllCameraManager>();
		offset = gm.offset;
		transform.position = target.position + offset;
		parent = transform.parent.gameObject;
    }

    // 各フレームで、Update の後に LateUpdate が呼び出されます。
    void LateUpdate()
    {
        //float v = Input.GetAxis("Vertical2");
        //float h = Input.GetAxis("Horizontal2");
        float v = Input.GetAxis("Vertical" + idNumber); // for use later with multiplayer
        float h = Input.GetAxis("Horizontal" + idNumber);

        //Vector3 axis = transform.TransformDirection(Vector3.up);
        //transform.position = target.position + m_AllCam.offset;
        //transform.RotateAround(target.position + m_AllCam.offset, axis , Time.deltaTime *10f);
        parent.transform.position = target.position;
        parent.transform.localEulerAngles += new Vector3(v * verticalSensitivity, h * horizontalSensitivity, 0);
        //this.transform.position = target.position + offset;

    }
}
