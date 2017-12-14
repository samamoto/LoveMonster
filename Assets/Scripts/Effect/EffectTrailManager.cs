using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ついてくる系のエフェクトのマネージャ
/// AddComponentして使おう
/// </summary>
public class EffectTrailManager : MonoBehaviour {

	//public GameObject m_TrailEffect;
	public float startTime = 0.0f;
	public float endTime = 0.6f;
	private float count = 0.0f;
	ParticleSystem particle;

	// Use this for initialization
	void Start () {
		particle = GetComponent<ParticleSystem>();
		particle.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		if(count > 0) {
			count += Time.deltaTime;
			// 指定時間再生したら非表示にする
			if(count > endTime) {
				count = 0f;
				particle.Stop();
			}
		}
	}

	/// <summary>
	/// エフェクトを切り替える
	/// </summary>
	/// <param name="flag">false/true</param>
	public void setActive(bool flag, GameObject obj) {
		particle.Play();
		count += Time.deltaTime;
	}


}
