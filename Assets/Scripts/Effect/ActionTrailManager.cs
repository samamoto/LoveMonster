using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ついてくる系のエフェクトのマネージャ
/// AddComponentして使おう
/// </summary>
public class ActionTrailManager : MonoBehaviour {

	//public GameObject m_TrailEffect;
	public float startTime = 0.0f;
	public float endTime = 0.0f;
	private float count = 0.0f;
	TrailRenderer trail;

	// Use this for initialization
	void Start() {
		trail = GetComponent<TrailRenderer>();
		trail.enabled = false;
	}

	// Update is called once per frame
	void Update() {

	}

	/// <summary>
	/// エフェクトを切り替える
	/// </summary>
	/// <param name="flag">false/true</param>
	public void setActive(bool flag, GameObject obj) {
		trail.enabled = flag;
	}


}
