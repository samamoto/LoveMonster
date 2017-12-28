using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ついてくる系のエフェクトのマネージャ
/// </summary>
public class EffectSpeedUp : MonoBehaviour {

	//public GameObject m_TrailEffect;
	ParticleSystem particle;

	// Use this for initialization
	void Start () {
		particle = GetComponent<ParticleSystem>();
		particle.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if(count > 0) {
			count += Time.deltaTime;
			// 指定時間再生したら非表示にする
			if(count > endTime) {
				count = 0f;
				particle.Stop();
			}
		}
		*/
	}

	/// <summary>
	/// エフェクトを切り替える
	/// </summary>
	/// <param name="flag">false/true</param>
	public void setActive(bool flag, GameObject obj) {
		if (flag) {
			particle.Play();
		} else {
			particle.Stop();
		}
		//count += Time.deltaTime;
	}


}
