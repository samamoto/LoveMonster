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
	public float endTime = 1.0f;
	private float count = 0.0f;
	TrailRenderer trail;

	// Use this for initialization
	void Start() {
		trail = GetComponent<TrailRenderer>();
		trail.enabled = false;
	}

	// Update is called once per frame
	void Update() {
		if (count > 0) {
			count += Time.deltaTime;
			// 指定時間再生したら非表示にする
			if (count > endTime) {
				count = 0f;
				trail.enabled = false;
			}
		}
	}

	/// <summary>
	/// エフェクトを切り替える
	/// </summary>
	/// <param name="flag">false/true</param>
	public void setActive(bool flag, GameObject obj) {
		trail.enabled = flag;
		count += Time.deltaTime;
	}


}
