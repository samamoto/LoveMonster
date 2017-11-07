using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera : MonoBehaviour {
    public Transform target;
    Vector3 offset;
	// Use this for initialization
	void Start () {
       // offset = transform.position - target.position;
    }

    // 各フレームで、Update の後に LateUpdate が呼び出されます。
    void LateUpdate()
    {
       // this.transform.position = target.position + offset;
    }
}
