using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera : MonoBehaviour {
    public Transform target;
    public Vector3 offset;
	 AllCameraManager m_AllCam;
	 GameObject parent;

	// Use this for initialization
	void Start () {
		  //m_AllCam = transform.root.GetComponent<AllCameraManager>();
		  //transform.position = target.position + offset;
		  transform.position = target.position + offset;
		  parent = transform.parent.gameObject;
    }

    // 各フレームで、Update の後に LateUpdate が呼び出されます。
    void LateUpdate()
    {
		  //Vector3 axis = transform.TransformDirection(Vector3.up);
		  //transform.position = target.position + m_AllCam.offset;
		  //transform.RotateAround(target.position + m_AllCam.offset, axis , Time.deltaTime *10f);
		  parent.transform.position = target.position;
		  parent.transform.Rotate(0, Input.GetAxis("Horizontal2"), 0);
		  parent.transform.Rotate(Input.GetAxis("Vertical2"), 0, 0);
		  //this.transform.position = target.position + offset;
	 }
}
